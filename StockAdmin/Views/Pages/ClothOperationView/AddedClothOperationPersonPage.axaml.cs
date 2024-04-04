using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Exceptions;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Repositories.Server;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Views.Pages.ClothOperationView;

public partial class AddedClothOperationPersonPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly ClothOperationPersonEntity _clothOperationPersonEntity;
    private readonly PackageEntity _packageEntity;
    private readonly IRepositoryFactory _factory;

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
        _factory = ServerConstants.GetRepository();
        _frame = frame;
        _clothOperationPersonEntity = clothOperationPersonEntity;
        _packageEntity = packageEntity;
        _clothOperationPersonEntity.ClothOperationId = clothOperationEntity.Id;
        Init();
    }

    private async void Init()
    {
        var repository = _factory.CreatePersonRepository();
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
        catch (MyValidationException ex)
        {
            ElementConstants.ErrorController.AddErrorMessage(ex.Message);
        }
        catch (Exception)
        {
            ElementConstants.ErrorController.AddErrorMessage(Constants.UnexpectedAdminExceptionMessage);
        }
    }

    private void CheckFields()
    {
        if (CbClothOperationsPersons.SelectedItem == null)
        {
            throw new MyValidationException("Выберите человека!");
        }

        if (CbDateStart.SelectedDate == null)
        {
            throw new MyValidationException("Выберите дату начала!");
        }
    }
    
    private async Task SaveChanges()
    {
        var repository = _factory.CreateClothOperationPersonRepository();
        var clothOperationRepo = _factory.CreateClothOperationRepository();
        if (_clothOperationPersonEntity.Id == 0)
        {
            await repository.CreateAsync(_clothOperationPersonEntity);
        }
        else
        {
            await repository.UpdateAsync(_clothOperationPersonEntity);
            var clothOperation = await clothOperationRepo.GetAsync(_clothOperationPersonEntity.ClothOperationId);
            bool allIsEnded = true;
            clothOperation.ClothOperationPersons.ForEach(item =>
            {
                if (!item.IsEnded)
                {
                    allIsEnded = false;
                }
            });

            if (allIsEnded)
            {
                clothOperation.IsEnded = true;
                await clothOperationRepo.UpdateAsync(clothOperation);
            }
        }

    }
    
    public override string ToString()
    {
        return PageTitles.AddClothOperationPerson;
    }

    private void CloseCurrentPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new ClothOperationPage(_packageEntity, _frame);
    }
}