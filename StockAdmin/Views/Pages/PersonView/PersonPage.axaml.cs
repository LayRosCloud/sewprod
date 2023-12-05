using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using StockAdmin.Models;
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
        _finderController = new FinderController(500, FilteringArrayOnText);
        
        _frame = frame;
        Init();
    }
    
    private async void Init()
    {
        var dataController = new DataController<PersonEntity>(new PersonRepository(), _persons, List);
        var loadingController = new LoadingController<PersonEntity>(LoadingBorder, dataController);
        await loadingController.FetchDataAsync();
    }

    private void FilteringArrayOnText()
    {
        List.ItemsSource = 
            _persons.Where(x => x.LastName.ToLower().Contains(Finder.Text.ToLower())).ToList();
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
    }

    private void ShowDeleteWindow(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = true;
    }

    private void NavigateToMoreInformation(object? sender, TappedEventArgs e)
    {
        if (List.SelectedItem is PersonEntity person)
        {
            _frame.Content = new StatisticPage(person);
        }
    }

    private void TextChanged(object? sender, TextChangedEventArgs e)
    {
        _finderController.ChangeText();
    }
}