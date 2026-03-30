using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartMealWPF.Application.Interfaces;
using SmartMealWPF.Application.Services;
using SmartMealWPF.Domain.Entities;

namespace SmartMealWPF.Presentation.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IEnvironmentService _service;
    private readonly ILogger<EnvironmentService> _logger;
    
    [ObservableProperty] private ObservableCollection<EnvVariable> variables = new();

    public IAsyncRelayCommand LoadCommand { get; }
    public IAsyncRelayCommand<EnvVariable> UpdateCommand { get; }
    
    public IRelayCommand MinimizeCommand { get; }
    
    public IRelayCommand CloseCommand { get; }

    
    
    public MainViewModel(IEnvironmentService service,ILogger<EnvironmentService> logger)
    {
        _service = service;
        _logger = logger;

        LoadCommand = new AsyncRelayCommand(LoadVariables);
        UpdateCommand = new AsyncRelayCommand<EnvVariable>(UpdateVariable);

        MinimizeCommand = new RelayCommand(MinimizeWindow);
        CloseCommand = new RelayCommand(CloseWindow);
        
        _ = LoadCommand.ExecuteAsync(null);
    }

    private async Task LoadVariables()
    {
        var list = await _service.LoadVariablesAsync();
        Variables = new ObservableCollection<EnvVariable>(list);
    }

    private async Task UpdateVariable(EnvVariable? item)
    {
        if (item is null) return;
        await _service.UpdateVariableAsync(item.Name, item.Value);
    }
    
    private void MinimizeWindow()
    {
        _logger.LogInformation("Пользователь нажал кнопку минимизации окна");
    }

    private void CloseWindow()
    {
        _logger.LogInformation("Пользователь нажал кнопку закрытия окна");
    }
}