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
    private readonly ClothOperationPersonEntity _clothOperationPersonEntity;
    private readonly PackageEntity _packageEntity;

    public AddedClothOperationPersonPage(ContentControl frame, ClothOperationEntity clothOperationEntity, PackageEntity packageEntity)
    :this(frame, clothOperationEntity, new ClothOperationPersonEntity(), packageEntity)
    {
        CbEnded.IsVisible = false;
    }

    public AddedClothOperationPersonPage(ContentControl frame, ClothOperationEntity clothOperationEntity, ClothOperationPersonEntity clothOperationPersonEntity, PackageEntity packageEntity)
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
        PersonRepository repository = new PersonRepository();
        CbClothOperationsPersons.ItemsSource = await repository.GetAllAsync();
        DataContext = _clothOperationPersonEntity;
    }

    private async void SaveChanges(object? sender, RoutedEventArgs e)
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

        _frame.Content = new ClothOperationPage(_packageEntity, _frame);
    }
    
    public override string ToString()
    {
        return "Добавление / Обновление участника операции";
    }
}