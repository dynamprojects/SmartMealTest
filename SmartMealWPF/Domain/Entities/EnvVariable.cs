namespace SmartMealWPF.Domain.Entities;

public sealed class EnvVariable
{
    public int Number { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Comment { get; init; } = string.Empty;

    public EnvVariable(int number, string name, string value, string comment)
    {
        Number = number;
        Name = name;
        Value = value;
        Comment = comment;
    }
}