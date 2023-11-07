using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.MaterialView;

public partial class MaterialsPage : UserControl
{
    private readonly ContentControl _frame;
    public MaterialsPage(ContentControl frame)
    {
        InitializeComponent();
        _frame = frame;
        Init();
    }

    private async void Init()
    {
        var repositoryMaterial = new MaterialRepository();
        var repositoryColor = new ColorRepository();

        ListMaterials.ItemsSource = await repositoryMaterial.GetAllAsync();
        ListColors.ItemsSource = await repositoryColor.GetAllAsync();
    }
    
    public override string ToString()
    {
        return "Материалы";
    }

    private void NavigateToAddedMaterialPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedMaterialPage(_frame);
    }

    private void NavigateToAddedColorPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedColorPage(_frame);
    }

    private void NavigateToEditMaterialPage(object? sender, RoutedEventArgs e)
    {
        Material material = (sender as Button)?.DataContext as Material;
        _frame.Content = new AddedMaterialPage(_frame, material);
    }

    private void NavigateToEditColorPage(object? sender, RoutedEventArgs e)
    {
        Color color = (sender as Button)?.DataContext as Color;
        _frame.Content = new AddedColorPage(_frame, color);
    }
}