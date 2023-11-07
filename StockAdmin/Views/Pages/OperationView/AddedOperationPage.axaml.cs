using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.OperationView;

public partial class AddedOperationPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly Operation _operation;
    private readonly List<Price> _prices;

    public AddedOperationPage(ContentControl frame) : this(frame, new Operation())
    {
        
    }
    
    public AddedOperationPage(ContentControl frame, Operation operation)
    {
        InitializeComponent();
        _frame = frame;
        _operation = operation;
        _prices = new List<Price>();
        Init();
    }

    private async void Init()
    {
        IDataReader<Price> repository = new PriceRepository();
        _prices.AddRange( await repository.GetAllAsync());
        CbPrices.ItemsSource = _prices;
        DataContext = _operation;
    }
    
    private async void SaveChanges(object? sender, RoutedEventArgs e)
    {
        var operationRepository = new OperationRepository();
        var priceRepository = new PriceRepository();
        var number = Convert.ToDouble(CbPrices.Text);
        var price = new Price { number = number, date = DateTime.Now};
        
        Price createdPrice = await priceRepository.CreateAsync(price);
        _operation.priceId = createdPrice.id;
        _operation.price = createdPrice;

        if (_operation.id == 0)
        {
            await operationRepository.CreateAsync(_operation);
        }
        else
        {
            await operationRepository.UpdateAsync(_operation);
        }

        _frame.Content = new OperationPage(_frame);

    }
    
    public override string ToString()
    {
        return "Добавление / Обновление операции";
    }

    private void KeyDownOnPriceField(object? sender, KeyEventArgs e)
    {
        Key key = e.Key;
        if ((key < Key.D0 || e.Key> Key.D9 && e.Key < Key.NumPad0 || e.Key> Key.NumPad9) &&  key != Key.OemComma && key != Key.OemPeriod)
        {
            e.Handled = true;
        }
    }

    private void InputTextInPriceField(object? sender, TextChangedEventArgs e)
    {
        CbPrices.Text = CbPrices.Text!.Replace('.', ',');
        CbPrices.Text = CbPrices.Text!.Replace('<', ',');
        CbPrices.Text = CbPrices.Text!.Replace('>', ',');
        CbPrices.Text = CbPrices.Text!.Replace('Б', ',');
        CbPrices.Text = CbPrices.Text!.Replace('Ю', ',');
    }
}