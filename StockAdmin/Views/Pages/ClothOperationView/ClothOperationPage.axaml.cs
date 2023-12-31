﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Records;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Views.Pages.PackageView;

namespace StockAdmin.Views.Pages.ClothOperationView;

public partial class ClothOperationPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly PackageEntity _packageEntity;
    
    private ListSelected _currentIndex;
    private ClothOperationPersonEntity? _clothOperationPerson;
    
    private readonly List<ClothOperationEntity> _clothOperations;
    private readonly DelayFinder _delayFinder;
    
    public ClothOperationPage(PackageEntity packageEntity, ContentControl frame)
    {
        InitializeComponent();
        
        _packageEntity = packageEntity;
        _clothOperations = new List<ClothOperationEntity>();
        
        TitleText.Text 
            = $"{packageEntity.Party?.CutNumber}/{packageEntity.Party?.Person?.Uid} {packageEntity.Size?.Number} {packageEntity.Count}";
        _delayFinder = new DelayFinder(TimeConstants.Ticks, FilteringArrayOnText);
        
        InitData(packageEntity);
        
        _frame = frame;
    }
    
    private async void InitData(PackageEntity packageEntity)
    {
        await InitAsync(packageEntity);
    }

    private async Task InitAsync(PackageEntity packageEntity)
    {
        var loadingController = new LoadingController<ClothOperationEntity>(LoadingBorder);
        await loadingController.FetchDataAsync(async () =>
        {
            var repository = new ClothOperationRepository();
            _clothOperations.Clear();
            _clothOperations.AddRange(await repository.GetAllAsync(packageEntity.Id));
            List.ItemsSource = _clothOperations;
        });
    }

    private void FilteringArrayOnText()
    {
        var text = Finded.Text.ToLower().Trim();
        List.ItemsSource = _clothOperations.Where(x=> x.Operation.Name
                .ToLower()
                .Contains(text))
            .ToList();
    }
    private void BackToPackagePage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new PackagePage(_frame);
    }

    private void NavigateToAddedPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedClothOperationPage(_packageEntity, _frame);
    }

    private void NavigateToAddedClothOperationPersonPage(object? sender, RoutedEventArgs e)
    {
        ClothOperationEntity clothOperationEntity = (List.SelectedItem as ClothOperationEntity)!;
        _frame.Content = new AddedClothOperationPersonPage(_frame, clothOperationEntity, _packageEntity);
    }

    private void NavigateToEditClothOperationPage(object? sender, RoutedEventArgs e)
    {
        ClothOperationEntity clothOperationEntity = (sender as Button).DataContext as ClothOperationEntity;
        _frame.Content = new AddedClothOperationPage(_packageEntity, clothOperationEntity!, _frame);
    }

    private void NavigateToEditClothOperationPersonPage(object? sender, RoutedEventArgs e)
    {
        var clothOperationEntity = (List.SelectedItem as ClothOperationEntity)!;
        var clothOperationPersonEntity  = (sender as Button).DataContext as ClothOperationPersonEntity;
        _frame.Content = new AddedClothOperationPersonPage(_frame, clothOperationEntity, clothOperationPersonEntity!, _packageEntity);
    }
    
    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repositoryPerson = new ClothOperationPersonRepository();
        var repositoryOperation = new ClothOperationRepository();
        switch (_currentIndex)
        {
            case ListSelected.First:
            {
                if (_clothOperationPerson != null)
                {
                    await repositoryPerson.DeleteAsync(_clothOperationPerson.Id);
                    await InitAsync(_packageEntity);
                    List.SelectAll();
                }

                break;
            }
            case ListSelected.Second:
            {
                if (List.SelectedItem is ClothOperationEntity operation)
                {
                    await repositoryOperation.DeleteAsync(operation.Id);
                
                    await InitAsync(_packageEntity);
                    List.SelectAll();

                }

                break;
            }
        }
        
        SendNoAnswerOnDeleteItem(sender, e);
    }

    private void SendNoAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = false;
    }

    private void ShowDeleteWindow(object? sender, RoutedEventArgs e)
    {
        _clothOperationPerson = (sender as Button).DataContext as ClothOperationPersonEntity;
        DeletedContainer.IsVisible = true;
        DeletedContainer.Text =
            "вы действительно уверены, что хотите удалить этого участника операции?" +
            " Восстановить участника операции будет нельзя!";
        
        _currentIndex = ListSelected.First;
    }

    private void ShowDeleteWindowClothOperation(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = true;
        DeletedContainer.Text =
            "вы действительно уверены, что хотите удалить операцию над одеждой?" +
            " Восстановить операцию над одеждой будет нельзя!";
        
        _currentIndex = ListSelected.Second;
    }
    
    public override string ToString()
    {
        return PageTitles.ClothOperation;
    }

    private void TextChanged(object? sender, TextChangedEventArgs e)
    {
        _delayFinder.ChangeText();
    }

    private void SelectedClothOperation(object? sender, SelectionChangedEventArgs e)
    {
        ListPersons.ItemsSource = (List.SelectedItem as ClothOperationEntity).ClothOperationPersons;
    }
}