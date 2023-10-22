using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages;

public partial class ModelPage : UserControl
{
    private readonly ContentControl _frame;
    public ModelPage(ContentControl frame)
    {
        InitializeComponent();
        _frame = frame;
        InitData();
    }

    private async void InitData()
    {
        ModelRepository repository = new ModelRepository();
        List.ItemsSource = await repository.GetAllAsync();
    }

    private void NavigateToCreatePage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedModelPage(_frame);
    }
}