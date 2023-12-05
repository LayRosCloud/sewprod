using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.MaterialView;

public partial class MaterialsPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly FinderController _finderController;
    private readonly List<MaterialEntity> _materials;
    
    public MaterialsPage(ContentControl frame)
    {
        InitializeComponent();
        
        _materials = new List<MaterialEntity>();
        _finderController = new FinderController(500, FilteringArrayOnText);
        _frame = frame;
        
        Init();
    }

    private async void Init()
    {
        var dataController =
            new DataController<MaterialEntity>(new MaterialRepository(), _materials, ListMaterials);
        
        var loadingController = new LoadingController<MaterialEntity>(LoadingBorder, dataController);
        await loadingController.FetchDataAsync();
    }
    
    public override string ToString()
    {
        return "Материалы";
    }

    private void FilteringArrayOnText()
    {
        ListMaterials.ItemsSource = _materials.Where(x => x.Name.ToLower().Contains(Finded.Text.ToLower()));
    }
    private void NavigateToAddedMaterialPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedMaterialPage(_frame);
    }

    private void NavigateToEditMaterialPage(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        if (button.DataContext is not MaterialEntity material)
        {
            return;
        }
        
        _frame.Content = new AddedMaterialPage(_frame, material);
    }

    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repository = new MaterialRepository();

        if (ListMaterials.SelectedItem is not MaterialEntity material)
        {
            return;
        }
        
        await repository.DeleteAsync(material.Id);
        Init();
    }
    
    private void ShowDeleteWindowMaterial(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = true;
    }

    private void TextChanged(object? sender, TextChangedEventArgs e)
    {
        _finderController.ChangeText();
    }
}