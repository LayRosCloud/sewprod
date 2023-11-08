﻿using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.OperationView;

public partial class OperationPage : UserControl
{
    private readonly ContentControl _frame;
    public OperationPage(ContentControl frame)
    {
        InitializeComponent();
        _frame = frame;
        Init();
    }

    private async void Init()
    {
        IDataReader<Operation> operationRepository = new OperationRepository();
        List.ItemsSource = await operationRepository.GetAllAsync();
    }

    private void NavigateToAddedOperationPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedOperationPage(_frame);
    }
    
    public override string ToString()
    {
        return "Операции";
    }

    private void NavigateToEditOperationPage(object? sender, RoutedEventArgs e)
    {
        Operation operation = (sender as Button).DataContext as Operation;
        _frame.Content = new AddedOperationPage(_frame, operation);
    }
    
    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repository = new OperationRepository();
        
        if (List.SelectedItem is Operation operation)
        {
            await repository.DeleteAsync(operation.id);
        }

        SendNoAnswerOnDeleteItem(sender, e);
        Init();
    }

    private void SendNoAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = false;
    }

    private void ShowDeleteWindow(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = true;
        DeletedMessage.Text =
            "вы действительно уверены, что хотите удалить операцию?" +
            " Восстановить операцию будет нельзя!";
    }
}