using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Exceptions;
using StockAdmin.Scripts.Extensions;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;
using StockAdmin.Scripts.Vectors;

namespace StockAdmin.Views.Pages.SizeView;

public partial class AddedTypeOfSizePage : UserControl
{
    private readonly ContentControl _frame;
    private readonly AgeEntity _ageEntity;
    private readonly IRepositoryFactory _factory;
    public AddedTypeOfSizePage(ContentControl frame)
        :this(frame, new AgeEntity()){}
    
    public AddedTypeOfSizePage(ContentControl frame, AgeEntity ageEntity)
    {
        InitializeComponent();
        _factory = ServerConstants.GetRepository();
        _frame = frame;
        _ageEntity = ageEntity;
        DataContext = _ageEntity;
    }

    private async void TrySaveChanges(object? sender, RoutedEventArgs e)
    {
        try
        {
            CheckFields();
            await SaveChanges();
            _frame.Content = new SizePage(_frame);
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
        var ageRepository = _factory.CreateAgeRepository();
        if (_ageEntity.Id == 0)
        {
            await ageRepository.CreateAsync(_ageEntity);
        }
        else
        {
            await ageRepository.UpdateAsync(_ageEntity);
        }
    }
    
    private void CheckFields()
    {
        const string lengthNameMessage = "Название от 1 до 30 символов";
        const string lengthNameDescription = "Описание от 1 до 255 символов";
        string name = TbName.Text!;
        string description = TbDesc.Text!;

        name.ContainLengthBetweenValues(new LengthVector(1, 30), lengthNameMessage);
        description.ContainLengthBetweenValues(new LengthVector(1, 255), lengthNameDescription);
    }
    
    public override string ToString()
    {
        return PageTitles.AddAge;
    }

    private void CloseCurrentPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new SizePage(_frame);
    }
}