using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using StockAdmin.Models;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.PackageView;

public partial class PackagePage : UserControl
{
    private readonly ContentControl _frame;
    private readonly List<PackageEntity> _parties;
    private readonly FinderController _finderController;
    private PackageEntity? _package;
    private int currentIndexBtn = 0;
    public PackagePage(ContentControl frame)
    {
        InitializeComponent();
        _parties = new List<PackageEntity>();
        _finderController = new FinderController(500,  () =>
        {
            string text = Finder.Text!.ToLower().Trim();
            var list = _parties.Where(x => 
                x.Party.Person.LastName.ToLower()
                .Contains(text) || x.Party.CutNumber.ToLower().Contains(text));
            List.ItemsSource = GroupPackages(list);
        });
        _frame = frame;
        Init();
    }

    private async void Init()
    {
        SelectButton(DateTime.Now.Month - 1);
        await InitAsync(DateTime.Now.Month);
    }

    private async Task InitAsync(int month)
    {
        LoadingBorder.IsVisible = true;
        var repository = new PackageRepository();
        _parties.Clear();
        _parties.AddRange((await repository.GetAllAsync(month))!);
        List.ItemsSource = GroupPackages(_parties);
        LoadingBorder.IsVisible = false;
    }
    
    private void NavigateToAddedPackagesPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedPackagesPage(_frame);
    }
    
    private void NavigateToMoreInformation(object? sender, TappedEventArgs e)
    {
        if ((sender as DataGrid)!.SelectedItem is PackageEntity package)
        {
            var page = new ClothOperationView.ClothOperationPage(package, _frame);
            _frame.Content = page;
        }
    }

    private List<GroupedPackages> GroupPackages(IEnumerable<PackageEntity> packages)
    {
        var groupedPackages = packages
            .OrderBy(x=>x.Party!.CutNumber)
            .GroupBy(x => x.Party?.Person?.FullName);
        var listGrouped = new List<GroupedPackages>();
        
        foreach (var package in groupedPackages)
        {
            var groupedPackage = new GroupedPackages
            {
                FullName = package.Key!,
                Packages = package.ToList()
            };
            listGrouped.Add(groupedPackage);
        }

        return listGrouped;
    }
    
    public override string ToString()
    {
        return "Пачки";
    }

    private void NavigateToEditPage(object? sender, RoutedEventArgs e)
    {
        if((sender as Button).DataContext is not PackageEntity package) return;
        _frame.Content = new EditPackagesPage(_frame, package);
    }
    
    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repository = new PackageRepository();
        
        if (_package != null)
        {
            await repository.DeleteAsync(_package.Id);
            Init();
        }

        SendNoAnswerOnDeleteItem(sender, e);
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
    
    private async void ChangeMonth(object? sender, RoutedEventArgs e)
    {
        Button button = (sender as Button)!;
        int index = Convert.ToInt32(button.Content);
        if (currentIndexBtn == index)
        {
            return;
        }
        SelectButton(index - 1);
        await InitAsync(currentIndexBtn);
    }

    private void SelectedPackage(object? sender, SelectionChangedEventArgs e)
    {
        _package = (sender as DataGrid).SelectedItem as PackageEntity;
    }

    private void SelectButton(int index)
    {
        currentIndexBtn = index + 1;

        foreach (Button button in MonthButtons.Children)
        {
            button.Background = new SolidColorBrush(Color.Parse("#c1ddf4"));
            button.FontWeight = FontWeight.Regular;
        }
        if(MonthButtons.Children[index] is not Button monthSelected) return;
        
        monthSelected.Background = new SolidColorBrush(Color.Parse("#95c0a0"));
        monthSelected.FontWeight = FontWeight.Bold;
    }
}