using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Extensions;
using StockAdmin.Scripts.Records;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Repositories.Server;
using StockAdmin.Scripts.Server;
using StockAdmin.Scripts.Statistic;

namespace StockAdmin.Views.Pages.StatisticPeople;

public partial class StatisticPage : UserControl
{
    private readonly List<PackageEntity> _packages;
    private readonly List<ClothOperationPersonEntity> _clothOperationPersons;
    private readonly Dictionary<string, Action> _categories;
    private ClothOperationStatistic _clothOperationStatistic;
    private PackagesStatistic _packagesStatistic;
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

        InitAsync(person.Id);
    }

    private async void InitAsync(int personId)
    {
        _packages.Clear();
        _clothOperationPersons.Clear();
        _categories.Clear();
        var repository = _factory.CreateClothOperationPersonRepository();
        var packageRepository = _factory.CreatePackagesRepository();
        var partyRepository = _factory.CreatePartyRepository();
        
        double fullSum = 0;
        
        DateTime now = DateTime.Now;
        (DateTime firstDay, DateTime lastDay) = now.GetTwoDates();
        
        var list = await repository.GetAllAsync(personId);
        var packageList = await packageRepository.GetAllAsync(personId);

        var partyList = await partyRepository.GetAllAsync();

        var hashtable = new Hashtable();

        foreach (var party in partyList)
        {
            hashtable.Add(party.Id, party);
        }
        
        foreach (var item in list)
        {
            if (item.DateStart >= firstDay && item.DateStart <= lastDay && item.IsEnded)
            {
                _clothOperationPersons.Add(item);
            }
        }
        
        foreach (var item in packageList!)
        {
            item.Party = (PartyEntity)hashtable[item.PartyId];
            if (item.CreatedAt >= firstDay && item.CreatedAt <= lastDay)
            {
                _packages.Add(item);
            }
        }
        
        _clothOperationStatistic = new ClothOperationStatistic(Chart, _clothOperationPersons, Initial);
        _packagesStatistic = new PackagesStatistic(Chart, _packages, Initial);
        
        _clothOperationStatistic.Generate();
        
        _categories.Add("Выкройки", _packagesStatistic.Generate);
        _categories.Add("Операции", _clothOperationStatistic.Generate);
        
        Categories.ItemsSource = _categories.Keys;
        Categories.SelectedIndex = 1;
        
        List<WalletOperation> walletOperations = new List<WalletOperation>();
        foreach (var clothOperation in _clothOperationPersons)
        {
            walletOperations.Add(new WalletOperation()
                {
                    Name = clothOperation.ClothOperation?.Operation?.Name ?? "",
                    Cost = clothOperation.ClothOperation?.Price?.Number ?? 0
                }
            );
            fullSum += clothOperation.ClothOperation?.Price?.Number ?? 0;
        }

        if (PartyOnly.IsChecked == true)
        {
            foreach (var item in _packages)
            {
                walletOperations.Add(new WalletOperation()
                    {
                        Name = item.Party?.CutNumber + " " + item.Size?.Number,
                        Cost = item.Party?.Price?.Number ?? 0
                    }
                );
                fullSum += item.Party?.Price?.Number ?? 0;
            }
        }
        else
        {
            foreach (var item in partyList)
            {
                walletOperations.Add(new WalletOperation()
                    {
                        Name = item.CutNumber,
                        Cost = item.Price?.Number ?? 0
                    }
                );
                fullSum += item.Price?.Number ?? 0;
            }
        }
        

        DataGrid.ItemsSource = walletOperations;
        FullSum.Text = fullSum.ToString("F2");
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