using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Repositories.Server;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Views.Pages.OperationView;

public partial class OperationPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly List<OperationEntity> _operations;
    private readonly DelayFinder _delayFinder;
    private readonly IRepositoryFactory _factory;
    
    public OperationPage(ContentControl frame)
    {
        InitializeComponent();
        
        _factory = ServerConstants.GetRepository();
        
        _operations = new List<OperationEntity>();
        _delayFinder = new DelayFinder(TimeConstants.Ticks, FilteringArrayOnText);
        _frame = frame;
        
        Init();
    }

    private async void Init()
    {
        var dataController 
            = new DataController<OperationEntity>(_factory.CreateOperationRepository(), _operations, List);
        
        var loadingController 
            = new LoadingController<OperationEntity>(LoadingBorder, dataController);
        
        await loadingController.FetchDataAsync();
    }

    private void FilteringArrayOnText()
    {
        string text = Finder.Text.ToLower().Trim();
        List.ItemsSource = 
            _operations
                .Where(x => x.Name.ToLower().Contains(text))
                .ToList();
    }
    
    private void NavigateToAddedOperationPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedOperationPage(_frame);
    }

    private void NavigateToEditOperationPage(object? sender, RoutedEventArgs e)
    {
        OperationEntity operationEntity = (sender as Button).DataContext as OperationEntity;
        _frame.Content = new AddedOperationPage(_frame, operationEntity);
    }
    
    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repository = _factory.CreateOperationRepository();
        
        if (List.SelectedItem is OperationEntity operation)
        {
            await repository.DeleteAsync(operation.Id);
            Init();
        }
    }

    private void ShowDeleteWindow(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = true;
    }

    private void TextChanged(object? sender, TextChangedEventArgs e)
    {
        _delayFinder.ChangeText();
    }
    
    public override string ToString()
    {
        return PageTitles.Operation;
    }
}