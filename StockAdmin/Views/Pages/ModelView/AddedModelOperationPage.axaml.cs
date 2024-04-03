using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Exceptions;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Repositories.Server;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Views.Pages.ModelView;

public partial class AddedModelOperationPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly ModelEntity _model;
    private readonly IRepositoryFactory _factory;
    public AddedModelOperationPage(ContentControl frame, ModelEntity model)
    {
        InitializeComponent();
        _factory = ServerConstants.GetRepository();
        _frame = frame;
        _model = model;
        Init();
    }

    private async void Init()
    {
        var repository = _factory.CreateOperationRepository();
        CbOperations.ItemsSource = await repository.GetAllAsync();
    }
    
    private async void TrySaveChanges(object? sender, RoutedEventArgs e)
    {
        try
        {
            OperationEntity operation = CheckFields();
            await SaveChanges(operation);
            _frame.Content = new ModelPage(_frame);
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

    private async Task SaveChanges(OperationEntity operation)
    {
        var modelOperationRepository = _factory.CreateModelOperationRepository();
        
        await modelOperationRepository.CreateAsync(new ModelOperationEntity{ModelId = _model.Id, OperationId = operation.Id});
    }
    
    private OperationEntity CheckFields()
    {
        if (CbOperations.SelectedItem is not OperationEntity operationEntity)
        {
            throw new ValidationException("Выберите операцию!");
        }

        return operationEntity;
    }
    
    public override string ToString()
    {
        return PageTitles.AddModelOperation;
    }

    private void CloseCurrentPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new ModelPage(_frame);
    }
}