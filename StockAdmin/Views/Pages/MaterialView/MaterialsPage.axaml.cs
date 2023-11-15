using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.MaterialView;

public partial class MaterialsPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly FinderController _finderController;
    private readonly List<Material> _materials;
    
    public MaterialsPage(ContentControl frame)
    {
        InitializeComponent();
        _materials = new List<Material>();
        _finderController = new FinderController(500, () =>
        {
            ListMaterials.ItemsSource = _materials.Where(x => x.name.ToLower().Contains(Finded.Text.ToLower()));
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
        Material material = (sender as Button)?.DataContext as Material;
        _frame.Content = new AddedMaterialPage(_frame, material);
    }

    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repository = new MaterialRepository();
        
        if (ListMaterials.SelectedItem is Material material)
        {
            await repository.DeleteAsync(material.id);
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