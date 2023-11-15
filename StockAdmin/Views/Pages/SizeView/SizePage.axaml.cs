using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.SizeView;

public partial class SizePage : UserControl
{
    private readonly ContentControl _frame;
    private readonly FinderController _finderSizeController;
    private readonly FinderController _finderTypeOfSizeController;

    private readonly List<Age> _ages;
    private readonly List<Size> _sizes;
    public SizePage(ContentControl frame)
    {
        InitializeComponent();
        _ages = new List<Age>();
        _sizes = new List<Size>();
        _finderSizeController = new FinderController(500, () =>
        {
            ListSizes.ItemsSource = _sizes.Where(x => x.number.ToLower().Contains(FinderSize.Text.ToLower())).ToList();
        });
        _finderTypeOfSizeController = new FinderController(500, () =>
        {
            ListAges.ItemsSource = _ages.Where(x => x.name.ToLower().Contains(FinderTypeOfSizes.Text.ToLower())).ToList();
        });
        _frame = frame;
        Init();
    }

    private async void Init()
    {
        var sizeRepository = new SizeRepository();
        var ageRepository = new AgeRepository();
        _sizes.AddRange(await sizeRepository.GetAllAsync());
        _ages.AddRange(await ageRepository.GetAllAsync());
        ListSizes.ItemsSource = _sizes;
        ListAges.ItemsSource = _ages;
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
        Size size = (sender as Button)?.DataContext as Size;
        _frame.Content = new AddedSizePage(_frame, size);
    }

    private void NavigateToEditTypeOfSizePage(object? sender, RoutedEventArgs e)
    {
        Age age = (sender as Button)?.DataContext as Age;
        _frame.Content = new AddedTypeOfSizePage(_frame, age);
    }
    
    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repository = new SizeRepository();
        
        if (ListSizes.SelectedItem is Size size)
        {
            await repository.DeleteAsync(size.id);
        }

        SendNoAnswerOnDeleteItem(sender, e);
        Init();
    }
    
    private async void SendYesTypeSizeAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repository = new AgeRepository();
        
        if (ListAges.SelectedItem is Age age)
        {
            await repository.DeleteAsync(age.id);
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