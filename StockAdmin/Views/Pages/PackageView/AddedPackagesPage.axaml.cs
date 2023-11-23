using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Extensions;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Server;
using StockAdmin.Scripts.Vectors;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace StockAdmin.Views.Pages.PackageView;

public partial class AddedPackagesPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly Hashtable _actionList;
    
    public AddedPackagesPage(ContentControl frame)
    {
        InitializeComponent();
        _frame = frame;
        _actionList = new Hashtable();
        
        InitActionList();
        
        Init();
    }

    private void InitActionList()
    {
        _actionList.Add(Key.Enter, CreateNewElement);
        _actionList.Add(Key.Up, NavigateToUp);
        _actionList.Add(Key.Down, NavigateToDown);
        _actionList.Add(Key.Decimal, RemoveSelectedTextBox);
    }

    private async void Init()
    {
        var ageRepository = new AgeRepository();
        var materialRepository = new MaterialRepository();

        var partyRepository = new PartyRepository();
        var modelRepository = new ModelRepository();
        var personRepository = new PersonRepository();
        
        CmbAges.ItemsSource = await ageRepository.GetAllAsync();
        CmbMaterials.ItemsSource = await materialRepository.GetAllAsync();
        var list = await personRepository.GetAllAsync();
        CbPersons.ItemsSource = list;
        CmbPersons.ItemsSource = list;
        CbModels.ItemsSource = await modelRepository.GetAllAsync();
        CbParties.ItemsSource = await partyRepository.GetAllAsync();
    }
    
    private async void TrySaveElements(object? sender, RoutedEventArgs e)
    {
        var packageRepository = new PackageRepository();

        try
        {
            PartyEntity partyEntity;

            if (IsNewCut.IsChecked == true)
            {
                partyEntity = CreateParty();
            }
            else
            {
                if (CbParties.SelectedItem is not PartyEntity party)
                {
                    throw new ValidationException("Выберите крой");
                }

                partyEntity = party;
            }

            partyEntity = await new PartyRepository().CreateAsync(partyEntity);
            var packagesList = ReadAllPackagesTextBox(partyEntity);
            
            if (CbPrices.SelectedItem is not PriceEntity priceEntity)
            {
                throw new ValidationException("Выберите цену!");
            }
            
            var packageEntities =  await packageRepository.CreateAsync(packagesList);
            var model = CbModels.SelectedItem as ModelEntity;
            
            var priceRepository = new PriceRepository();
            var clothOperationRepository = new ClothOperationRepository();
            
            foreach (var package in packageEntities!)
            {
                foreach (var operation in model!.Operations!)
                {
                    var price = new PriceEntity
                    {
                        Number = priceEntity.Number * operation.Percent / 100.0
                    };

                    var createdPrice = await priceRepository.CreateAsync(price);
                    
                    var clothOperationEntity = new ClothOperationEntity
                    {
                        OperationId = operation.Id,
                        PriceId = createdPrice.Id,
                        PackageId = package.Id
                    };

                    await clothOperationRepository.CreateAsync(clothOperationEntity);
                }
            }
            
            _frame.Content = new PackagePage(_frame);
        }
        catch (ValidationException ex)
        {
            ElementConstants.ErrorController.AddErrorMessage(ex.Message);
        }
        catch (Exception)
        {
            ElementConstants.ErrorController.AddErrorMessage("Ошибка! проверьте все поля на ввод");
        }
    }

    private PartyEntity CreateParty()
    {
        (var model, var person, var dateStart, var price, var cutNumber) = CheckFieldsParty();
        
        var partyEntity = new PartyEntity()
        {
            CutNumber = cutNumber,
            DateStart = dateStart,
            DateEnd = CdpDateStart.SelectedDate!.Value,
            PersonId = person.Id,
            PriceId = price.Id,
            ModelId = model.Id,
            Price = price
        };
        return partyEntity;
    }

    private (ModelEntity, PersonEntity, DateTime, PriceEntity, string) CheckFieldsParty()
    {
        string cutNumber = TbCutNumber.Text!;
        cutNumber.ContainLengthBetweenValues(new LengthVector(1, 10), "Артикул партии может быть от 1 до 10 символов");
        
        if (CbModels.SelectedItem is not ModelEntity modelEntity)
        {
            throw new ValidationException("Выберите модель!");
        }
        
        if (CbPersons.SelectedItem is not PersonEntity personEntity)
        {
            throw new ValidationException("Выберите человека кройщика");
        }

        if (CdpDateStart.SelectedDate == null)
        {
            throw new ValidationException("Выберите дату начала кройки");
        }
        
        if (CbPrices.SelectedItem is not PriceEntity priceEntity)
        {
            throw new ValidationException("Выберите цену!");
        }
        
        return (modelEntity, personEntity, CdpDateStart.SelectedDate.Value, priceEntity, cutNumber);
    }
    
    private List<PackageEntity> ReadAllPackagesTextBox(PartyEntity party)
    {
        if (CmbSizes.SelectedItem is not SizeEntity sizeEntity)
        {
            throw new ValidationException("Выберите размер!");
        }
        
        var packages = new List<PackageEntity>();
        
        foreach (var control in MainPanel.Children)
        {
            if (control is not StackPanel stackPanel) continue;
            
            var tbCount = (stackPanel.Children[0] as TextBox)!;
            var cbMaterials = (stackPanel.Children[1] as ComboBox)!;

            var count = Convert.ToInt32(tbCount.Text);
            var materialEntity = cbMaterials.SelectedItem as MaterialEntity;

            var packageEntity = new PackageEntity
              { 
                Count = count, 
                SizeId = sizeEntity.Id, 
                PersonId = ServerConstants.Token.Id, 
                MaterialId = materialEntity!.Id,
                PartyId = party.Id
              };
                
            packages.Add(packageEntity);
        }

        return packages;
    }

    private void InputSymbol(object? sender, KeyEventArgs e)
    {
        object? obj = _actionList[e.Key];
        
        if (obj is Action<object> action)
        {
            action.Invoke(sender!);
        }

        char k = 'a';
        if ((e.Key < Key.D0 || e.Key > Key.D9) && (e.Key< Key.NumPad0 || e.Key > Key.NumPad9))
        {
            e.Handled = true;
        }
    }

    private void RemoveSelectedTextBox(object sender)
    {
        if (MainPanel.Children.Count > 1 && sender is TextBox textBox)
        {
            MainPanel.Children.Remove(textBox.Parent as StackPanel);
            (MainPanel.Children[^1] as StackPanel).Children[0].Focus();
        }
    }
    
    private void CreateNewElement(object sender)
    {
        var controller = new ItemControlController();
        
        var stack = (MainPanel.Children[^1] as StackPanel)!;
        var lastText = stack.Children[0] as TextBox;
        var cbComboMaterials = stack.Children[1] as ComboBox;
        var countTextBox = controller.CreateTextBox(lastText!, InputSymbol);
        
        var containerController = new ContainerController(controller.CreateStackPanel(stack))
        {
            Controls =
            {
                countTextBox,
                controller.CreateComboBox(cbComboMaterials!)
            }
        };
        
        containerController.PushElementsToPanel();
        containerController.AddPanelToParent(MainPanel);
        
        countTextBox.Focus();
        countTextBox.SelectionStart = countTextBox.Text!.Length;
    }

    private void NavigateToUp(object sender)
    {
        int index = GetIndexOfElement(sender);
        
        if (index > 0)
        {
            StackPanel stackPanel = MainPanel.Children[index - 1] as StackPanel;
            stackPanel!.Children[0].Focus();
        }
        else
        {
            StackPanel stackPanel = MainPanel.Children[^1] as StackPanel;
            stackPanel!.Children[0].Focus();
        }
    }
    
    private void NavigateToDown(object sender)
    {
        int index = GetIndexOfElement(sender);
        
        if (index < MainPanel.Children.Count - 1)
        {
            StackPanel stackPanel = MainPanel.Children[index + 1] as StackPanel;
            stackPanel.Children[0].Focus();
        }
        else
        {
            StackPanel stackPanel = MainPanel.Children[0] as StackPanel;
            stackPanel.Children[0].Focus();
        }
    }

    private int GetIndexOfElement(object sender)
    {
        StackPanel stackPanel = (sender as TextBox).Parent as StackPanel;
        return MainPanel.Children.IndexOf(stackPanel);
    }
    
    private void CheckOnPartyAdded(object? sender, RoutedEventArgs e)
    {
        PanelSelectedItem.IsVisible = IsNewCut.IsChecked == false;
    }

    private async void SelectedTypeOfSize(object? sender, SelectionChangedEventArgs e)
    {
        if (CmbAges.SelectedItem is not AgeEntity entity) return;
        
        var repository = new SizeRepository();
        
        CmbSizes.ItemsSource = await repository.GetAllAsync(entity.Id);

    }
    
    public override string ToString()
    {
        return "Добавление пачек";
    }

    private async void SelectedPerson(object? sender, SelectionChangedEventArgs e)
    {
        if(CmbPersons.SelectedItem is not PersonEntity personEntity) return;
        
        var repository = new PartyRepository();
        CbParties.ItemsSource = await repository.GetAllAsync(personEntity.Id);
    }

    private void Back(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new PackagePage(_frame);
    }

    private void SelectedModel(object? sender, SelectionChangedEventArgs e)
    {
        CbPrices.ItemsSource = (CbModels.SelectedItem as ModelEntity).Prices;
    }
}