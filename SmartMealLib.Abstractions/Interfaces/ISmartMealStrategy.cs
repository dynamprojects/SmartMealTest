using SmartMealLib.Abstractions.Options;

namespace SmartMealLib.Abstractions.Interfaces;

public interface ISmartMealStrategy
{
    ISmartMealClient GetClient(SmartMealProtocol protocol);
}