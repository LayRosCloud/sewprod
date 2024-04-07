using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Extensions;
using StockAdmin.Scripts.Records;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;
using StockAdmin.Scripts.Statistic;
using StockAdmin.Scripts.Statistic.Records;

namespace StockAdmin.Views.Pages.StatisticPeople;

public partial class StatisticPage : UserControl
{
    private readonly List<PackageEntity> _packages;
    private readonly List<ClothOperationPersonEntity> _clothOperationPersons;
    private readonly Dictionary<string, Action> _categories;
    private readonly IRepositoryFactory _factory;
    private readonly PersonEntity _person;

    public StatisticPage(PersonEntity person)
    {
        InitializeComponent();
        _factory = ServerConstants.GetRepository();
        _categories = new Dictionary<string, Action>();
        _person = person;

        _packages = new List<PackageEntity>();
        _clothOperationPersons = new List<ClothOperationPersonEntity>();
        FullName.Text = person.FullName;

        TryInitAsync(person.Id);
    }

    private async void TryInitAsync(int personId)
    {
        try
        {
            InitAsync(personId);
        }
        catch (Exception)
        {
            ElementConstants.ErrorController.AddErrorMessage(Constants.UnexpectedAdminExceptionMessage);
        }
    }
    
    private async void InitAsync(int personId)
    {
        ClearLists();
        
        await FetchClothOperationPersonsAsync(personId);
        var partyList = await FetchPartiesAsync();
        var packageList = await FetchPackagesAsync(personId);
        
        ConnectPackagesAndParties(partyList, packageList);

        InitializeFields(partyList);

        (var walletOperations, var fullSum) = CalcFullSumAndConvertToDisplayData(partyList);
        
        DataGrid.ItemsSource = walletOperations;
        FullSum.Text = fullSum.ToString("F2");
    }

    private void ClearLists()
    {
        _packages.Clear();
        _clothOperationPersons.Clear();
        _categories.Clear();
    }
    
    private void InitializeFields(IEnumerable<PartyEntity> parties)
    {
        if (PackageAndPartyOnly.IsChecked == true)
        {
            var options = new StatisticOptions<PackageEntity>
            {
                Chart = Chart,
                OnGraphicClick = Initial,
                Source = _packages,
                CurrentFilterMonthText = FilterSum
            };
            var packagesStatistic = new PackagesStatistic(options);
            _categories.Add("Выкройки", packagesStatistic.Generate);
        }
        else
        {
            var options = new StatisticOptions<PartyEntity>
            {
                Chart = Chart,
                OnGraphicClick = Initial,
                Source = parties,
                CurrentFilterMonthText = FilterSum
            };
            var packagesStatistic = new PartyStatistic(options);
            _categories.Add("Крои", packagesStatistic.Generate);
        }
        var optionsOperations = new StatisticOptions<ClothOperationPersonEntity>
        {
            Chart = Chart,
            OnGraphicClick = Initial,
            Source = _clothOperationPersons,
            CurrentFilterMonthText = FilterSum
        };
        var clothOperationStatistic = new ClothOperationStatistic(optionsOperations);
        _categories.Add("Операции", clothOperationStatistic.Generate);
        clothOperationStatistic.Generate();
        Categories.Items.Clear();
        
        foreach (var key in _categories.Keys)
        {
            Categories.Items.Add(key);
        }
        
        Categories.SelectedIndex = 1;
    }
    
    private (List<WalletOperation>, double) CalcFullSumAndConvertToDisplayData(List<PartyEntity> parties)
    {
        var walletOperations = new List<WalletOperation>();
        var fullSum = GetSumAndAddOnListClothOperations(walletOperations);
        
        if (PackageAndPartyOnly.IsChecked == true)
        {
            fullSum += GetSumAndAddOnListPackages(walletOperations);
        }
        else
        {
            fullSum += GetSumAndAddOnListParties(walletOperations, parties);
        }
        

        return (walletOperations, fullSum);
    }

    private double GetSumAndAddOnListClothOperations(List<WalletOperation> walletOperations)
    {
        double fullSum = 0;
        foreach (var clothOperation in _clothOperationPersons)
        {
            walletOperations.Add(ConvertToDisplayData(clothOperation));
            fullSum += GetPrice(clothOperation);
        }

        return fullSum;
    }
    
