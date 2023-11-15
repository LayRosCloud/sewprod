using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using StockAdmin.Scripts.Constants;
using StockAdmin.Views.Pages;
using StockAdmin.Views.Pages.HistoryView;
using StockAdmin.Views.Pages.MaterialView;
using StockAdmin.Views.Pages.PackageView;
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
        Init();
    }

    private async void Init()
    {
        try
        {
            var page = new PackagePage(Frame);
            Frame.Content = page;
        }
        catch (Exception)
        {
            ForbiddenContainer.IsVisible = true;
        }
    }

    private async void NavigateToPartyPage(object? sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        var page = new PackagePage(Frame);
        Frame.Content = page;
        
        SwitchThemeButton(button);
    }

    private void ExitFromProfile(object? sender, RoutedEventArgs e)
    {
        AuthWindow authWindow = new AuthWindow();
        authWindow.Show();
        Close();
    }

    private void NavigateToModelPage(object? sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        Frame.Content = new ModelPage(Frame);
        SwitchThemeButton(button);
    }

    private void NavigateToOperationPage(object? sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        Frame.Content = new OperationPage(Frame);
        SwitchThemeButton(button);
    }

    private void NavigateToSizePage(object? sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        Frame.Content = new SizePage(Frame);
        SwitchThemeButton(button);
    }

    private void NavigateToPersonalPage(object? sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        Frame.Content = new PersonPage(Frame);
        SwitchThemeButton(button);
    }

    private void SwitchThemeButton(Button button)
    {
        int index = NavPanel.Children.IndexOf(button);
        if (index == _currentIndexActive)
        {
            return;
        }
        Button buttonActive = NavPanel.Children[_currentIndexActive] as Button;
        StackPanel activePanel = buttonActive.Content as StackPanel;
        StackPanel panel = button.Content as StackPanel;
        
        (panel.Children[0] as Border).Width = 5;
        (panel.Children[1] as Border).Background = (activePanel.Children[1] as Border).Background;
        (panel.Children[2] as TextBlock).Foreground = (activePanel.Children[2] as TextBlock).Foreground;
        (activePanel.Children[0] as Border).Width = 0;
        (activePanel.Children[1] as Border).Background = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        (activePanel.Children[2] as TextBlock).Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        _currentIndexActive = index;
    }

    private void NavigateToHistoryPage(object? sender, RoutedEventArgs e)
    {
        Frame.Content = new HistoryPage();
        SwitchThemeButton(sender as Button);
    }

    private void NavigateToMaterialPage(object? sender, RoutedEventArgs e)
    {
        Frame.Content = new MaterialsPage(Frame);
        SwitchThemeButton(sender as Button);
    }
}