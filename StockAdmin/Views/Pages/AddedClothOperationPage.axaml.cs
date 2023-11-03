using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages;

public partial class AddedClothOperationPage : UserControl
{
    private readonly ClothOperation _clothOperation;
    private readonly ContentControl _frame;
    private readonly Package _package;
    public AddedClothOperationPage(Package package, ContentControl frame) : this(package, new ClothOperation(), frame)
    {
        
    }
    
    public AddedClothOperationPage(Package package, ClothOperation clothOperation, ContentControl frame)
    {
        InitializeComponent();
        
        _clothOperation = clothOperation;
        _clothOperation.packageId = package.id;
        _frame = frame;
        _package = package;
        
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

        _frame.Content = new ClothOperationPage(_package, _frame);
    }
}