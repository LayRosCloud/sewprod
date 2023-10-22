using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages;

public partial class AddedClothOperationPage : UserControl
{
    private readonly ClothOperation _clothOperation;
    private readonly ContentControl _frame;
    private readonly Party _party;
    public AddedClothOperationPage(Party party, ContentControl frame) : this(party, new ClothOperation(), frame)
    {
    }
    
    public AddedClothOperationPage(Party party, ClothOperation clothOperation, ContentControl frame)
    {
        InitializeComponent();
        _clothOperation = clothOperation;
        _clothOperation.partyId = party.id;
        _frame = frame;
        _party = party;
        InitData();
    }

    private async void InitData()
    {
        var personRepository = new PersonRepository();
        var operationRepository = new OperationRepository();

        CmbPerson.ItemsSource = await personRepository.GetAllAsync();
        CmbOperation.ItemsSource = await operationRepository.GetAllAsync();
        DataContext = _clothOperation;
    }

    private async void SaveChanges(object? sender, RoutedEventArgs e)
    {
        Operation operation = CmbOperation.SelectedItem as Operation;
        _clothOperation.priceId = operation.priceId;
        var clothOperationRepository = new ClothOperationRepository();
        
        if (_clothOperation.id == 0)
        {
            await clothOperationRepository.CreateAsync(_clothOperation);
        }
        else
        {
            await clothOperationRepository.UpdateAsync(_clothOperation);
        }

        _frame.Content = new ClothOperationPage(_party, _frame);
    }
}