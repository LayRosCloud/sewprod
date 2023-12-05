using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Exceptions;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Validations;

namespace StockAdmin.Views.Pages.ModelView;

public partial class AddedModelPricePage : UserControl
{
    private readonly ContentControl _frame;
    private readonly ModelEntity _model;
    public AddedModelPricePage(ContentControl frame, ModelEntity model)
    {
        InitializeComponent();
        _frame = frame;
        _model = model;
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

    private async Task SaveChanges()
    {
        var modelPriceRepository = new ModelPriceRepository();
        var priceRepository = new PriceRepository();
        string priceText = TbPrice.Text!;

        double number = Convert.ToDouble(priceText);
        var price = await priceRepository.CreateAsync(new PriceEntity { Number = number });
        await modelPriceRepository.CreateAsync(new ModelPriceEntity{ModelId = _model.Id, PriceId = price.Id});
    }
    
    private void CheckFields()
    {
        if (TbPrice.Text.Length == 0)
        {
            throw new ValidationException("Введите цену!");
        }
    }

    public override string ToString()
    {
        return PageTitles.AddModelPrice;
    }

    private void InputSymbol(object? sender, KeyEventArgs e)
    {
        NumberValidation validation = new NumberValidation();
        if (validation.AddNumberValidation().AddPointValidation().Validate(e.Key))
        {
            e.Handled = true;
        }
    }
}