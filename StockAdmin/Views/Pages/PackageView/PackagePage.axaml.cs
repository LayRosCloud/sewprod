using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.PackageView;

public partial class PackagePage : UserControl
{
    private readonly ContentControl _frame;
    private readonly List<PackageEntity> _packages;
    private readonly FinderController _finderController;
    
    private PackageEntity? _package;
    private int _currentIndexBtn = 0;
    
    public PackagePage(ContentControl frame)
    {
        InitializeComponent();
        _packages = new List<PackageEntity>();
        _finderController = new FinderController(TimeConstants.Ticks,  FilteringArray);
        _frame = frame;
        Init();
    }

    private async void Init()
    {
        SelectButton(DateTime.Now.Month - 1);
        await InitAsync();
    }
    
    private async Task InitAsync()
    {
        var loadingController = new LoadingController<PartyEntity>(LoadingBorder);
        await loadingController.FetchDataAsync( async () =>
        {
            var repository = new PartyRepository();
            var personRepository = new PersonRepository();
            
            var post = new PostEntity { Name = "CUTTER" };
            
            CbPerson.ItemsSource = (await personRepository.GetAllAsync())
                .Where(x => x.Posts.Contains(post))
                .ToList();
            
            CbPerson.SelectedIndex = 0;
            
            var personEntity = CbPerson.SelectedItem as PersonEntity;
            
            var list = await repository.GetAllAsync(personEntity!.Id);
            
            CbParties.ItemsSource = FindBetweenDate(list);
            
            CbParties.SelectedIndex = 0;
        });
    }

    private void FilteringArray()
    {
        string text = Finder.Text!.ToLower().Trim();
        var list = _packages.Where(x => 
            x.Party.Person.LastName.ToLower()
                .Contains(text) || x.Party.CutNumber.ToLower().Contains(text));
        List.ItemsSource = list;
    }
    
    private void NavigateToAddedPackagesPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedPackagesPage(_frame);
    }
    
    private void NavigateToMoreInformation(object? sender, TappedEventArgs e)
    {
        if ((sender as DataGrid)!.SelectedItem is not PackageEntity package)
        {
            return;
        }
        var page = new ClothOperationView.ClothOperationPage(package, _frame);
        _frame.Content = page;
    }
    
    public override string ToString()
    {
        return PageTitles.Package;
    }

    private void NavigateToEditPage(object? sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not PackageEntity package)
        {
            return;
        }
        _frame.Content = new EditPackagesPage(_frame, package);
    }
    
    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repository = new PackageRepository();

        if (_package == null)
        {
            return;
        }
        await repository.DeleteAsync(_package.Id);
        Init();
    }
    

    private void ShowDeleteWindow(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = true;
    }

    private void TextChanged(object? sender, TextChangedEventArgs e)
    {
        _finderController.ChangeText();
    }
    
    private async void ChangeMonth(object? sender, RoutedEventArgs e)
    {
        var button = (sender as Button)!;
        int index = Convert.ToInt32(button.Content);
        if (_currentIndexBtn == index)
        {
            return;
        }
        SelectButton(index - 1);
        var personEntity = CbPerson.SelectedItem as PersonEntity;
        var repository = new PartyRepository();
        var list = await repository.GetAllAsync(personEntity.Id);
            
        CbParties.ItemsSource = FindBetweenDate(list);
        CbParties.SelectedIndex = 0;
    }

    private void SelectedPackage(object? sender, SelectionChangedEventArgs e)
    {
        _package = (sender as DataGrid)?.SelectedItem as PackageEntity;
    }

    private void SelectButton(int index)
    {
        _currentIndexBtn = index + 1;

        foreach (var control in MonthButtons.Children)
        {
            var button = (Button)control;
            button.Background = ColorConstants.Blue;
            button.FontWeight = FontWeight.Regular;
        }
        if(MonthButtons.Children[index] is not Button monthSelected) return;

        monthSelected.Background = ColorConstants.Green;
        monthSelected.FontWeight = FontWeight.Bold;
    }


    private async void SelectParty(object? sender, SelectionChangedEventArgs e)
    {
        if (CbParties.SelectedItem is not PartyEntity partyEntity)
        {
            List.ItemsSource = null;
            return;
        }
        var loadingController = new LoadingController<PackageEntity>(LoadingBorder);
        await loadingController.FetchDataAsync(async () =>
        {
            var repository = new PackageRepository();
            _packages.Clear();
            var list = await repository.GetAllOnPartyAsync(partyEntity.Id);
            foreach (var item in list)
            {
                item.Party = partyEntity;
                _packages.Add(item);
            }
            List.ItemsSource = _packages;
        });
    }

    private async void SelectPerson(object? sender, SelectionChangedEventArgs e)
    { 
        PersonEntity personEntity = CbPerson.SelectedItem as PersonEntity;
        var loadingController = new LoadingController<PartyEntity>(LoadingBorder);
        await loadingController.FetchDataAsync(async () =>
        {
            var repository = new PartyRepository();
            var list = await repository.GetAllAsync(personEntity.Id);
            
            CbParties.ItemsSource = FindBetweenDate(list);
            
            CbParties.SelectedIndex = 0;
        });
    }
    private IEnumerable<PartyEntity> FindBetweenDate(IEnumerable<PartyEntity> parties)
    {
        var now = DateTime.Now;
        var first = new DateTime(now.Year, _currentIndexBtn, 1);
        var last = new DateTime(now.Year, _currentIndexBtn, 1).AddMonths(1).AddDays(-1);
            
        var sortingArray = parties
            .Where(item => item.DateStart >= first && item.DateStart <= last)
            .ToList();

        return sortingArray;
    }
}