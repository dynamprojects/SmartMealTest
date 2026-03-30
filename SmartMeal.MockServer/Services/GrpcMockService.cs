using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Sms.Test;
using GetMenuResponse = Sms.Test.GetMenuResponse;
using SendOrderResponse = Sms.Test.SendOrderResponse;

namespace SmartMeal.MockServer.Services;

public class GrpcMockService : SmsTestService.SmsTestServiceBase
{
    public override Task<GetMenuResponse> GetMenu(BoolValue request, ServerCallContext context)
    {
        return Task.FromResult(new GetMenuResponse {
            Success = true,
            MenuItems =
            {
                new MenuItem {                         
                    Id = "5979224",
                    Article = "A1004292",
                    Name = "Каша гречневая",
                    Price = 50,
                    IsWeighted = false,
                    FullPath = "ПРОИЗВОДСТВО\\Гарниры"
                },
                new MenuItem {  Id = "9084246",
                    Article = "A1004293",
                    Name = "Конфеты Коровка",
                    Price = 300,
                    IsWeighted = true,
                    FullPath = "ДЕСЕРТЫ\\Развес"
                }
            }
        });
    }

    public override Task<SendOrderResponse> SendOrder(Order request, ServerCallContext context)
    {
        return Task.FromResult(new SendOrderResponse { Success = true });
    }
}