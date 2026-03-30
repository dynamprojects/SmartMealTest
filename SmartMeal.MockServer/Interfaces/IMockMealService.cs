using Sms.Test;
using Order = SmartMealLib.Abstractions.Models.Order;

namespace SmartMeal.MockServer.Interfaces;

public interface IMockMealService
{
    Task<MenuItem[]> GetMenuAsync(bool withPrice = true);
    Task SendOrderAsync(Order order);
}