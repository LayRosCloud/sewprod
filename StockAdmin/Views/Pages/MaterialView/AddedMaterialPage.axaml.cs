using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using StockAdmin.Models;
using StockAdmin.Scripts;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Exceptions;
using StockAdmin.Scripts.Extensions;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Repositories.Server;
using StockAdmin.Scripts.Server;
using StockAdmin.Scripts.Vectors;

namespace StockAdmin.Views.Pages.MaterialView;

public partial class AddedMaterialPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly MaterialEntity _materialEntity;
    private readonly IRepositoryFactory _factory;
    
    public AddedMaterialPage(ContentControl frame) : this(frame, new MaterialEntity())
    { }
    
    public AddedMaterialPage(ContentControl frame, MaterialEntity materialEntity)
    {
        InitializeComponent();
        
        _factory = ServerConstants.GetRepository();
        _frame = frame;
        _materialEntity = materialEntity;
        
        DataContext = _materialEntity;
    }

    private async void TrySaveChanges(object? sender, RoutedEventArgs e)
    {
        try
        {
            CheckFields();
            await SaveChanges();
            _frame.Content = new MaterialsPage(_frame);
        }
        catch (ValidationException ex)
        {
            ElementConstants.ErrorController.AddErrorMessage(ex.Message);
        }
        catch (Exception)
        {
            ElementConstants.ErrorController.AddErrorMessage(Constants.UnexpectedAdminExceptionMessage);
        }
    }

    private async Task SaveChanges()
    {
        var repository = _factory.CreateMaterialRepository();
        
        if (_materialEntity.Id == 0)
        {
            await repository.CreateAsync(_materialEntity);
        }
        else
        {
            await repository.UpdateAsync(_materialEntity);
        }
    }

    private void CheckFields()
    {
        string name = TbName.Text!;
        string description = TbDesc.Text!;
        string codeVendor = TbCodeVendor.Text!;

        name.ContainLengthBetweenValues(new LengthVector(1, 30), "Наименование должно быть от 1 до 30 символов");
        description.ContainLengthBetweenValues(new LengthVector(1, 255), "Описание должно быть от 1 до 255 символов");
        codeVendor.ContainLengthBetweenValues(new LengthVector(1, 10), "Артикул должен быть от 1 до 10 символов");
    }

    public override string ToString()
    {
        return PageTitles.AddMaterial;
    }

    private void CloseCurrentPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new MaterialsPage(_frame);
    }
}