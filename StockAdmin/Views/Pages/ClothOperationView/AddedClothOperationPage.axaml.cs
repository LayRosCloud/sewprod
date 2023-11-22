using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
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

    private async void SaveChanges(object? sender, RoutedEventArgs e)
    {
        var clothOperationRepository = new ClothOperationRepository();
        
        if (_clothOperationEntity.Id == 0)
        {
            await clothOperationRepository.CreateAsync(_clothOperationEntity);
        }
        else
        {
            await clothOperationRepository.UpdateAsync(_clothOperationEntity);
        }

        _frame.Content = new ClothOperationPage(_packageEntity, _frame);
    }
    
    public override string ToString()
    {
        return "Добавление / Обновление операций над одеждой";
    }
}