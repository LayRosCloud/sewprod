using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.ClothOperationView;

public partial class AddedClothOperationPage : UserControl
{
    private readonly ClothOperationEntity _clothOperationEntity;
    private readonly ContentControl _frame;
    private readonly PackageEntity _packageEntity;

    public AddedClothOperationPage(PackageEntity packageEntity, ContentControl frame)
        : this(packageEntity, new ClothOperationEntity(), frame)
    {
        CbEnded.IsVisible = false;
    }
    
    public AddedClothOperationPage(PackageEntity packageEntity, ClothOperationEntity clothOperationEntity, ContentControl frame)
    {
        InitializeComponent();
        
        _clothOperationEntity = clothOperationEntity;
        _clothOperationEntity.PackageId = packageEntity.Id;
        _frame = frame;
        _packageEntity = packageEntity;
        
        InitData();
    }

    private async void InitData()
    {
        var operationRepository = new OperationRepository();

        CmbOperation.ItemsSource = await operationRepository.GetAllAsync();
        DataContext = _clothOperationEntity;
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
        if (CmbOperation.SelectedItem == null)
        {
            throw new ValidationException("Выберите операцию");
        }
    }
    
    private async Task SaveChanges()
    {
        var clothOperationRepository = new ClothOperationRepository();
        if (CmbOperation.SelectedItem is not OperationEntity operationEntity)
        {
            return;
        }
        double numberOnPrice = _packageEntity.Party.Price.Number * operationEntity.Percent / 100.0;
        var price = await new PriceRepository().CreateAsync(new PriceEntity { Number = numberOnPrice });
        _clothOperationEntity.PriceId = price.Id;
        if (_clothOperationEntity.Id == 0)
        {
            await clothOperationRepository.CreateAsync(_clothOperationEntity);
        }
        else
        {
            await clothOperationRepository.UpdateAsync(_clothOperationEntity);
        }
    }
    
    public override string ToString()
    {
        return "Добавление / Обновление операций над одеждой";
    }
}