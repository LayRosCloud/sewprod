using Avalonia.Controls;
using Avalonia.Interactivity;
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

    
}