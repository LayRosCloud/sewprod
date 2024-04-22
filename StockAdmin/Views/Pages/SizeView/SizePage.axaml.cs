using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Repositories.Server;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Views.Pages.SizeView;

public partial class SizePage : UserControl
{
    private readonly ContentControl _frame;
    private readonly DelayFinder _delayFinderSize;
    private readonly DelayFinder _delayFinderTypeOfSize;

    private readonly List<AgeEntity> _ages;
    private readonly List<SizeEntity> _sizes;
    private readonly IRepositoryFactory _factory;
    public SizePage(ContentControl frame)
    {
        InitializeComponent();
        _factory = ServerConstants.GetRepository();
        
        _ages = new List<AgeEntity>();
        _sizes = new List<SizeEntity>();
        
        _delayFinderSize = new DelayFinder(TimeConstants.Ticks, SortingArraySizes);
        _delayFinderTypeOfSize = new DelayFinder(TimeConstants.Ticks, SortingArrayTypes);
        
        _frame = frame;
        
        Init();
    }

    private async void Init()
    {
        var loadingSizes = new LoadingController<SizeEntity>(LoadingBorder,
            new DataController<SizeEntity>(_factory.CreateSizeRepository(), _sizes, ListSizes));
        var loadingTypes = new LoadingController<AgeEntity>(LoadingBorder,
            new DataController<AgeEntity>(_factory.CreateAgeRepository(), _ages, ListAges));
        
        await loadingSizes.FetchDataAsync();
        await loadingTypes.FetchDataAsync();
    }

    private void SortingArraySizes()
    {
        ListSizes.ItemsSource = _sizes
            .Where(x => x.Number.ToLower().Contains(FinderSize.Text!.ToLower()))
            .ToList();
    }
    
    private void SortingArrayTypes()
    {
        ListAges.ItemsSource = _ages
            .Where(x => x.Name.ToLower().Contains(FinderTypeOfSizes.Text!.ToLower()))
            .ToList();
    }
    
    private void NavigateToAddedSizePage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedSizePage(_frame);
    }

    private void NavigateToTypeOfSizePage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedTypeOfSizePage(_frame);
    }
    
    public override string ToString()
    {
        return PageTitles.Size;
    }

    private void NavigateToEditSizePage(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        if (button.DataContext is not SizeEntity size)
        {
            return;
        }
        
        _frame.Content = new AddedSizePage(_frame, size);
    }

    private void NavigateToEditTypeOfSizePage(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        if (button.DataContext is not AgeEntity age)
        {
            return;
        }
        
        _frame.Content = new AddedTypeOfSizePage(_frame, age);
    }
    
    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        try
        {
            var repository = _factory.CreateSizeRepository();
        
            if (ListSizes.SelectedItem is SizeEntity size)
            {
                await repository.DeleteAsync(size.Id);
            }
            Init();
        }
        catch (Exception)
        {
            ElementConstants.ErrorController.AddErrorMessage(Constants.DeletedExceptionMessage);
        }
    }
    
    private async void SendYesTypeSizeAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        try
        {
            var repository = _factory.CreateAgeRepository();
        
            if (ListAges.SelectedItem is AgeEntity age)
            {
                await repository.DeleteAsync(age.Id);
            }
            Init();
        }
        catch (Exception)
        {
            ElementConstants.ErrorController.AddErrorMessage(Constants.UnexpectedAdminExceptionMessage);
        }
    }
    
    private void ShowDeleteWindowSize(object? sender, RoutedEventArgs e)
    {
        DeletedBorderSize.IsVisible = true;
    }
    
    private void ShowDeleteWindowTypeOfSize(object? sender, RoutedEventArgs e)
    {
        DeletedBorder.IsVisible = true;
    }

    private void TextChangedSize(object? sender, TextChangedEventArgs e)
    {
        _delayFinderSize.ChangeText();
    }

    private void TextChangedTypeOfSize(object? sender, TextChangedEventArgs e)
    {
        _delayFinderTypeOfSize.ChangeText();
    }
}