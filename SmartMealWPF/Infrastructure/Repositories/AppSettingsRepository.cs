using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using SmartMealWPF.Application.Interfaces;

namespace SmartMealWPF.Infrastructure.Repositories;

public class AppSettingsRepository: IVariableRepository
{
    private readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

    public string? Get(string name)
    {
        var jsonText = File.ReadAllText(_filePath);
        var jsonNode = JsonNode.Parse(jsonText);
        return jsonNode?[name]?.ToString();
    }

    public void Set(string name, string? value)
    {
        var jsonText = File.ReadAllText(_filePath);
        var jsonNode = JsonNode.Parse(jsonText);

        jsonNode![name] = value;

        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(_filePath, jsonNode.ToJsonString(options));
    }
}