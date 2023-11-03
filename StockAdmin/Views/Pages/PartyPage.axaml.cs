using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages;

public partial class PartyPage : UserControl
{
    private readonly ContentControl _frame;
    public PartyPage(ContentControl frame)
    {
        InitializeComponent();
        InitData();
        _frame = frame;
    }
    
    private async void InitData()
    {
        var partyRepository = new PartyRepository();
        List.ItemsSource = await partyRepository.GetAllAsync();
    }
    
    private void NavigateToMoreInformation(object? sender, TappedEventArgs e)
    {
        DataGrid dataGrid = sender as DataGrid;
        if (dataGrid.SelectedItem is Package package)
        {
            var page = new ClothOperationPage(package, _frame);
            _frame.Content = page;
        }
    }
    
    private void NavigateToEditPage(object? sender, RoutedEventArgs e)
    {
        Party party = ((sender as Button)!.DataContext as Party)!;
        _frame.Content = new AddedPartyPage(party, _frame);
    }

    private void DeleteSelectedItem(object? sender, RoutedEventArgs e)
    {
        Party party = ((sender as Button)!.DataContext as Party)!;
        DeletedContainer.IsVisible = true;
        Person person = party.person;
        DeletedMessage.Text =
            $"вы действительно уверены, что хотите удалить крою " +
            $"{person.lastName} {person.firstName[0]}. {person.patronymic?[0]} с моделью \"{party.model.title}\"?" +
            " Восстановить крою будет нельзя!";
    }

    private void NavigateToAddedPartyPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedPartyPage(_frame);
    }

    private async void SendYesAnswerToDeleteRow(object? sender, RoutedEventArgs e)
    {
        Party party = (List.SelectedItem as Party)!;

        PartyRepository repository = new PartyRepository();
        await repository.DeleteAsync(party.id);
        
        DeletedContainer.IsVisible = false;
        InitData();
    }

    private void SendNoAnswerToDeleteRow(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = false;
    }

    private void NavigateToAddedPackagesPage(object? sender, RoutedEventArgs e)
    {
        var party = (List.SelectedItem as Party)!;

        _frame.Content = new AddedPackagesPage(party, _frame);
    }
}