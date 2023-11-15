using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.OperationView;

public partial class OperationPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly List<Operation> _operations;
    private readonly FinderController _finderController;
    
    public OperationPage(ContentControl frame)
    {
        InitializeComponent();
        _operations = new List<Operation>();
        _finderController = new FinderController(500, () =>
        {
            List.ItemsSource = 
                _operations
                    .Where(x => x.name.ToLower().Contains(Finder.Text.ToLower().Trim()))
                    .ToList();
        });
        _frame = frame;
        Init();
    }

    private async void Init()
    {
        IDataReader<Operation> operationRepository = new OperationRepository();
        _operations.AddRange(await operationRepository.GetAllAsync());
        List.ItemsSource = _operations;
    }

    private void NavigateToAddedOperationPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedOperationPage(_frame);
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

    private void TextChanged(object? sender, TextChangedEventArgs e)
    {
        _finderController.ChangeText();
    }
    
    public override string ToString()
    {
        return "Операции";
    }
}