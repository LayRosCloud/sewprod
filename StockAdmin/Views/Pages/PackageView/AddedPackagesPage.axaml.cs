using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
using StockAdmin.Scripts.Vectors;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace StockAdmin.Views.Pages.PackageView;

public partial class AddedPackagesPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly Hashtable _actionList;
    private readonly IRepositoryFactory _factory;
    
    public AddedPackagesPage(ContentControl frame)
    {
        InitializeComponent();
        _factory = ServerConstants.GetRepository();
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
        _actionList.Add(Key.Decimal, RemoveSelectedField);
    }

    private async void Init()
    {
        var ageRepository = _factory.CreateAgeRepository();
        var materialRepository = _factory.CreateMaterialRepository();

        var modelRepository = _factory.CreateModelRepository();
        var personRepository = _factory.CreatePersonRepository();
        
        CmbAges.ItemsSource = await ageRepository.GetAllAsync();
        CmbMaterials.ItemsSource = await materialRepository.GetAllAsync();
        var list = await personRepository.GetAllAsync();
        
        CbPersons.ItemsSource = list;
        CmbPersons.ItemsSource = list;
        CmPersons.ItemsSource = list;
        
        CbModels.ItemsSource = await modelRepository.GetAllAsync();
        //CbParties.ItemsSource = await partyRepository.GetAllAsync();
    }
    
    private async void TrySaveElements(object? sender, RoutedEventArgs e)
    {
        var packageRepository = _factory.CreatePackagesRepository();
        var priceRepository = _factory.CreatePriceRepository();
        var clothOperationRepository = _factory.CreateClothOperationRepository();
        var partyRepository = _factory.CreatePartyRepository();
        
        try
        {
            PartyEntity partyEntity;
            PriceEntity priceEntity;

            if (IsNewCut.IsChecked == true)
            {
                partyEntity = CreateParty();
                priceEntity = partyEntity.Price!;
                partyEntity.Price = null;
                partyEntity = await partyRepository.CreateAsync(partyEntity);
            }
            else
            {
                if (CbParties.SelectedItem is PartyEntity party)
                {
                    partyEntity = party;
                    priceEntity = party.Price!;
                }
                else
                {
                    throw new MyValidationException("Ошибка! Крой не выбран");
                }
            }

            LoadingBorder.IsVisible = true;
            var packagesList = ReadAllPackagesTextBox(partyEntity);

            var packageEntities = await packageRepository.CreateAsync(packagesList);

            var model = IsNewCut.IsChecked == true ? CbModels.SelectedItem as ModelEntity : partyEntity.Model;
            
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
        catch (MyValidationException ex)
        {
            ElementConstants.ErrorController.AddErrorMessage(ex.Message);
        }
        catch (Exception)
        {
            ElementConstants.ErrorController.AddErrorMessage("Проверьте все поля на корректность данных или " + Constants.UnexpectedAdminExceptionMessage);
        }
        finally
        {
            LoadingBorder.IsVisible = false;
        }
    }
    
    
    private PartyEntity CreateParty()
    {
        (var model, var person, var dateStart, var price, var cutNumber) = CheckFieldsParty();
        
        var partyEntity = new PartyEntity()
        {
            CutNumber = cutNumber,
            DateStart = dateStart,
            DateEnd = CdpDateEnd.SelectedDate,
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
            var cbPersons = (stackPanel.Children[0] as ComboBox)!;
            var tbCount = (stackPanel.Children[1] as TextBox)!;
            var cbMaterials = (stackPanel.Children[2] as ComboBox)!;

            var count = Convert.ToInt32(tbCount.Text);
            var materialEntity = cbMaterials.SelectedItem as MaterialEntity;
            var person = cbPersons.SelectedItem as PersonEntity;

            var packageEntity = new PackageEntity
              { 
                Count = count, 
                SizeId = sizeEntity.Id, 
                PersonId = person!.Id, 
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
        
        if ((e.Key < Key.D0 || e.Key > Key.D9) && (e.Key< Key.NumPad0 || e.Key > Key.NumPad9))
        {
            e.Handled = true;
        }
    }

    private void RemoveSelectedField(object sender)
    {
        if (MainPanel.Children.Count - 1 <= 1)
        {
            return;
        }
        
        if (sender is TextBox textBox)
        {
            MainPanel.Children.Remove(textBox.Parent as StackPanel);
            (MainPanel.Children[^2] as StackPanel).Children[1].Focus();
        }
        else if (sender is Button button)
        {
            MainPanel.Children.Remove(button.Parent as StackPanel);
        }
    }
    
    private void CreateNewElement(object sender)
    {
        var controller = new ItemControlController();
        
        var stack = (MainPanel.Children[^2] as StackPanel)!;
        var cbComboPersons = stack.Children[0] as ComboBox;
        var lastText = stack.Children[1] as TextBox;
        var cbComboMaterials = stack.Children[2] as ComboBox;
        var removeButton = stack.Children[3] as Button;
        var countTextBox = controller.CreateTextBox(lastText!, InputSymbol);
        
        var containerController = new ContainerController(controller.CreateStackPanel(stack))
        {
            Controls =
            {
                controller.CreateComboBox(cbComboPersons!),
                countTextBox,
                controller.CreateComboBox(cbComboMaterials!),
                controller.CreateButton(removeButton, RemoveField)
            }
        };
        
        containerController.PushElementsToPanel();
        containerController.AddPanelToParent(MainPanel, MainPanel.Children.Count - 1);
        
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
        if (CmbAges.SelectedItem is not AgeEntity entity)
        {
            return;
        }
        
        var repository = _factory.CreateSizeRepository();
        
        CmbSizes.ItemsSource = await repository.GetAllAsync(entity.Id);

    }
    
    public override string ToString()
    {
        return PageTitles.AddPackage;
    }

    private async void SelectedPerson(object? sender, SelectionChangedEventArgs e)
    {
        if (CmbPersons.SelectedItem is not PersonEntity personEntity)
        {
            return;
        }
        
        var repository = _factory.CreatePartyRepository();
        var list = await repository.GetAllAsync(personEntity.Id);
        var filterList = new List<PartyEntity>(list);

        var now = DateTime.Now;

        var start = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
        var end = new DateTime(now.Year, now.Month, 1).AddMonths(2).AddDays(-1);

        filterList = list.Where(party => party.DateStart > start && party.DateStart < end).ToList();
        CbParties.ItemsSource = filterList;
    }

    private void Back(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new PackagePage(_frame);
    }

    private void SelectedModel(object? sender, SelectionChangedEventArgs e)
    {
        CbPrices.ItemsSource = (CbModels.SelectedItem as ModelEntity).Prices;
    }

    private void AddField(object? sender, RoutedEventArgs e)
    {
        CreateNewElement(sender);
    }

    private void RemoveField(object? sender, RoutedEventArgs e)
    {
        RemoveSelectedField(sender);
    }
}