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
        _finderSizeController = new FinderController(500, () =>
        {
            ListSizes.ItemsSource = _sizes.Where(x => x.Number.ToLower().Contains(FinderSize.Text.ToLower())).ToList();
        });
        _finderTypeOfSizeController = new FinderController(500, () =>
        {
            ListAges.ItemsSource = _ages.Where(x => x.Name.ToLower().Contains(FinderTypeOfSizes.Text.ToLower())).ToList();
        });
        _frame = frame;
        Init();
    }

    private async void Init()
    {
        LoadingBorder.IsVisible = true;
        var sizeRepository = new SizeRepository();
        var ageRepository = new AgeRepository();
        _sizes.AddRange(await sizeRepository.GetAllAsync());
        _ages.AddRange(await ageRepository.GetAllAsync());
        ListSizes.ItemsSource = _sizes;
        ListAges.ItemsSource = _ages;
        LoadingBorder.IsVisible = false;
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
        SizeEntity sizeEntity = (sender as Button)?.DataContext as SizeEntity;
        _frame.Content = new AddedSizePage(_frame, sizeEntity);
    }

    private void NavigateToEditTypeOfSizePage(object? sender, RoutedEventArgs e)
    {
        AgeEntity ageEntity = (sender as Button)?.DataContext as AgeEntity;
        _frame.Content = new AddedTypeOfSizePage(_frame, ageEntity);
    }
    
    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repository = new SizeRepository();
        
        if (ListSizes.SelectedItem is SizeEntity size)
        {
            await repository.DeleteAsync(size.Id);
        }

        SendNoAnswerOnDeleteItem(sender, e);
        Init();
    }
    
    private async void SendYesTypeSizeAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repository = new AgeRepository();
        
        if (ListAges.SelectedItem is AgeEntity age)
        {
            await repository.DeleteAsync(age.Id);
        }

        SendNoAnswerOnDeleteItem(sender, e);
        Init();
    }

    private void SendNoAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        DeletedContainerTypeSize.IsVisible = false;
        DeletedContainerSize.IsVisible = false;
    }

    private void ShowDeleteWindowSize(object? sender, RoutedEventArgs e)
    {
        DeletedContainerSize.IsVisible = true;
        DeletedMessageSize.Text =
            "вы действительно уверены, что хотите удалить размер?" +
            " Восстановить размер будет нельзя!";
    }
    
    private void ShowDeleteWindowTypeOfSize(object? sender, RoutedEventArgs e)
    {
        DeletedContainerTypeSize.IsVisible = true;
        DeletedMessageTypeSize.Text =
            "вы действительно уверены, что хотите удалить тип размера?" +
            " Восстановить тип размера будет нельзя!";
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