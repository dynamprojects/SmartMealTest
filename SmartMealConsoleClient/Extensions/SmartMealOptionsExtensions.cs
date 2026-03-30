using SmartMealLib.Abstractions.Options;

namespace SmartMealConsoleClient.Extensions;

public static class SmartMealOptionsExtensions
{
    public static string GetFullBaseUrl(this SmartMealAppSettingsOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.BaseUrl))
            return string.Empty;

        var uri = new UriBuilder(options.BaseUrl);

        int port = options.Protocol.ToString().ToLower() switch
        {
            "http" or "rest" => options.HttpPort,
            "grpc" => options.GrpcPort,
            _ => options.HttpPort
        };

        uri.Port = port;

      if (!string.IsNullOrEmpty(options.Protocol.ToString()))
            uri.Scheme = options.Protocol.ToString().ToLower() == "grpc" ? "http" : options.Protocol.ToString().ToLower();

        return uri.ToString().TrimEnd('/');
    }
}