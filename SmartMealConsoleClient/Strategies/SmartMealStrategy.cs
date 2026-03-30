using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SmartMealConsoleClient.Extensions;
using SmartMealLib.Abstractions.Auth;
using SmartMealLib.Abstractions.Interfaces;
using SmartMealLib.Abstractions.Options;
using SmartMealLib.Grpc.Clients;
using SmartMealLib.Http.Clients;

namespace SmartMealConsoleClient.Strategies;

public class SmartMealStrategy : ISmartMealStrategy
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<SmartMealAppSettingsOptions> _settingsOptions;

    public SmartMealStrategy(IServiceProvider serviceProvider,IOptions<SmartMealAppSettingsOptions> settingsOptions)
    {
        _serviceProvider = serviceProvider;
        _settingsOptions = settingsOptions;
    }

    public ISmartMealClient GetClient(SmartMealProtocol protocol)
    {
        var settings = _settingsOptions.Value;
        
        var fullBaseUrl = settings.GetFullBaseUrl();
        
        return protocol switch
        {
            SmartMealProtocol.Http => CreateHttpClient(fullBaseUrl, settings),
            SmartMealProtocol.Grpc => CreateGrpcClient(fullBaseUrl, settings),
            _ => throw new NotSupportedException($"Протокол {protocol} не поддерживается")
        };
    }

    private HttpSmartMealClient CreateHttpClient(string baseUrl, SmartMealAppSettingsOptions settings)
    {
        var httpClient = _serviceProvider.GetRequiredService<HttpClient>();
        httpClient.BaseAddress = new Uri(baseUrl);
        var authType = settings.AuthenticationType;                    
        var credentials = new AuthCredentials(settings.Username, settings.Password);                          

        return new HttpSmartMealClient(httpClient, authType, credentials);
    }

    private GrpcSmartMealClient CreateGrpcClient(string baseUrl, SmartMealAppSettingsOptions settings)
    {
        
        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new InvalidOperationException("BaseUrl не указан в настройках");

        var address = baseUrl;
        var authType = settings.AuthenticationType;

        return new GrpcSmartMealClient(address, authType);
    }
}