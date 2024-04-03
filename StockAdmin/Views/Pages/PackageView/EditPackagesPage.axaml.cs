using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Views.Pages.PackageView;

public partial class EditPackagesPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly PackageEntity _packageEntity;
    private readonly IRepositoryFactory _factory;
    
    public EditPackagesPage(ContentControl frame, PackageEntity packageEntity)
    {
        InitializeComponent();
        _factory = ServerConstants.GetRepository();
        _frame = frame;
        _packageEntity = packageEntity;
        
        Init();
    }
    
    private async void Init()
    {
        var repository = _factory.CreateSizeRepository();
        var repositoryMaterials = _factory.CreateMaterialRepository();
        var repositoryPersons = _factory.CreatePersonRepository();
        
        CbSizes.ItemsSource = await repository.GetAllAsync();
        CbMaterials.ItemsSource = await repositoryMaterials.GetAllAsync();
        CbPersons.ItemsSource = await repositoryPersons.GetAllAsync();
        
        DataContext = _packageEntity;
    }


    private async void SaveChanges(object? sender, RoutedEventArgs e)
    {
        try
        {
            var repository = _factory.CreatePackagesRepository();
            _packageEntity.IsUpdated = true;
        
            await repository.UpdateAsync(_packageEntity);
        
            _frame.Content = new PackagePage(_frame);
        }
        catch (Exception)
        {
            ElementConstants.ErrorController.AddErrorMessage(Constants.UnexpectedAdminExceptionMessage);
        }
    }

    public override string ToString()
    {
        return PageTitles.EditPackage;
    }

    private void CloseCurrentPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new PackagePage(_frame);
    }
}