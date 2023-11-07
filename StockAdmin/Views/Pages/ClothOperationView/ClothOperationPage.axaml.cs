using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Views.Pages.PackageView;
using StockAdmin.Views.Pages.PartyView;

namespace StockAdmin.Views.Pages.ClothOperationView;

public partial class ClothOperationPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly Package _package;
    public ClothOperationPage(Package package, ContentControl frame)
    {
        InitializeComponent();
        _package = package;
        InitData(package);
        _frame = frame;
    }

    private async void InitData(Package party)
    {
        var repository = new ClothOperationRepository();
        var clothOperations = await repository.GetAllAsync(party.id);
        List.ItemsSource = clothOperations;
        double sum = 0;
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
}