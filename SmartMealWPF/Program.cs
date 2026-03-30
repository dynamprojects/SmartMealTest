using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartMealWPF.Application.Interfaces;
using SmartMealWPF.Application.Services;
using SmartMealWPF.Infrastructure.Repositories;
using SmartMealWPF.Presentation.ViewModels;
using SmartMealWPF.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SmartMealWPF;

public static class Program
{
    public static IHost? AppHost { get; private set; }

    [STAThread]
    public static void Main()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(
                path: "logs/test-sms-wpf-app-.log",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"
            )
            .CreateLogger();

        AppHost = Host.CreateDefaultBuilder()
            .UseSerilog()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(context.Configuration);
                
                services.AddSingleton<IVariableRepository, VariableVariableRepository>();
                services.AddSingleton<IVariableRepository, AppSettingsRepository>();
                
                services.AddSingleton<IEnvironmentService, EnvironmentService>();
                
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>();
            })
            .Build();

        var app = new App();
        app.Run();
    }
}