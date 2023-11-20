using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using StockAdmin.Models;
using StockAdmin.Scripts;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.PackageView;

public partial class PackagePage : UserControl
{
    private readonly ContentControl _frame;
    private readonly List<PackageEntity> _packages;
    private readonly FinderController _finderController;
    public PackagePage(ContentControl frame)
    {
        InitializeComponent();
        _packages = new List<PackageEntity>();
        _finderController = new FinderController(500,  async () =>
        {
            var list = _packages.Where(x=> x.Party.Person.LastName.ToLower()
                .Contains(Finder.Text.ToLower()))
                .GroupBy(x=> x.Party.Person.FullName).ToList();
            var packages = new List<GroupedPackages>();
        
            foreach (var groupPackage in list)
            {
                var groupedPackages = new GroupedPackages()
                {
                    FullName = groupPackage.Key,
                    Packages = groupPackage.ToList()
                };
                
                packages.Add(groupedPackages);
            }
            List.ItemsSource = packages;

        });
        _frame = frame;
        Init();
    }

    private async void Init()
    {
        PackageRepository repository = new PackageRepository();
        _packages.AddRange(await repository.GetAllAsync());
        var list = _packages.GroupBy(x=> x.Party.Person.FullName).ToList();
        List<GroupedPackages> packages = new List<GroupedPackages>();
        
        foreach (var groupPackage in list)
        {
            GroupedPackages groupedPackages = new GroupedPackages()
            {
                FullName = groupPackage.Key,
                Packages = groupPackage.ToList()
            };
            packages.Add(groupedPackages);
        }
        
        List.ItemsSource = packages;
    }
    
    private void NavigateToAddedPackagesPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedPackagesPage(_frame);
    }
    
    private void LoadRowPackage(object? sender, DataGridRowEventArgs e)
    {

    }
    
    private void NavigateToMoreInformation(object? sender, TappedEventArgs e)
    {
        
        if ((sender as DataGrid).SelectedItem is PackageEntity package)
        {
            var page = new ClothOperationView.ClothOperationPage(package, _frame);
            _frame.Content = page;
        }
    }
    
    public override string ToString()
    {
        return "Пачки";
    }

    private void NavigateToEditPage(object? sender, RoutedEventArgs e)
    {
        PackageEntity packageEntity = (sender as Button).DataContext as PackageEntity;
        //_frame.Content = new EditPackagesPage(_frame, package, _party);
    }
    
    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repository = new PackageRepository();
        
        if (List.SelectedItem is PackageEntity package)
        {
            await repository.DeleteAsync(package.Id);
        }

        SendNoAnswerOnDeleteItem(sender, e);
        Init();
    }

    private void SendNoAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = false;
    }

    private void ShowDeleteWindow(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = true;
        DeletedMessage.Text =
            "вы действительно уверены, что хотите удалить пачку?" +
            " Восстановить пачку будет нельзя!";
    }

    private void TextChanged(object? sender, TextChangedEventArgs e)
    {
        _finderController.ChangeText();
    }
}