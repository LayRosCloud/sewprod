using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Views.Pages;

namespace StockAdmin.Views;

public partial class MainContainer : Window
{
    public MainContainer()
    {
        InitializeComponent();
        Frame.Content = new PartyPage(Frame);
    }

    private void NavigateToPartyPage(object? sender, RoutedEventArgs e)
    {
        Frame.Content = new PartyPage(Frame);
    }

    private void ExitFromProfile(object? sender, RoutedEventArgs e)
    {
        AuthWindow authWindow = new AuthWindow();
        authWindow.Show();
        Close();
    }

    private void NavigateToModelPage(object? sender, RoutedEventArgs e)
    {
        Frame.Content = new ModelPage(Frame);
    }

    private void NavigateToOperationPage(object? sender, RoutedEventArgs e)
    {
        Frame.Content = new OperationPage();
    }
}