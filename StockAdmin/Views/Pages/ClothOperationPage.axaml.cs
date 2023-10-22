using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages;

public partial class ClothOperationPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly Party _party;
    public ClothOperationPage(Party party, ContentControl frame)
    {
        InitializeComponent();
        _party = party;
        InitData(party);
        _frame = frame;
    }

    private async void InitData(Party party)
    {
        var repository = new ClothOperationRepository();
        var clothOperations = await repository.GetAllAsync(party.id);
        List.ItemsSource = clothOperations;
        double sum = 0;
        foreach (var clothOperation in clothOperations)
        {
            sum += clothOperation.price.number;
        }

        SumText.Text = $"Сумма: {sum} с наценкой {sum * (party.model.percent / 100.0) + sum}";
    }

    private void BackToPartyPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new PartyPage(_frame);
    }

    private void NavigateToAddedPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedClothOperationPage(_party, _frame);
    }
}