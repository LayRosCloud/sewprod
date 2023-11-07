using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.PackageView;

public partial class EditPackagesPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly Package _package;
    private readonly Party _party;
    
    public EditPackagesPage(ContentControl frame, Package package, Party party)
    {
        InitializeComponent();
        _frame = frame;
        _package = package;
        _party = party;
        Init();
    }
    
    private async void Init()
    {
        var repository = new SizeRepository();
        var repositoryMaterials = new MaterialRepository();
        var repositoryColors = new ColorRepository();
        
        CbSizes.ItemsSource = await repository.GetAllAsync();
        CbMaterials.ItemsSource = await repositoryMaterials.GetAllAsync();
        CbColors.ItemsSource = await repositoryColors.GetAllAsync();
        DataContext = _package;
    }


    private async void SaveChanges(object? sender, RoutedEventArgs e)
    {
        var repository = new PackageRepository();

        await repository.UpdateAsync(_package);

        _frame.Content = new PackagePage(_frame, _party);
    }

    public override string ToString()
    {
        return "Изменение пачки";
    }
}