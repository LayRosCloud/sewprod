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
    
    private async void SaveChanges(object? sender, RoutedEventArgs e)
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