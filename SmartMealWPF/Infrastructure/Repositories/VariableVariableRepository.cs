using SmartMealWPF.Application.Interfaces;

namespace SmartMealWPF.Infrastructure.Repositories;

public sealed class VariableVariableRepository : IVariableRepository
{
    public string? Get(string name) 
        => Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);

    public void Set(string name, string? value)
        => Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.User);

}