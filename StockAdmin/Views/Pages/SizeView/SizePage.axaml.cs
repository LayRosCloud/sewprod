
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Views.Pages.SizeView;

namespace StockAdmin.Views.Pages;

public partial class SizePage : UserControl
{
    private readonly ContentControl _frame;
    public SizePage(ContentControl frame)
    {
        InitializeComponent();
        _frame = frame;
        Init();
    }

    private async void Init()
    {
        var sizeRepository = new SizeRepository();
        var ageRepository = new AgeRepository();

        ListSizes.ItemsSource = await sizeRepository.GetAllAsync();
        ListAges.ItemsSource = await ageRepository.GetAllAsync();
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
}