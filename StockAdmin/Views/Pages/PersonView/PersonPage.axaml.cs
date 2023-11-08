using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.PersonView;

public partial class PersonPage : UserControl
{
    private readonly ContentControl _frame;
    
    public PersonPage(ContentControl frame)
    {
        InitializeComponent();
        _frame = frame;
        Init();
    }
    
    private async void Init()
    {
        var personRepository = new PersonRepository();
        List.ItemsSource = await personRepository.GetAllAsync();
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
        Person person = (sender as Button).DataContext as Person;
        _frame.Content = new AddedPersonPage(_frame, person);
    }

    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repository = new PersonRepository();
        
        if (List.SelectedItem is Person person)
        {
            await repository.DeleteAsync(person.id);
        }

        SendNoAnswerOnDeleteItem(sender, e);
        Init();
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
}