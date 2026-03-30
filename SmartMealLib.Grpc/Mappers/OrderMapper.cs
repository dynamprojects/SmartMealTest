using Sms.Test;
namespace SmartMealLib.Grpc.Mappers;

public static class OrderMapper
{
    public static OrderItem ToGrpc(this DomainModels.OrderItem item) => new()
    {
        Id = item.DishId,
        Quantity = item.Quantity
    };
}