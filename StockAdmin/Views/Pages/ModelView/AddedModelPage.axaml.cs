using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Exceptions;
using StockAdmin.Scripts.Extensions;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Vectors;

namespace StockAdmin.Views.Pages.ModelView;

public partial class AddedModelPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly ModelEntity _modelEntity;
    private readonly PriceRepository _priceRepository;

    public AddedModelPage(ContentControl frame) : this(frame, new ModelEntity())
    {
    }
    
    public AddedModelPage(ContentControl frame, ModelEntity modelEntity)
    {
        InitializeComponent();
        _frame = frame;
        _modelEntity = modelEntity;
        _priceRepository = new PriceRepository();
        Init();
    }

    private void Init()
    {
        DataContext = _modelEntity;
    }
    
    private async void TrySaveChanges(object? sender, RoutedEventArgs e)
    {
        try
        {
            CheckFields();
            await SaveChanges();

            _frame.Content = new ModelPage(_frame);
        }
        catch (ValidationException ex)
        {
            ElementConstants.ErrorController.AddErrorMessage(ex.Message);
        }
    }

    private void CheckFields()
    {
        var title = TbTitle.Text!;
        var codeVendor = TbCodeVendor.Text!;
        title.ContainLengthBetweenValues(new LengthVector(1, 255), "Длина названия от 1 до 255 символов!");
        codeVendor.ContainLengthBetweenValues( new LengthVector(1, 30), "Длина артикула от 1 до 30 символов!");
        var regex = new Regex(@"^\d+$");
        
        //TODO: changed on more prices
    }

    private async Task SaveChanges()
    {
        var repository = new ModelRepository();

        if (_modelEntity.Id == 0)
        {
            await repository.CreateAsync(_modelEntity);
        }
        else
        {
            await repository.UpdateAsync(_modelEntity);
        }
    }
    
    public override string ToString()
    {
        return "Добавление / Обновление моделей";
    }
}