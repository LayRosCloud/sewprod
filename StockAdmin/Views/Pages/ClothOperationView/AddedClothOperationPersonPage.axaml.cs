using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.ClothOperationView;

public partial class AddedClothOperationPersonPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly ClothOperationPerson _clothOperationPerson;
    private readonly Package _package;

    public AddedClothOperationPersonPage(ContentControl frame, ClothOperation clothOperation, Package package)
    :this(frame, clothOperation, new ClothOperationPerson(), package)
    {
        CbEnded.IsVisible = false;
    }

    public AddedClothOperationPersonPage(ContentControl frame, ClothOperation clothOperation, ClothOperationPerson clothOperationPerson, Package package)
    {
        InitializeComponent();
        _frame = frame;
        _clothOperationPerson = clothOperationPerson;
        _package = package;
        _clothOperationPerson.clothOperationId = clothOperation.id;
        Init();
    }

    private async void Init()
    {
        PersonRepository repository = new PersonRepository();
        CbClothOperationsPersons.ItemsSource = await repository.GetAllAsync();
        DataContext = _clothOperationPerson;
    }

    private async void SaveChanges(object? sender, RoutedEventArgs e)
    {
        var repository = new ClothOperationPersonRepository();

        if (_clothOperationPerson.id == 0)
        {
            await repository.CreateAsync(_clothOperationPerson);
        }
        else
        {
            await repository.UpdateAsync(_clothOperationPerson);
        }

        _frame.Content = new ClothOperationPage(_package, _frame);
    }
    
    public override string ToString()
    {
        return "Добавление / Обновление участника операции";
    }
}