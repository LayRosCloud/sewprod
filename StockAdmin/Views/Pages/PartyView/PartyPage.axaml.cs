using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Views.Pages.PackageView;

namespace StockAdmin.Views.Pages.PartyView;

public partial class PartyPage : UserControl
{
    private readonly ContentControl _frame;
    public PartyPage(ContentControl frame)
    {
        InitializeComponent();
        _frame = frame;
    }
    
    public async Task InitData()
    {
        var partyRepository = new PartyRepository();
        List.ItemsSource = await partyRepository.GetAllAsync();
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
        await InitData();
    }

    private void SendNoAnswerToDeleteRow(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = false;
    }

    private void NavigateToPackagePage(object? sender, TappedEventArgs e)
    {
        if (List.SelectedItem is Party party)
        {
            _frame.Content = new PackagePage(_frame, party);
        }
    }
    
    public override string ToString()
    {
        return "Крои";
    }
}