using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Views.Pages.MaterialView;

public partial class MaterialsPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly DelayFinder _delayFinder;
    private readonly List<MaterialEntity> _materials;
    private readonly IRepositoryFactory _factory;
    
    public MaterialsPage(ContentControl frame)
    {
        InitializeComponent();
        _factory = ServerConstants.GetRepository();
        
        _materials = new List<MaterialEntity>();
        _delayFinder = new DelayFinder(TimeConstants.Ticks, FilteringArrayOnText);
        _frame = frame;
        
        Init();
    }

    private async void Init()
    {
        var dataController =
            new DataController<MaterialEntity>(_factory.CreateMaterialRepository(), _materials, ListMaterials);
        
        var loadingController = new LoadingController<MaterialEntity>(LoadingBorder, dataController);
        await loadingController.FetchDataAsync();
    }
    
    public override string ToString()
    {
        return PageTitles.Material;
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
        var repository = _factory.CreateMaterialRepository();

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
        _delayFinder.ChangeText();
    }
}