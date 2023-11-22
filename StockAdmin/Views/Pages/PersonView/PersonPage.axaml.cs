using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Views.Pages.StatisticPeople;

namespace StockAdmin.Views.Pages.PersonView;

public partial class PersonPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly FinderController _finderController;
    private readonly List<PersonEntity> _persons;
    
    public PersonPage(ContentControl frame)
    {
        InitializeComponent();
        _persons = new List<PersonEntity>();
        _finderController = new FinderController(500, () =>
        {
            List.ItemsSource = _persons.Where(x => x.LastName.ToLower().Contains(Finder.Text.ToLower())).ToList();
        });
        
        _frame = frame;
        Init();
    }
    
    private async void Init()
    {
        var personRepository = new PersonRepository();
        _persons.Clear();
        _persons.AddRange(await personRepository.GetAllAsync());
        List.SelectedItem = null;
        List.ItemsSource = _persons;
        LoadingBorder.IsVisible = false;
    }
    
    private void NavigateToAddedPersonPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedPersonPage(_frame);
    }
    
    public override string ToString()
    {
        return "Персонал";
    }

    private void NavigateToEditPersonPage(object? sender, RoutedEventArgs e)
    {
        PersonEntity personEntity = (sender as Button).DataContext as PersonEntity;
        _frame.Content = new AddedPersonPage(_frame, personEntity);
    }

    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repository = new PersonRepository();
        
        if (List.SelectedItem is PersonEntity person)
        {
            await repository.DeleteAsync(person.Id);
            Init();
        }

        SendNoAnswerOnDeleteItem(sender, e);
    }

    private void SendNoAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = false;
    }

    private void ShowDeleteWindow(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = true;
        DeletedMessage.Text =
            "вы действительно уверены, что хотите удалить пользователя?" +
            " Восстановить пользователя будет нельзя!";
    }

    private void NavigateToMoreInformation(object? sender, TappedEventArgs e)
    {
        _frame.Content = new StatisticPage();
    }

    private void TextChanged(object? sender, TextChangedEventArgs e)
    {
        _finderController.ChangeText();
    }
}