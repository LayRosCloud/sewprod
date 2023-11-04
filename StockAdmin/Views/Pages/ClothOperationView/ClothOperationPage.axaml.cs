using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

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
        _frame.Content = new PartyView.PartyPage(_frame);
    }

    private void NavigateToAddedPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedClothOperationPage(_package, _frame);
    }
    
    public override string ToString()
    {
        return "Операции над одеждой";
    }
}