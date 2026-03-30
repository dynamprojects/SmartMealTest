using Grpc.Net.Client;
using SmartMealLib.Abstractions.Interfaces;
using SmartMealLib.Abstractions.Models;
using Google.Protobuf.WellKnownTypes;
using SmartMealLib.Abstractions.Options;
using SmartMealLib.Grpc.Mappers;
using Sms.Test;

namespace SmartMealLib.Grpc.Clients;

public class GrpcSmartMealClient : ISmartMealClient, IDisposable
{
    private readonly GrpcChannel _channel;
    private readonly SmsTestService.SmsTestServiceClient _client;

    public GrpcSmartMealClient(string address, SmartMealAuthenticationType authenticationType)
    {
        AuthenticationType = authenticationType;
        _channel = GrpcChannel.ForAddress(address);
        _client = new SmsTestService.SmsTestServiceClient(_channel);
    }

    public SmartMealProtocol Protocol => SmartMealProtocol.Grpc;
    public SmartMealAuthenticationType AuthenticationType { get; }

    public async Task<IEnumerable<Dish>> GetMenuAsync()
    {
        var request = new BoolValue { Value = true }; 
        var response = await _client.GetMenuAsync(request);
        
        if (!response.Success) { throw new Exception($"gRPC Error: {response.ErrorMessage}"); }
        
        return response.MenuItems.Select(item => item.ToDomain());
    }

    public async Task<bool> SendOrderAsync(DomainModels.Order order)
    {
        var request = new GrpcModel.Order();
        request.OrderItems.AddRange(order.Items.Select(i => i.ToGrpc()));
        
        var result = await _client.SendOrderAsync(request);
        return result.Success;
    }

    public void Dispose() => _channel.Dispose();
}