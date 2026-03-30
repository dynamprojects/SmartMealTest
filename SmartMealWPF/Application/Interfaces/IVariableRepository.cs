namespace SmartMealWPF.Application.Interfaces;

public interface IVariableRepository
{
    string? Get(string name);
    void Set(string name, string? value);
}