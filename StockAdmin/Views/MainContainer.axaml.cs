using System;
using System.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Server;
using StockAdmin.Views.Pages.HistoryView;
using StockAdmin.Views.Pages.MaterialView;
using StockAdmin.Views.Pages.PackageView;
using StockAdmin.Views.UserControllers;
using ModelPage = StockAdmin.Views.Pages.ModelView.ModelPage;
using OperationPage = StockAdmin.Views.Pages.OperationView.OperationPage;
using PersonPage = StockAdmin.Views.Pages.PersonView.PersonPage;
using SizePage = StockAdmin.Views.Pages.SizeView.SizePage;

namespace StockAdmin.Views;

public partial class MainContainer : Window
{
    private int _currentIndexActive = 0;
    
    public MainContainer()
    {
        InitializeComponent();
        ElementConstants.MainContainer = this;
        ElementConstants.ErrorController = new ErrorController(ErrorContainer);

        const int indexOnFirstLetter = 0;
        if (ServerConstants.GetRepository() is DatabaseFactory)
        {
            HistoryButton.IsVisible = false;
        }
        
        ShortName.Text = $"{ServerConstants.AuthorizationUser.LastName[indexOnFirstLetter]}{ServerConstants.AuthorizationUser.FirstName[indexOnFirstLetter]}";
        LongName.Text = $"{ServerConstants.AuthorizationUser.LastName} {ServerConstants.AuthorizationUser.FirstName}";
        Init();
    }

    private void Init()
    {
        NavigateTo(PartyButton, new PackagePage(Frame));
        PartyButton.ActivateButton();
    }
    
    private void ExitFromProfile(object? sender, RoutedEventArgs e)
    {
        AuthWindow authWindow = new AuthWindow();
        authWindow.Show();
        Close();
    }
    private void NavigateToPackagesPage(object? sender, RoutedEventArgs e)
    {
        var button = sender as MenuButton;
        NavigateTo(button, new PackagePage(Frame));
    }
    
    private void NavigateToMaterialsPage(object? sender, RoutedEventArgs e)
    {
        var button = sender as MenuButton;
        NavigateTo(button, new MaterialsPage(Frame));
    }
    
    private void NavigateToModelsPage(object? sender, RoutedEventArgs e)
    {
        var button = sender as MenuButton;
        NavigateTo(button, new ModelPage(Frame));
    }
    
    private void NavigateToOperationsPage(object? sender, RoutedEventArgs e)
    {
        var button = sender as MenuButton;
        NavigateTo(button, new OperationPage(Frame));
    }
    
    private void NavigateToPeoplePage(object? sender, RoutedEventArgs e)
    {
        var button = sender as MenuButton;
        NavigateTo(button, new PersonPage(Frame));
    }
    
    private void NavigateToSizesPage(object? sender, RoutedEventArgs e)
    {
        var button = sender as MenuButton;
        NavigateTo(button, new SizePage(Frame));
    }
    
    private void NavigateToHistoryPage(object? sender, RoutedEventArgs e)
    {
        var button = sender as MenuButton;
        NavigateTo(button, new HistoryPage());
    }

    private void NavigateTo(MenuButton button, UserControl page)
    {
        Frame.Content = page;
        SwitchThemeButton(button);
    }
    
    private void SwitchThemeButton(MenuButton button)
    {
        int index = NavPanel.Children.IndexOf(button);
        if (index == _currentIndexActive)
        {
            return;
        }
        var activeButton = (NavPanel.Children[_currentIndexActive] as MenuButton)!;
        button.ActivateButton();
        activeButton.DisableButton();
        _currentIndexActive = index;
    }
}