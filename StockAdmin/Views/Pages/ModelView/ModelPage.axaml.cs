using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Records;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Repositories.Server;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Views.Pages.ModelView;

public partial class ModelPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly List<ModelEntity> _models;
    private readonly DelayFinder _delayFinder;
    private ListSelected _currentSelectedList;
    private readonly IRepositoryFactory _factory;
    public ModelPage(ContentControl frame)
    {
        InitializeComponent();
        _factory = ServerConstants.GetRepository();
        _frame = frame;
        _models = new List<ModelEntity>();
        _delayFinder = new DelayFinder(TimeConstants.Ticks, FilteringArrayOnText);
        InitData();
    }

    private async Task InitData()
    {
        var dataController = new DataController<ModelEntity>(_factory.CreateModelRepository(), _models, List);
        var loadingController = new LoadingController<ModelEntity>(LoadingBorder, dataController);
        await loadingController.FetchDataAsync();
    }

    private void FilteringArrayOnText()
    {
        List.ItemsSource = _models
            .Where(x => x.Title.ToLower().Contains(Finded.Text.ToLower()))
            .ToList();
    }
    
    private void NavigateToCreatePage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedModelPage(_frame);
    }

    private void NavigateToEditPage(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        if (button.DataContext is not ModelEntity model)
        {
            return;
        }
        _frame.Content = new AddedModelPage(_frame, model);
    }
    
    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        try
        {
            await RemoveModelItems();
        }
        catch (Exception)
        {
            ElementConstants.ErrorController.AddErrorMessage(Constants.DeletedExceptionMessage);
        }
        
    }

    private async Task RemoveModelItems()
    {
        var repository = _factory.CreateModelRepository();
        var modelPricesRepository = _factory.CreateModelPriceRepository();
        var modelOperationRepository = _factory.CreateModelOperationRepository();
        switch (_currentSelectedList)
        {
            case ListSelected.First:
                if (List.SelectedItem is ModelEntity model)
                {
                    await repository.DeleteAsync(model.Id);
                }

                break;
            case ListSelected.Second:
                if (Operations.SelectedItem is OperationEntity operation)
                {
                    await modelOperationRepository.DeleteAsync(operation.ModelOperation ?? new ModelOperationEntity
                    {
                        ModelId = (List.SelectedItem as ModelEntity).Id,
                        OperationId = operation.Id
                    });
                }

                break;
            case ListSelected.Third:
                if (Prices.SelectedItem is PriceEntity price)
                {
                    await modelPricesRepository.DeleteAsync(price.ModelPrice ?? new ModelPriceEntity
                    {
                        ModelId = (List.SelectedItem as ModelEntity).Id,
                        PriceId = price.Id
                    });
                }

                break;
        }

        DeletedContainer.IsVisible = false;

        await InitData();
    }

    private void SendNoAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = false;
    }

    private void ShowDeleteWindow(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = true;
        DeletedContainer.Text =
            "вы действительно уверены, что хотите удалить модель?" +
            " Восстановить модель будет нельзя!";
        _currentSelectedList = ListSelected.First;
    }

    private void TextChanged(object? sender, TextChangedEventArgs e)
    {
        _delayFinder.ChangeText();
    }

    private void SelectedModel(object? sender, SelectionChangedEventArgs e)
    {
        if(List.SelectedItem is not ModelEntity model) return;
        
        Prices.ItemsSource = model.Prices;
        Operations.ItemsSource = model.Operations;
    }

    private void ShowDeleteWindowOperation(object? sender, RoutedEventArgs e)
    {
        _currentSelectedList = ListSelected.Second;

        DeletedContainer.IsVisible = true;
        DeletedContainer.Text =
            "вы действительно уверены, что хотите удалить операцию над моделью?" +
            " Восстановить операцию над моделью будет нельзя!";
    }

    private void ShowDeleteWindowPrice(object? sender, RoutedEventArgs e)
    {
        _currentSelectedList = ListSelected.Third;

        DeletedContainer.IsVisible = true;
        DeletedContainer.Text =
            "вы действительно уверены, что хотите удалить цену над моделью?" +
            " Восстановить цену над моделью будет нельзя!";
    }
    
    public override string ToString()
    {
        return PageTitles.Model;
    }

    private void NavigateToAddedOperationPage(object? sender, RoutedEventArgs e)
    {
        if (List.SelectedItem is ModelEntity modelEntity)
        {
            _frame.Content = new AddedModelOperationPage(_frame, modelEntity);
        }
    }

    private void NavigateToAddedPricePage(object? sender, RoutedEventArgs e)
    {
        if (List.SelectedItem is ModelEntity modelEntity)
        {
            _frame.Content = new AddedModelPricePage(_frame, modelEntity);
        }
    }
}