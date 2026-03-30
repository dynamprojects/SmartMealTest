using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SmartMealConsoleClient.Data;
using SmartMealConsoleClient.Strategies;
using SmartMealLib.Abstractions.Interfaces;
using SmartMealLib.Abstractions.Options;
using SmartMealLib.Http.Clients;
using SmartMealLib.Grpc.Clients;

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((_,config) => config
        .AddJsonFile("appsettings.json", optional: false)
        .AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: false)
        .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true))
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(context.Configuration.GetConnectionString("Default")));
        
        var settings = context.Configuration.GetSection("SmartMealOptions").Get<SmartMealAppSettingsOptions>();
        
        services.Configure<SmartMealAppSettingsOptions>(
            context.Configuration.GetSection("SmartMealOptions"));
        
        services.AddHttpClient<HttpSmartMealClient>(client=>
        {
            client.BaseAddress = new Uri(settings.BaseUrl);
        });
        
        services.AddGrpcClient<GrpcSmartMealClient>(o =>
        {
            o.Address = new Uri(settings.BaseUrl);
        });
        
        services.AddSingleton<ISmartMealStrategy, SmartMealStrategy>();
       
        services.AddScoped<ISmartMealClient>(sp => 
        {
            var strategy = sp.GetRequiredService<ISmartMealStrategy>();
            return strategy.GetClient(settings.Protocol);
        });
    }).UseSerilog((context, cfg) => cfg.ReadFrom.Configuration(context.Configuration))
.Build();

await host.StartAsync();



using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();// 
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var client = services.GetRequiredService<ISmartMealClient>();
    
    
    dbContext.Database.EnsureCreated();
    logger.LogInformation("БД и таблица инициализированы (или уже существовали)");
    
    logger.LogInformation("Запуск");
 
    
    IEnumerable<DomainModels.Dish>? dishes = null;
    try
    {
        dishes = await client.GetMenuAsync();
        
        logger.LogInformation("Получено {Count} блюд от сервера", dishes.Count());

        dbContext.Dishes.RemoveRange(dbContext.Dishes);
        dbContext.Dishes.AddRange(dishes);
        await dbContext.SaveChangesAsync();

        foreach (var item in dishes)
        {
            Console.WriteLine($"{item.Name} – {item.Article} – {item.Price}");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Ошибка сервера");
        Console.WriteLine(ex.Message);
        
        await host.StopAsync(); 
    }
    
    
    var order = new DomainModels.Order();

    while (true)
    {
        Console.WriteLine("\nВведите блюда (Код:Количество;Код:Количество...):");
        var input = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Ввод пустой. Повторите.");
            continue;
        }

        var parts = input.Split(';', StringSplitOptions.RemoveEmptyEntries);
        var orderItems = new List<DomainModels.OrderItem>();
        var valid = true;

        foreach (var part in parts)
        {
            var kv = part.Split(':', 2);
            if (kv.Length != 2)
            {
                valid = false;
                break;
            }

            var code = kv[0].Trim();
            if (!double.TryParse(kv[1].Trim(), out var qty) || qty <= 0)
            {
                valid = false;
                break;
            }

            var dish = dishes.FirstOrDefault(m => m.Article == code);
            if (dish == null)
            {
                Console.WriteLine($"Код {code} не найден!");
                valid = false;
                break;
            }

            orderItems.Add(new DomainModels.OrderItem(dish.Id, qty));
        }

        if (!valid)
        {
            Console.WriteLine("Некорректный ввод. Повторите.");
            continue;
        }

        order.Items = orderItems;
        break;
    }

    try
    {
        await client.SendOrderAsync(order);
        Console.WriteLine("УСПЕХ");
        logger.LogInformation("Заказ отправлен успешно. OrderId: {Id}", order.Id);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        logger.LogError(ex, "Ошибка отправки заказа");
    }

    logger.LogInformation("Завершение работы");
    await host.StopAsync();
}


