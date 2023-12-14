using System;
using System.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Controllers;
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
    private readonly Hashtable _hashtable;
    
    public MainContainer()
    {
        InitializeComponent();
        ElementConstants.MainContainer = this;
        ElementConstants.ErrorController = new ErrorController(ErrorContainer);
        _hashtable = new Hashtable();

        const int indexOnFirstLetter = 0;
        
        ShortName.Text = $"{ServerConstants.AuthorizationUser.LastName[indexOnFirstLetter]}{ServerConstants.AuthorizationUser.FirstName[indexOnFirstLetter]}";
        LongName.Text = $"{ServerConstants.AuthorizationUser.LastName} {ServerConstants.AuthorizationUser.FirstName}";
        InitPages();
        Init();
    }

    private void InitPages()
    {
        _hashtable.Add("крои", new PackagePage(Frame));
        _hashtable.Add("модели", new ModelPage(Frame));
        _hashtable.Add("персонал", new PersonPage(Frame));
        _hashtable.Add("операции", new OperationPage(Frame));
        _hashtable.Add("размеры", new SizePage(Frame));
        _hashtable.Add("материалы", new MaterialsPage(Frame));
        _hashtable.Add("история", new HistoryPage());
    }
    
    private void Init()
    {
        NavigateTo(PartyButton, null);
        PartyButton.ActivateButton();
    }
    
    private void ExitFromProfile(object? sender, RoutedEventArgs e)
    {
        AuthWindow authWindow = new AuthWindow();
        authWindow.Show();
        Close();
    }
    
    
    private void NavigateTo(object? sender, RoutedEventArgs e)
    {
        var button = sender as MenuButton;
        Frame.Content = _hashtable[button!.Text.ToLower().Trim()];
        SwitchThemeButton(button);
    }

    private void SwitchThemeButton(MenuButton button)
    {
        int index = NavPanel.Children.IndexOf(button);
        if (index == _currentIndexActive)
        {
            return;
        }
        MenuButton buttonActive = NavPanel.Children[_currentIndexActive] as MenuButton;
        button.ActivateButton();
        buttonActive.DisableButton();
        _currentIndexActive = index;
    }
}