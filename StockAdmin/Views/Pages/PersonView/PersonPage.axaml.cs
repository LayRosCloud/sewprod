using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Exports.Other;
using StockAdmin.Scripts.Exports.Outputs;
using StockAdmin.Scripts.Exports.Outputs.Interfaces;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Repositories.Server;
using StockAdmin.Scripts.Server;
using StockAdmin.Views.Pages.StatisticPeople;

namespace StockAdmin.Views.Pages.PersonView;

public partial class PersonPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly DelayFinder _delayFinder;
    private readonly List<PersonEntity> _persons;
    private readonly IRepositoryFactory _factory;
    
    public PersonPage(ContentControl frame)
    {
        InitializeComponent();
        _factory = ServerConstants.GetRepository();
        _persons = new List<PersonEntity>();
        
        _delayFinder = new DelayFinder(TimeConstants.Ticks, FilteringArrayOnText);
        _frame = frame;
        
        Init();
    }
    
    private async void Init()
    {
        var dataController = new DataController<PersonEntity>(_factory.CreatePersonRepository(), _persons, List);
        var loadingController = new LoadingController<PersonEntity>(LoadingBorder, dataController);
        await loadingController.FetchDataAsync();
    }

    private void FilteringArrayOnText()
    {
        var text = Finder.Text!.ToLower().Trim();
        List.ItemsSource = 
            _persons.Where(x => x.LastName.ToLower().Contains(text)).ToList();
    }
    
    private void NavigateToAddedPersonPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedPersonPage(_frame);
    }

    private void NavigateToEditPersonPage(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
        {
            throw new ArgumentException("Ошибка! Объект не может быть не кнопкой");
        }

        if (button.DataContext is not PersonEntity personEntity)
        {
            throw new ArgumentException("Ошибка! Объект не повешан на список");
        }
        
        _frame.Content = new AddedPersonPage(_frame, personEntity);
    }

    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        try
        {
            var repository = _factory.CreatePersonRepository();

            if (List.SelectedItem is not PersonEntity person)
            {
                return;
            }

            await repository.DeleteAsync(person.Id);
            Init();
        }
        catch (Exception)
        {
            ElementConstants.ErrorController.AddErrorMessage(Constants.DeletedExceptionMessage);
        }
    }

    private void ShowDeleteWindow(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = true;
    }

    private void NavigateToMoreInformation(object? sender, TappedEventArgs e)
    {
        if (List.SelectedItem is PersonEntity person)
        {
            _frame.Content = new StatisticPage(person);
        }
    }

    private void TextChanged(object? sender, TextChangedEventArgs e)
    {
        _delayFinder.ChangeText();
    }
    
    public override string ToString()
    {
        return PageTitles.Person;
    }

    [Obsolete("Obsolete")]
    private async void ExportToWord(object? sender, RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog();
        
        var filters = new List<FileDialogFilter>();
        var filter = new FileDialogFilter();
        
        filter.Name = "Word (.docx)";
        
        filter.Extensions = new List<string> { "docx" };
        
        filters.Add(filter);
        
        dialog.Filters = filters;
        
        string? path = await dialog.ShowAsync(ElementConstants.MainContainer);
        if (path == null)
        {
            return;
        }
        
        dialog.Filters = filters;
        List<PersonGroup> groupedPersons = new List<PersonGroup>();

        var repositoryOperations = _factory.CreateClothOperationRepository();
        var repositoryPackages = _factory.CreatePackagesRepository();
        
        var operations = await repositoryOperations.GetAllAsync();
        var packages = await repositoryPackages.GetAllAsync();

        foreach (var item in _persons)
        {
            PersonGroup group = new PersonGroup
            {
                Person = item
            };
            foreach (var operation in operations)
            {
                foreach (var operationPerson in operation.ClothOperationPersons)
                {
                    if (operationPerson.PersonId == item.Id)
                    {
                        group.Operations.Add(operation);
                        break;
                    }
                }
            }

            foreach (var package in packages)
            {
                if (package.PersonId == item.Id)
                {
                    group.Packages.Add(package);
                }
            }
            groupedPersons.Add(group);
        }
        
        var controller = new WordController();
        
        IOutputTable outputTable = new PersonsOutput(groupedPersons);
        
        controller.ExportOnTemplateData(outputTable);
        try
        {
            controller.Save(path);
        }
        catch (Exception)
        {
            ElementConstants.ErrorController.AddErrorMessage("Процесс занят другим. Закройте Word");
        }
    }
}