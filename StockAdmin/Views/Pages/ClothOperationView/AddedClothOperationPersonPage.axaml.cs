using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.ClothOperationView;

public partial class AddedClothOperationPersonPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly ClothOperationPersonEntity _clothOperationPersonEntity;
    private readonly PackageEntity _packageEntity;

    public AddedClothOperationPersonPage(ContentControl frame, ClothOperationEntity clothOperationEntity, PackageEntity packageEntity)
    :this(frame, clothOperationEntity, new ClothOperationPersonEntity(), packageEntity)
    {
        CbEnded.IsVisible = false;
    }

    public AddedClothOperationPersonPage(ContentControl frame, 
        ClothOperationEntity clothOperationEntity, 
        ClothOperationPersonEntity clothOperationPersonEntity, 
        PackageEntity packageEntity)
    {
        InitializeComponent();
        _frame = frame;
        _clothOperationPersonEntity = clothOperationPersonEntity;
        _packageEntity = packageEntity;
        _clothOperationPersonEntity.ClothOperationId = clothOperationEntity.Id;
        Init();
    }

    private async void Init()
    {
        var repository = new PersonRepository();
        CbClothOperationsPersons.ItemsSource = await repository.GetAllAsync();
        DataContext = _clothOperationPersonEntity;
    }

    private async void TrySaveChanges(object? sender, RoutedEventArgs e)
    {
        try
        {
            CheckFields();
            await SaveChanges();
            _frame.Content = new ClothOperationPage(_packageEntity, _frame);
        }
        catch (ValidationException ex)
        {
            ElementConstants.ErrorController.AddErrorMessage(ex.Message);
        }
    }

    private void CheckFields()
    {
        if (CbClothOperationsPersons.SelectedItem == null)
        {
            throw new ValidationException("Выберите человека!");
        }

        if (CbDateStart.SelectedDate == null)
        {
            throw new ValidationException("Выберите дату начала!");
        }
    }
    
    private async Task SaveChanges()
    {
        var repository = new ClothOperationPersonRepository();

        if (_clothOperationPersonEntity.Id == 0)
        {
            await repository.CreateAsync(_clothOperationPersonEntity);
        }
        else
        {
            await repository.UpdateAsync(_clothOperationPersonEntity);
        }

    }
    
    public override string ToString()
    {
        return "Добавление / Обновление участника операции";
    }
}