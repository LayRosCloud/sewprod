using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages;

public partial class AddedModelPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly Model _model;
    
    public AddedModelPage(ContentControl frame) : this(frame, new Model())
    {
    }
    
    public AddedModelPage(ContentControl frame, Model model)
    {
        InitializeComponent();
        _frame = frame;
        _model = model;
        
        DataContext = _model;
    }

    private async void SaveChanges(object? sender, RoutedEventArgs e)
    {
        var repository = new ModelRepository();

        if (_model.id == 0)
        {
            await repository.CreateAsync(_model);
        }
        else
        {
            await repository.UpdateAsync(_model);
        }

        _frame.Content = new ModelPage(_frame);
    }
}