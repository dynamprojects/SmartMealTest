using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using SmartMealLib.Abstractions.Auth;
using SmartMealLib.Abstractions.Interfaces;
using SmartMealLib.Abstractions.Options;
using SmartMealLib.Http.Dto;

namespace SmartMealLib.Http.Clients;

public class HttpSmartMealClient : ISmartMealClient
{
    private readonly HttpClient _httpClient;
    
    private readonly string _endpointPath = "api/main/";
    public SmartMealProtocol Protocol => SmartMealProtocol.Http;
    public SmartMealAuthenticationType AuthenticationType { get; }

    public HttpSmartMealClient(HttpClient httpClient, SmartMealAuthenticationType authenticationType, AuthCredentials? credentials)
    {
        _httpClient = httpClient;
        AuthenticationType = authenticationType;
        
        //про взаимодействие через единый эндпоинт надеюсь правильно понял ожидаемую реализацию
        
        //тут можно еще под варианты аутентификации типа NTLM и т.д продумать для масштабируемости
        var prepCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{credentials.Login}:{credentials.Password}"));
        _httpClient.DefaultRequestHeaders.Authorization = new ("Basic", prepCredentials);
    }
    
    public async Task<IEnumerable<DomainModels.Dish>> GetMenuAsync()
    {
        var requestBody = new { Command = "GetMenu" };
        var response = await SendRequestAsync<GetMenuResponse>(requestBody);
        
        if (response is not { Success: true })
        {
            throw new Exception($"Error {response?.ErrorMessage ?? "Unknown"}");
        }
        
        return response.Data.MenuItems.Select(dto => new DomainModels.Dish
        {
            Id = dto.Id,
            Article = dto.Article,
            Name = dto.Name,
            Price = (decimal)dto.Price
        });
    }

    public async Task<bool> SendOrderAsync(DomainModels.Order order)
    {
        var requestBody = new 
        { 
            Command = "SendOrder", 
            Parameters = order 
        };
        
        var response = await SendRequestAsync<SendOrderResponse>(requestBody);
        return response.Success;
    }
    
    private async Task<T?> SendRequestAsync<T>(object body)
    {
        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var httpResponse = await _httpClient.PostAsync(_endpointPath, content);
        
        var responseJson = await httpResponse.Content.ReadAsStringAsync();
        
        return JsonSerializer.Deserialize<T>(responseJson, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        });
    }
}