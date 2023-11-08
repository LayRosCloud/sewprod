using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.Templates;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Views.Pages.PartyView;

namespace StockAdmin.Views.Pages.ClothOperationView;

public partial class ClothOperationPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly Package _package;
    private int _currentIndex = 0;
    private ClothOperationPerson? _clothOperationPerson;
    public ClothOperationPage(Package package, ContentControl frame)
    {
        InitializeComponent();
        _package = package;
        InitData(package);
        _frame = frame;
    }
    
    private async void InitData(Package package)
    {
        var repository = new ClothOperationRepository();
        var clothOperations = await repository.GetAllAsync(package.id);
        List.ItemsSource = clothOperations;
    }

    private void BackToPartyPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new PartyPage(_frame);
    }

    private void NavigateToAddedPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedClothOperationPage(_package, _frame);
    }
    
    public override string ToString()
    {
        return "Операции над одеждой";
    }

    private void NavigateToAddedClothOperationPersonPage(object? sender, RoutedEventArgs e)
    {
        ClothOperation clothOperation = (List.SelectedItem as ClothOperation)!;
        _frame.Content = new AddedClothOperationPersonPage(_frame, clothOperation, _package);
    }

    private void NavigateToEditClothOperationPage(object? sender, RoutedEventArgs e)
    {
        ClothOperation clothOperation = (sender as Button).DataContext as ClothOperation;
        _frame.Content = new AddedClothOperationPage(_package, clothOperation, _frame);
    }

    private void NavigateToEditClothOperationPersonPage(object? sender, RoutedEventArgs e)
    {
        ClothOperation clothOperation = (List.SelectedItem as ClothOperation)!;
        ClothOperationPerson clothOperationPerson  = (sender as Button).DataContext as ClothOperationPerson;
        _frame.Content = new AddedClothOperationPersonPage(_frame, clothOperation, clothOperationPerson, _package);
    }
    
    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repositoryPerson = new ClothOperationPersonRepository();
        var repositoryOperation = new ClothOperationRepository();
        if (_currentIndex == 1)
        {
            if (_clothOperationPerson != null)
            {
                await repositoryPerson.DeleteAsync(_clothOperationPerson.id);
            }
        }
        else if(_currentIndex == 2)
        {
            if (List.SelectedItem is ClothOperation operation)
            {
                await repositoryOperation.DeleteAsync(operation.id);
            }
        }
        

        SendNoAnswerOnDeleteItem(sender, e);
        InitData(_package);
    }

    private void SendNoAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = false;
    }

    private void ShowDeleteWindow(object? sender, RoutedEventArgs e)
    {
        _clothOperationPerson = (sender as Button).DataContext as ClothOperationPerson;
        DeletedContainer.IsVisible = true;
        DeletedMessage.Text =
            "вы действительно уверены, что хотите удалить этого участника операции?" +
            " Восстановить участника операции будет нельзя!";
        _currentIndex = 1;
    }

    private void ShowDeleteWindowClothOperation(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = true;
        DeletedMessage.Text =
            "вы действительно уверены, что хотите удалить операцию над одеждой?" +
            " Восстановить операцию над одеждой будет нельзя!";
        _currentIndex = 2;
    }
    
}