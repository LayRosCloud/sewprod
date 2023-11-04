using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.ModelView;

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
    
    public override string ToString()
    {
        return "Модели";
    }
}