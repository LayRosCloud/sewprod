using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using StockAdmin.Scripts.Constants;

namespace StockAdmin.Scripts.Controllers;

public class LoadingController<TEntity>
{
    private readonly Decorator _panel;
    private readonly DataController<TEntity>? _dataController;

    public LoadingController(Decorator panel, DataController<TEntity> dataController)
    {
        _panel = panel;
        _dataController = dataController;
    }
    
    public LoadingController(Decorator panel)
    {
        _panel = panel;
        _dataController = null;
    }

    public async Task FetchDataAsync()
    {
        _panel.IsVisible = true;
        
        if (_dataController == null)
        {
            throw new ArgumentException("Error! DataController is empty!");
        }

        try
        {
            await _dataController.FetchDataAsync();
            _panel.IsVisible = false;
        }
        catch (Exception)
        {
            ElementConstants.ErrorController.AddErrorMessage("Возникла ошибка с получением данных");
        }
    }
    
    public async Task FetchDataAsync(Func<Task> action)
    {
        _panel.IsVisible = true;
        await action.Invoke();
        _panel.IsVisible = false;
    }
}