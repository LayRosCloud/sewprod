using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.PackageView;

public partial class EditPackagesPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly PackageEntity _packageEntity;
    private readonly PartyEntity _partyEntity;
    
    public EditPackagesPage(ContentControl frame, PackageEntity packageEntity, PartyEntity partyEntity)
    {
        InitializeComponent();
        _frame = frame;
        _packageEntity = packageEntity;
        _partyEntity = partyEntity;
        Init();
    }
    
    private async void Init()
    {
        var repository = new SizeRepository();
        var repositoryMaterials = new MaterialRepository();
        
        CbSizes.ItemsSource = await repository.GetAllAsync();
        CbMaterials.ItemsSource = await repositoryMaterials.GetAllAsync();
        DataContext = _packageEntity;
    }


    private async void SaveChanges(object? sender, RoutedEventArgs e)
    {
        var repository = new PackageRepository();
        _packageEntity.IsUpdated = true;
        await repository.UpdateAsync(_packageEntity);
        
        //TODO
        
        //_frame.Content = new PackagePage(_frame, _party);
    }

    public override string ToString()
    {
        return "Изменение пачки";
    }
}