using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.PersonView;

public partial class AddedPersonPage : UserControl
{
    private readonly Person _person;
    private readonly ContentControl _frame;

    public AddedPersonPage(ContentControl frame)
        : this(frame, new Person()) { }
    
    public AddedPersonPage(ContentControl frame, Person person)
    {
        InitializeComponent();
        _person = person;
        _frame = frame;
        DataContext = _person;
    }

    private async void SaveChanges(object? sender, RoutedEventArgs e)
    {
        PersonRepository personRepository = new PersonRepository();
        if (_person.id == 0)
        {
            await personRepository.CreateAsync(_person);
        }
        else
        {
            await personRepository.UpdateAsync(_person);
        }

        _frame.Content = new PersonPage(_frame);
    }
    
    public override string ToString()
    {
        return "Добавление / Обновление персонала";
    }
}