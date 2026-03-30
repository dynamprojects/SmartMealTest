using Sms.Test;
namespace SmartMealLib.Grpc.Mappers;

public static class DishMapper
{
    public static DomainModels.Dish ToDomain(this MenuItem dto) => new ()
    {
        Id = dto.Id,
        Article = dto.Article,
        Name = dto.Name,
        Price = (decimal)dto.Price
    };
}