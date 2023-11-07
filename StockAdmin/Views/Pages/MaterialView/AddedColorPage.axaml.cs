using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.MaterialView;

public partial class AddedColorPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly Color _color;

    public AddedColorPage(ContentControl frame) : this(frame, new Color())
    {
        
    }
    
    public AddedColorPage(ContentControl frame, Color color)
    {
        InitializeComponent();
        _frame = frame;
        _color = color;
        DataContext = _color;
    }


    private async void SaveChanges(object? sender, RoutedEventArgs e)
    {
        var repository = new ColorRepository();
        if (_color.id == 0)
        {
            await repository.CreateAsync(_color);
        }
        else
        {
            await repository.UpdateAsync(_color);
        }

        _frame.Content = new MaterialsPage(_frame);
    }
    public override string ToString()
    {
        return "Добавление / Обновление цвета";
    }
}