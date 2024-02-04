using System;
using System.Collections;
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
using StockAdmin.Scripts.Exports.Other;
using StockAdmin.Scripts.Exports.Outputs;
using StockAdmin.Scripts.Exports.Outputs.Interfaces;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Views.Pages.PackageView;

public partial class PackagePage : UserControl
{
    private readonly ContentControl _frame;
    private readonly List<PackageEntity> _packages;
    private readonly DelayFinder _delayFinder;
    private readonly Hashtable _partyEntities;

    private const string AllElements = "Все крои";
    private PackageEntity? _package;
    private int _currentIndexBtn = 0;
    private readonly IRepositoryFactory _factory;
    
    public PackagePage(ContentControl frame)
    {
        InitializeComponent();
        _factory = ServerConstants.GetRepository();
        
        _partyEntities = new Hashtable();
        _packages = new List<PackageEntity>();
        _delayFinder = new DelayFinder(TimeConstants.Ticks,  FilteringArray);
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
            var personRepository = _factory.CreatePersonRepository();
            
            var post = new PostEntity { Name = PostEntity.CutterName, Id = 3};
            var persons = new List<PersonEntity>
            {
                new() { LastName = "Все", FirstName = "закройщики"}
            };
            persons.AddRange((await personRepository.GetAllAsync()).Where(x => x.Posts.Contains(post)).ToList());
            CbPerson.ItemsSource = persons;
            
            CbPerson.SelectedIndex = 0;
        });
    }

    private void FilteringArray()
    {
        string text = Finder.Text!.ToLower().Trim();
        var list = _packages
            .Where(x => x.Party.CutNumber.ToLower().Contains(text) 
                        || x.Person.FullName.Trim().ToLower().Contains(text));
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
        var repository = _factory.CreatePackagesRepository();
        
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
        _delayFinder.ChangeText();
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
        
        await GetOnPersonIdParties();
    }

    private void SelectedPackage(object? sender, SelectionChangedEventArgs e)
    {
        _package = (sender as DataGrid)?.SelectedItem as PackageEntity;
    }

    private void SelectButton(int index)
    {
        _currentIndexBtn = index + 1;

        foreach (var button in MonthButtons.Children.Cast<Button?>())
        {
            button.Background = ColorConstants.Blue;
            button.FontWeight = FontWeight.Regular;
        }

        if (MonthButtons.Children[index] is not Button monthSelected)
        {
            return;
        }

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
        List.ItemsSource = null;
        var loadingController = new LoadingController<PackageEntity>(LoadingBorder);
        await loadingController.FetchDataAsync(async () =>
        {
            
            _packages.Clear();
            var list = await GetPackagesListFromApiOnPartyCutNumber(partyEntity);
            
            _packages.AddRange(FilteringPackages(list));
            List.ItemsSource = _packages;
        });
    }

    private async Task<IEnumerable<PackageEntity>> GetPackagesListFromApiOnPartyCutNumber(PartyEntity partyEntity)
    {
        var repository = _factory.CreatePackagesRepository();
        var list = new List<PackageEntity>();
        
        if (partyEntity.CutNumber == AllElements)
        {
            list.AddRange(await repository.GetAllAsync());
        }
        else
        {
            list.AddRange(await repository.GetAllOnPartyAsync(partyEntity.Id));
        }

        return list;
    }
    
    private IEnumerable<PackageEntity> FilteringPackages(IEnumerable<PackageEntity> list)
    {
        var sortedArray = new List<PackageEntity>();
        foreach (var package in list)
        {
            if (_partyEntities[package.PartyId] is not PartyEntity party)
            {
                continue;
            }
                
            package.Party = party;
            sortedArray.Add(package);
        }
        
        return sortedArray;
    }
    
    private async void SelectPerson(object? sender, SelectionChangedEventArgs e)
    { 
        var loadingController = new LoadingController<PartyEntity>(LoadingBorder);
        await loadingController.FetchDataAsync(async () =>
        {
            await GetOnPersonIdParties();
        });
    }

    private async Task GetOnPersonIdParties()
    {
        var personEntity = CbPerson.SelectedItem as PersonEntity;
            
        _partyEntities.Clear();
        
        var repository = _factory.CreatePartyRepository();
        var list = new List<PartyEntity>();
        if (personEntity.LastName == "Все" && personEntity.FirstName == "закройщики")
        {
            list = await repository.GetAllAsync();
        }
        else
        {
            list = await repository.GetAllAsync(personEntity!.Id);
        }
        var sortedArray = new List<PartyEntity> { new() {CutNumber = AllElements} };

        sortedArray.AddRange(FindBetweenDate(list));

        foreach (var item in sortedArray)
        {
            _partyEntities.Add(item.Id, item);
        }
        
        CbParties.ItemsSource = sortedArray;
        var listForPartiesGrid = new List<PartyEntity>();
        listForPartiesGrid.AddRange(sortedArray);
        listForPartiesGrid.RemoveAt(0);
        Parties.ItemsSource = listForPartiesGrid;
            
        CbParties.SelectedIndex = 0;
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
    
    private void ChangeVisibilityParties(object? sender, RoutedEventArgs e)
    {
        PartiesGrid.IsVisible = !PartiesGrid.IsVisible;
        PartiesBackground.IsVisible = !PartiesBackground.IsVisible;
    }

    private void NavigateToAddPartyPage(object? sender, RoutedEventArgs e)
    {
        var page = new AddedPartyPage(_frame);
        _frame.Content = page;
    }

    private void NavigateToEditPartyPage(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
        {
            throw new ArgumentException("Ошибка! Объект не является кнопкой");
        }

        if (button.DataContext is not PartyEntity partyEntity)
        {
            throw new ArgumentException("Ошибка! Объект не привязан к партиям!");
        }
        var page = new AddedPartyPage(_frame, partyEntity);
        _frame.Content = page;
    }
    
    public override string ToString()
    {
        return PageTitles.Package;
    }

    [Obsolete("Obsolete")]
    private async void ExportToWord(object? sender, RoutedEventArgs e)
    {
        SaveFileDialog dialog = new SaveFileDialog();
        var filters = new List<FileDialogFilter>();
        var filter = new FileDialogFilter();
        filter.Name = "Word (.docx)";
        filter.Extensions = new List<string>() { "docx" };
        filters.Add(filter);
        dialog.Filters = filters;
        string? path = await dialog.ShowAsync(ElementConstants.MainContainer);
        if (path == null)
        {
            return;
        }
        
        dialog.Filters = filters;
        var controller = new WordController();
        IOutputTable outputTable = new PackagesOutput(_packages);
        controller.ExportOnTemplateData(outputTable);
        try
        {
            controller.Save(path);
        }
        catch (Exception)
        {
            ElementConstants.ErrorController.AddErrorMessage("Процесс занят другим. Закройте Word");
        }
    }
}