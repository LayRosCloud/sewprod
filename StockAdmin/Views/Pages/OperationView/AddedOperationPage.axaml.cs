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

    public AddedOperationPage(ContentControl frame) : this(frame, new Operation())
    {
        
    }
    
    public AddedOperationPage(ContentControl frame, Operation operation)
    {
        InitializeComponent();
        _frame = frame;
        _operation = operation;
        DataContext = _operation;
    }
    
    private async void SaveChanges(object? sender, RoutedEventArgs e)
    {
        var operationRepository = new OperationRepository();

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
        if (key < Key.D0 || e.Key> Key.D9 && e.Key < Key.NumPad0 || e.Key> Key.NumPad9)
        {
            e.Handled = true;
        }
    }
}