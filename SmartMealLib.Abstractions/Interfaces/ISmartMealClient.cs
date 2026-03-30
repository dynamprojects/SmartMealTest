using SmartMealLib.Abstractions.Models;
using SmartMealLib.Abstractions.Options;

namespace SmartMealLib.Abstractions.Interfaces;

public interface ISmartMealClient
{
    SmartMealProtocol Protocol { get; }
    SmartMealAuthenticationType AuthenticationType { get; }
    Task<IEnumerable<Dish>> GetMenuAsync();
    Task<bool> SendOrderAsync(Order order);
}