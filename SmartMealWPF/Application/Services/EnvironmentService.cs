using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartMealWPF.Application.Interfaces;
using SmartMealWPF.Domain.Entities;

namespace SmartMealWPF.Application.Services;

public sealed class EnvironmentService : IEnvironmentService
{
    private readonly IVariableRepository _envRepository;
    private readonly IVariableRepository _jsonRepository;
    private readonly IConfiguration _config;
    private readonly ILogger<EnvironmentService> _logger;

    public EnvironmentService(
        IVariableRepository envRepository, 
        IVariableRepository jsonRepository,
        IConfiguration config,
        ILogger<EnvironmentService> logger)
    {
        _envRepository = envRepository;
        _jsonRepository = jsonRepository;
        _config = config;
        _logger = logger;
    }

    public async Task<IReadOnlyList<EnvVariable>> LoadVariablesAsync()
    {
        var names = _config.GetSection("EnvironmentVariables").Get<string[]>() ?? Array.Empty<string>();
        var commentsArray = _config.GetSection("VariableComments").Get<Dictionary<string, string>[]>() 
                            ?? Array.Empty<Dictionary<string, string>>();
        var comments = commentsArray
            .SelectMany(d => d)
            .ToDictionary(pair => pair.Key, pair => pair.Value);

        var result = new List<EnvVariable>();

        for (int i = 0; i < names.Length; i++)
        {
            var name = names[i];
            var value = _envRepository.Get(name) ?? _jsonRepository.Get(name) ?? string.Empty;

            if (string.IsNullOrEmpty(value))
            {
                _envRepository.Set(name, "");
                _jsonRepository.Set(name, "");
                _logger.LogInformation("Переменная {Name} инициализирована пустой строкой", name);
            }

            result.Add(new EnvVariable(i + 1, name, value, comments.GetValueOrDefault(name, "")));
        }

        _logger.LogInformation("Загружено {Count} переменных", result.Count);
        return result;
    }

    public Task UpdateVariableAsync(string name, string newValue)
    {
        var current = _envRepository.Get(name) ?? "";
        if (current == newValue) return Task.CompletedTask;

        _envRepository.Set(name, newValue);
        _jsonRepository.Set(name, newValue);
        _logger.LogInformation("ИЗМЕНЕНИЕ: {Name} > '{Value}'", name, newValue);

        return Task.CompletedTask;
    }
}