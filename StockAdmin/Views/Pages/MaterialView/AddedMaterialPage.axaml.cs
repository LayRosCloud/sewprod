using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.MaterialView;

public partial class AddedMaterialPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly Material _material;
    
    public AddedMaterialPage(ContentControl frame) : this(frame, new Material())
    {
        
    }
    
    public AddedMaterialPage(ContentControl frame, Material material)
    {
        InitializeComponent();
        _frame = frame;
        _material = material;
        
        DataContext = _material;
    }

    private async void SaveChanges(object? sender, RoutedEventArgs e)
    {
        var repository = new MaterialRepository();
        
        if (_material.id == 0)
        {
            await repository.CreateAsync(_material);
        }
        else
        {
            await repository.UpdateAsync(_material);
        }

        _frame.Content = new MaterialsPage(_frame);
    }

    public override string ToString()
    {
        return "Добавление / Обновление материала";
    }
}