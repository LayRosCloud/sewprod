using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Exceptions;
using StockAdmin.Scripts.Extensions;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;
using StockAdmin.Scripts.Validations;
using StockAdmin.Scripts.Vectors;

namespace StockAdmin.Views.Pages.ModelView;

public partial class AddedModelPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly ModelEntity _modelEntity;
    private readonly Hashtable _actionList;
    private readonly IRepositoryFactory _factory;
    
    public AddedModelPage(ContentControl frame) : this(frame, new ModelEntity())
    {
        
    }
    
    public AddedModelPage(ContentControl frame, ModelEntity modelEntity)
    {
        InitializeComponent();

        if (modelEntity.Id != 0)
        {
            TabPrices.IsVisible = false;
        }
        
        _factory = ServerConstants.GetRepository();
        _frame = frame;
        _modelEntity = modelEntity;
        
        _actionList = new Hashtable();
        
        InitActions();
        Init();
    }

    private void InitActions()
    {
        _actionList.Add(Key.Enter, CreateNewElement);
        _actionList.Add(Key.Decimal, DestroyPriceTextBox);
    }
    
    private async void Init()
    {
        var repository = _factory.CreateOperationRepository();
        CbOperations.ItemsSource = await repository.GetAllAsync();
        DataContext = _modelEntity;
    }
    
    private async void TrySaveChanges(object? sender, RoutedEventArgs e)
    {
        try
        {
            CheckFields();
            var (operations, prices) = GetFromInterfaceItems();
            await SaveChanges(operations, prices);

            _frame.Content = new ModelPage(_frame);
        }
        catch (MyValidationException ex)
        {
            ElementConstants.ErrorController.AddErrorMessage(ex.Message);
        }
        catch (Exception)
        {
            ElementConstants.ErrorController.AddErrorMessage(Constants.UnexpectedAdminExceptionMessage);
        }
        finally
        {
            LoadingBorder.IsVisible = false;
        }
    }

    private (IEnumerable<OperationEntity>, IEnumerable<PriceEntity>) GetFromInterfaceItems()
    {
        var operations = new List<OperationEntity>();
        var prices = new List<PriceEntity>();
        if (_modelEntity.Id == 0)
        {
            operations.AddRange(GetOperations());
            prices.AddRange(GetPrices());
        }

        return (operations, prices);
    }

    private void CheckFields()
    {
        var title = TbTitle.Text!;
        var codeVendor = TbCodeVendor.Text!;
        title.ContainLengthBetweenValues(new LengthVector(1, 255), "Длина названия от 1 до 255 символов!");
        codeVendor.ContainLengthBetweenValues( new LengthVector(1, 30), "Длина артикула от 1 до 30 символов!");
    }
    
    private IEnumerable<PriceEntity> GetPrices()
    {
        var countItems = PricePanel.Children.Count - 1;
        const string exceptionValidationMessage = "Введите в каждое поле цену!";
        var prices = new List<PriceEntity>();
            
        for (int indexChild = 0; indexChild < countItems; indexChild++)
        {
            if(PricePanel.Children[indexChild] is StackPanel stackPanel && stackPanel.Children[0] is TextBox textBox)
            {
                if (String.IsNullOrEmpty(textBox.Text!.Trim()))
                {
                    continue;
                }
                prices.Add(new PriceEntity{Number = Convert.ToDouble(textBox.Text)});
            }
            else
            {
                throw new MyValidationException(exceptionValidationMessage);
            }
        }

        return prices;
        }
    
    private IEnumerable<OperationEntity> GetOperations()
    {
        var countItems = OperationsPanel.Children.Count - 1;
        const int firstItem = 0;

        var operations = new List<OperationEntity>();
        
        for (int indexChild = firstItem; indexChild < countItems; indexChild++)
        {
            var comboBox = (OperationsPanel.Children[indexChild] as StackPanel)!.Children[firstItem] as ComboBox;
            if (comboBox!.SelectedItem is OperationEntity entity)
            {
                operations.Add(entity);
            }
        }

        return operations;
    }

    private async Task SaveChanges(IEnumerable<OperationEntity> operations, IEnumerable<PriceEntity> prices)
    {
        LoadingBorder.IsVisible = true;
        if (_modelEntity.Id == 0)
        {
            var model = await SaveModel();

            await SaveOperations(operations, model);

            await SavePrices(prices, model);
        }
        else
        {
            await UpdateModel();
        }
    }

    private async Task<ModelEntity> SaveModel()
    {
        var repository = _factory.CreateModelRepository();
        var model = await repository.CreateAsync(_modelEntity);
        return model;
    }

    private async Task SaveOperations(IEnumerable<OperationEntity> operations, ModelEntity model)
    {
        var operationRepository = _factory.CreateModelOperationRepository();
        
        foreach (var operation in operations)
        {
            var modelOperation = new ModelOperationEntity()
            {
                ModelId = model.Id,
                OperationId = operation.Id
            };
                
            await operationRepository.CreateAsync(modelOperation);
        }
    }
    
    private async Task SavePrices(IEnumerable<PriceEntity> prices, ModelEntity model)
    {
        var priceRepository = _factory.CreatePriceRepository();
        var modelPriceRepository = _factory.CreateModelPriceRepository();
        foreach (var price in prices)
        {
            var createdPrice = await priceRepository.CreateAsync(price);
            var modelPrice = new ModelPriceEntity()
            {
                PriceId = createdPrice.Id,
                ModelId = model.Id
            };

            await modelPriceRepository.CreateAsync(modelPrice);
        }
    }
    
    

    private async Task<ModelEntity> UpdateModel()
    {
        var repository = _factory.CreateModelRepository();
        _modelEntity.Prices = null;
        _modelEntity.Operations = null;
        return await repository.UpdateAsync(_modelEntity);
    }


    public override string ToString()
    {
        return PageTitles.AddModel;
    }

    private void InputSymbol(object? sender, KeyEventArgs e)
    {
        var key = e.Key;
        object? actionObj = _actionList[key];
        NumberValidation validation = new NumberValidation();
        if (actionObj is Action<object?> action)
        {
            action.Invoke(sender!);
        }

        if (validation.AddPointValidation().AddNumberValidation().Validate(e.Key))
        {
            e.Handled = true;
        }
    }
    
    private void CreateNewElement(object? sender)
    {
        var controller = new ItemControlController();
        
        var stackPanel = PricePanel.Children[^2] as StackPanel;
        var lastText = stackPanel.Children[0] as TextBox;
        var removeButton = stackPanel.Children[1] as Button;
        var countTextBox = controller.CreateTextBox(lastText!, InputSymbol);
        
        var containerController = new ContainerController(controller.CreateStackPanel(stackPanel))
        {
            Controls =
            {
                countTextBox,
                controller.CreateButton(removeButton, RemoveField) //TODO: Event
            }
        };
        countTextBox.TextChanged += ReplaceOnNormalDoubleDigit;
        containerController.PushElementsToPanel();
        containerController.AddPanelToParent(PricePanel, PricePanel.Children.Count - 1);
        
        countTextBox.Focus();
        countTextBox.SelectionStart = countTextBox.Text!.Length;
    }

    private void DestroyPriceTextBox(object? sender)
    {
        if (PricePanel.Children.Count - 1 <= 1)
        {
            return;
        }

        if (sender is TextBox textBox)
        {
            PricePanel.Children.Remove(textBox.Parent as StackPanel);
            (PricePanel.Children[^2] as StackPanel).Children[0].Focus();
        }
        else if (sender is Button button)
        {
            PricePanel.Children.Remove(button.Parent as StackPanel);
            (PricePanel.Children[^2] as StackPanel).Children[0].Focus();
        }
    }
    
    private void AddOperationControl(object? sender, RoutedEventArgs e)
    {
        var parent = (sender as Button)?.Parent as StackPanel;
        var insidePanel = parent!.Children[^2] as StackPanel;
        
        var controller = new ItemControlController();
        var containerController = new ContainerController(controller.CreateStackPanel(insidePanel!))
        {
            Controls =
            {
                controller.CreateComboBox(insidePanel!.Children[0] as ComboBox),
                controller.CreateButton(insidePanel.Children[1] as Button, DeleteCurrentOperationItem)
            }
        };
        
        containerController.PushElementsToPanel();
        
        containerController.AddPanelToParent(parent, parent.Children.Count - 1);
    }

    private void DeleteCurrentOperationItem(object? sender, RoutedEventArgs e)
    {
        var currentStackPanel = (sender as Button)?.Parent as StackPanel;
        var parent = currentStackPanel!.Parent as StackPanel;
        if (parent?.Children.Count - 1 > 1)
        {
            parent.Children.Remove(currentStackPanel);
        }
    }

    private void ReplaceOnNormalDoubleDigit(object? sender, TextChangedEventArgs e)
    {
        if(sender is not TextBox textBox) return;

        string text = textBox.Text!;
        text = text.ToLower();
        const char letterOnChange = ',';

        char[] letterOnChanged = { '.', 'ю', 'б', '<', '>', '/', '?' };
        foreach (var character in letterOnChanged)
        {
            text = text.Replace(character, letterOnChange);
        }

        textBox.Text = text;
    }

    private void CloseCurrentPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new ModelPage(_frame);
    }

    private void RemoveField(object? sender, RoutedEventArgs e)
    {
        DestroyPriceTextBox(sender);
    }

    private void AddField(object? sender, RoutedEventArgs e)
    {
        CreateNewElement(sender);
    }
}