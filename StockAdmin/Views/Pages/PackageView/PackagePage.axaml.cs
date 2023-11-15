using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using StockAdmin.Models;
using StockAdmin.Scripts;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.PackageView;

public partial class PackagePage : UserControl
{
    private readonly ContentControl _frame;
    private readonly List<Package> _packages;
    private readonly FinderController _finderController;
    public PackagePage(ContentControl frame)
    {
        InitializeComponent();
        _packages = new List<Package>();
        _finderController = new FinderController(500,  async () =>
        {
            var list = _packages.Where(x=> x.party.person.lastName.ToLower().Contains(Finder.Text.ToLower())).GroupBy(x=> x.party.person.FullName).ToList();
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
        var list = _packages.GroupBy(x=> x.party.person.FullName).ToList();
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
        DataGridRow row = e.Row;
        if (row.DataContext is Package package)
        {
            bool isAdmin = package.person.posts.Contains(new Post(){name = "ADMIN"});

            row.Background = new SolidColorBrush(
                isAdmin 
                ? Color.Parse("#427D9D") 
                : Color.FromRgb(255, 255, 255));
            
            row.Foreground = new SolidColorBrush(
                isAdmin 
                ? Color.FromRgb(255, 255, 255) 
                : Color.FromRgb(0, 0, 0));
            if (package.isEnded)
            {
                row.Foreground = new SolidColorBrush(Colors.Black);
                row.Background = new SolidColorBrush(Colors.White);
            }
            else if (package.isRepeat)
            {
                row.Foreground = new SolidColorBrush(Colors.White);
                row.Background = new SolidColorBrush(Avalonia.Media.Color.Parse("#f49e31"));
            }
            else if (package.isUpdated)
            {
                row.Foreground = new SolidColorBrush(Colors.White);
                row.Background = new SolidColorBrush(Avalonia.Media.Color.Parse("#282828"));
            }
        }
    }
    
    private void NavigateToMoreInformation(object? sender, TappedEventArgs e)
    {
        
        if ((sender as DataGrid).SelectedItem is Package package)
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
        Package package = (sender as Button).DataContext as Package;
        //_frame.Content = new EditPackagesPage(_frame, package, _party);
    }
    
    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repository = new PackageRepository();
        
        if (List.SelectedItem is Package package)
        {
            await repository.DeleteAsync(package.id);
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