    private double GetSumAndAddOnListPackages(List<WalletOperation> walletOperations)
    {
        double fullSum = 0;
        foreach (var item in _packages)
        {
            walletOperations.Add(ConvertToDisplayData(item));
            fullSum += GetPrice(item);
        }

        return fullSum;
    }
    
    private double GetSumAndAddOnListParties(List<WalletOperation> walletOperations, List<PartyEntity> parties)
    {
        double fullSum = 0;
        DateTime now = DateTime.Now;
        (DateTime firstDay, DateTime lastDay) = now.GetTwoDates();
        
        foreach (var item in parties)
        {
            if (item.DateStart >= firstDay && item.DateStart <= lastDay && item.Person.Id == _person.Id)
            {
                walletOperations.Add(ConvertToDisplayData(item));
                fullSum += GetPrice(item);
            }
        }

        return fullSum;
    }
    
    private double GetPrice(ClothOperationPersonEntity item)
    {
        return item.ClothOperation?.Price?.Number ?? 0;
    }
    
    private double GetPrice(PackageEntity item)
    {
        return item.Party?.Price?.Number ?? 0;
    }
    
    private double GetPrice(PartyEntity item)
    {
        return item.Price?.Number ?? 0;
    }
    
    private WalletOperation ConvertToDisplayData(ClothOperationPersonEntity item)
    {
        return new WalletOperation
        {
            Name = item.ClothOperation?.Operation?.Name ?? "",
            Cost = item.ClothOperation?.Price?.Number ?? 0
        };
    }
    
    private WalletOperation ConvertToDisplayData(PartyEntity item)
    {
        return new WalletOperation
        {
            Name = item.CutNumber,
            Cost = item.Price?.Number ?? 0
        };
    }

    private WalletOperation ConvertToDisplayData(PackageEntity item)
    {
        return new WalletOperation()
        {
            Name = item.Party?.CutNumber + " " + item.Size?.Number,
            Cost = item.Party?.Price?.Number ?? 0
        };
    }
    
    private async Task FetchClothOperationPersonsAsync(int personId)
    {
        DateTime now = DateTime.Now;
        (DateTime firstDay, DateTime lastDay) = now.GetTwoDates();
        var repository = _factory.CreateClothOperationPersonRepository();
        var list = await repository.GetAllAsync(personId);
        foreach (var item in list)
        {
            if (item.DateStart >= firstDay && item.DateStart <= lastDay && item.IsEnded)
            {
                _clothOperationPersons.Add(item);
            }
        }
    }

    private void ConnectPackagesAndParties(List<PartyEntity> parties, List<PackageEntity> packages)
    {
        var hashtable = ConvertToHashtableById(parties);
        var now = DateTime.Now;
        (var firstDay, var lastDay) = now.GetTwoDates();
        foreach (var item in packages)
        {
            item.Party = (PartyEntity)hashtable[item.PartyId];
            if (item.CreatedAt >= firstDay && item.CreatedAt <= lastDay)
            {
                _packages.Add(item);
            }
        }
    }
    
    private async Task<List<PackageEntity>> FetchPackagesAsync(int personId)
    {
        var packageRepository = _factory.CreatePackagesRepository();
        var list = await packageRepository.GetAllAsync(personId);
        return list;
    }
    
    private async Task<List<PartyEntity>> FetchPartiesAsync()
    {
        var partyRepository = _factory.CreatePartyRepository();
        var partyList = await partyRepository.GetAllAsync();
        return partyList;
    }
    
    private Hashtable ConvertToHashtableById(List<PartyEntity> entities)
    {
        Hashtable hashtable = new Hashtable();

        foreach (var item in entities)
        {
            hashtable.Add(item.Id, item);
        }

        return hashtable;
    }

    public override string ToString()
    {
        return PageTitles.Statistic;
    }

    private void GroupItems_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
         if (Categories.SelectedItem is not string item)
         {
             return;
         }
         List.ItemsSource = new List<WalletOperation>();
         Result.Text = "";
         Count.Text = "";
         _categories[item].Invoke();
    }

    private void Initial(List<WalletOperation> items, double sum)
    {
        Result.Text = sum.ToString("F2") + " Р.";
        Count.Text = items.Count.ToString();
        List.ItemsSource = items;
    }

    private void Refresh(object? sender, RoutedEventArgs e)
    {
        InitAsync(_person.Id);
    }
}