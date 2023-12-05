using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Exceptions;
using StockAdmin.Scripts.Extensions;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Validations;
using StockAdmin.Scripts.Vectors;

namespace StockAdmin.Views.Pages.OperationView;

public partial class AddedOperationPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly OperationEntity _operationEntity;

    public AddedOperationPage(ContentControl frame) : this(frame, new OperationEntity())
    {
        
    }
    
    public AddedOperationPage(ContentControl frame, OperationEntity operationEntity)
    {
        InitializeComponent();
        _frame = frame;
        _operationEntity = operationEntity;
        DataContext = _operationEntity;
    }
    
    private async void TrySaveChanges(object? sender, RoutedEventArgs e)
    {
        try
        {
            CheckFields();
            await SaveChanges();
            _frame.Content = new OperationPage(_frame);

        }
        catch (ValidationException ex)
        {
            ElementConstants.ErrorController.AddErrorMessage(ex.Message);
        }

    }

    private void CheckFields()
    {
        TbName.Text!.ContainLengthBetweenValues(new LengthVector(1, 30), "Название от 1 до 30 символов");
        TbDescription.Text!.ContainLengthBetweenValues(new LengthVector(1, 255), "Описание от 1 до 255 символов");
        TbUid.Text!.ContainLengthBetweenValues(new LengthVector(1, 5), "Артикул от 1 до 5 символов");
        TbPercent.Text!.ContainLengthBetweenValues(new LengthVector(1, 10), "Введите процент!");
    }
    
    private async Task SaveChanges()
    {
        var operationRepository = new OperationRepository();

        if (_operationEntity.Id == 0)
        {
            await operationRepository.CreateAsync(_operationEntity);
        }
        else
        {
            await operationRepository.UpdateAsync(_operationEntity);
        }
    }
    
    public override string ToString()
    {
        return PageTitles.AddOperation;
    }

    private void KeyDownOnPercentField(object? sender, KeyEventArgs e)
    {
        var numberValidation = new NumberValidation();
        
        Key key = e.Key;
        if (numberValidation.AddNumberValidation().AddPointValidation().Validate(key))
        {
            e.Handled = true;
        }
    }
}