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
        _finderController = new FinderController(500, () =>
        {
            ListMaterials.ItemsSource = _materials.Where(x => x.Name.ToLower().Contains(Finded.Text.ToLower()));
        });
        _frame = frame;
        Init();
    }

    private async void Init()
    {
        var repositoryMaterial = new MaterialRepository();
        _materials.AddRange(await repositoryMaterial.GetAllAsync());
        ListMaterials.ItemsSource = _materials;
    }
    
    public override string ToString()
    {
        return "Материалы";
    }

    private void NavigateToAddedMaterialPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedMaterialPage(_frame);
    }

    private void NavigateToEditMaterialPage(object? sender, RoutedEventArgs e)
    {
        MaterialEntity materialEntity = (sender as Button)?.DataContext as MaterialEntity;
        _frame.Content = new AddedMaterialPage(_frame, materialEntity);
    }

    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repository = new MaterialRepository();
        
        if (ListMaterials.SelectedItem is MaterialEntity material)
        {
            await repository.DeleteAsync(material.Id);
        }

        SendNoAnswerOnDeleteItem(sender, e);
        Init();
    }
    

    private void SendNoAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        DeletedContainerMaterial.IsVisible = false;
    }

    private void ShowDeleteWindowMaterial(object? sender, RoutedEventArgs e)
    {
        DeletedContainerMaterial.IsVisible = true;
        DeletedMessageMaterial.Text =
            "вы действительно уверены, что хотите удалить материал?" +
            " Восстановить материал будет нельзя!";
    }

    private void TextChanged(object? sender, TextChangedEventArgs e)
    {
        _finderController.ChangeText();
    }
}