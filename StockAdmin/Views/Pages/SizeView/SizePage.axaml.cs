using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.SizeView;

public partial class SizePage : UserControl
{
    private readonly ContentControl _frame;
    private readonly FinderController _finderSizeController;
    private readonly FinderController _finderTypeOfSizeController;

    private readonly List<AgeEntity> _ages;
    private readonly List<SizeEntity> _sizes;
    public SizePage(ContentControl frame)
    {
        InitializeComponent();
        
        _ages = new List<AgeEntity>();
        _sizes = new List<SizeEntity>();
        
        _finderSizeController = new FinderController(500, SortingArraySizes);
        _finderTypeOfSizeController = new FinderController(500, SortingArrayTypes);
        
        _frame = frame;
        
        Init();
    }

    private async void Init()
    {
        var loadingSizes = new LoadingController<SizeEntity>(LoadingBorder,
            new DataController<SizeEntity>(new SizeRepository(), _sizes, ListSizes));
        var loadingTypes = new LoadingController<AgeEntity>(LoadingBorder,
            new DataController<AgeEntity>(new AgeRepository(), _ages, ListAges));
        
        await loadingSizes.FetchDataAsync();
        await loadingTypes.FetchDataAsync();
    }

    private void SortingArraySizes()
    {
        ListSizes.ItemsSource = _sizes
            .Where(x => x.Number.ToLower().Contains(FinderSize.Text.ToLower()))
            .ToList();
    }
    
    private void SortingArrayTypes()
    {
        ListAges.ItemsSource = _ages
            .Where(x => x.Name.ToLower().Contains(FinderTypeOfSizes.Text.ToLower()))
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
        return "Размеры";
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
        var repository = new SizeRepository();
        
        if (ListSizes.SelectedItem is SizeEntity size)
        {
            await repository.DeleteAsync(size.Id);
        }
        Init();
    }
    
    private async void SendYesTypeSizeAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repository = new AgeRepository();
        
        if (ListAges.SelectedItem is AgeEntity age)
        {
            await repository.DeleteAsync(age.Id);
        }
        Init();
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
        _finderSizeController.ChangeText();
    }

    private void TextChangedTypeOfSize(object? sender, TextChangedEventArgs e)
    {
        _finderTypeOfSizeController.ChangeText();
    }
}