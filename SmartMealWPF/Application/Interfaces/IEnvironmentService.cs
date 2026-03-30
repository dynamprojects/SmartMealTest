using SmartMealWPF.Domain.Entities;

namespace SmartMealWPF.Application.Interfaces;

public interface IEnvironmentService
{
    Task<IReadOnlyList<EnvVariable>> LoadVariablesAsync();
    Task UpdateVariableAsync(string name, string newValue);
}