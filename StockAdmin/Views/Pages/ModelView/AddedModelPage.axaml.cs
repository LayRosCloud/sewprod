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
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Repositories.Server;
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
        _factory = ServerConstants.GetRepository();
        _frame = frame;
        _modelEntity = modelEntity;
        
        _actionList = new Hashtable();
        
        InitActions();
        Init();
    }

    private void InitActions()
    {
        _actionList.Add(Key.Up, "");
        _actionList.Add(Key.Down, "");
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
            var operations = GetOperations();
            var prices = GetPrices();
            CheckFields();
            await SaveChanges(operations, prices);

            _frame.Content = new ModelPage(_frame);
        }
        catch (ValidationException ex)
        {
            ElementConstants.ErrorController.AddErrorMessage(ex.Message);
        }
        catch (Exception)
        {
            ElementConstants.ErrorController.AddErrorMessage("Непредвиденная ошибка");
        }
        finally
        {
            LoadingBorder.IsVisible = false;
        }
    }

    private void CheckFields()
    {
        var title = TbTitle.Text!;
        var codeVendor = TbCodeVendor.Text!;
        title.ContainLengthBetweenValues(new LengthVector(1, 255), "Длина названия от 1 до 255 символов!");
        codeVendor.ContainLengthBetweenValues( new LengthVector(1, 30), "Длина артикула от 1 до 30 символов!");
    }
    
    private List<PriceEntity> GetPrices()
    {
        var prices = new List<PriceEntity>();
            
        for (int i = 0; i < PricePanel.Children.Count - 1; i++)
        {
            if(PricePanel.Children[i] is TextBox textBox)
            {
                prices.Add(new PriceEntity{Number = Convert.ToDouble(textBox.Text)});
            }
            else
            {
                throw new ValidationException("Введите в каждое поле цену");
            }
        }

        return prices;
        }
    
    private List<OperationEntity> GetOperations()
    {
        var operations = new List<OperationEntity>();
        
        for (int index = 0; index < OperationsPanel.Children.Count - 1; index++)
        {
            ComboBox comboBox = (OperationsPanel.Children[index] as StackPanel).Children[0] as ComboBox;
            if (comboBox.SelectedItem is OperationEntity entity)
            {
                operations.Add(entity);
            }
            else
            {
                throw new ValidationException("Выберите все операции!");
            }
        }

        return operations;
        }

    private async Task SaveChanges(List<OperationEntity> operations, List<PriceEntity> prices)
    {
        
        var repository = _factory.CreateModelRepository();
        LoadingBorder.IsVisible = true;
        if (_modelEntity.Id == 0)
        {
            var model = await repository.CreateAsync(_modelEntity);
            var operationRepository = _factory.CreateModelOperationRepository();
            var priceRepository = _factory.CreatePriceRepository();
            var modelPriceRepository = _factory.CreateModelPriceRepository();
            foreach (var operation in operations)
            {
                var modelOperation = new ModelOperationEntity()
                {
                    ModelId = model.Id,
                    OperationId = operation.Id
                };
                
                await operationRepository.CreateAsync(modelOperation);
            }

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
        else
        {
            await repository.UpdateAsync(_modelEntity);
        }
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
        
        var lastText = PricePanel.Children[^1] as TextBox;
        var countTextBox = controller.CreateTextBox(lastText!, InputSymbol);
        
        var containerController = new ContainerController(PricePanel)
        {
            Controls =
            {
                countTextBox,
            }
        };
        countTextBox.TextChanged += ReplaceOnNormalDoubleDigit;
        containerController.PushElementsToPanel();
        
        countTextBox.Focus();
        countTextBox.SelectionStart = countTextBox.Text!.Length;
    }

    private void DestroyPriceTextBox(object? sender)
    {
        if (PricePanel.Children.Count <= 1)
        {
            return;
        }
        PricePanel.Children.Remove(sender as TextBox);
        (PricePanel.Children[^1] as TextBox).Focus();
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
}