using System;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Exceptions;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Views;

public partial class AuthWindow : Window
{
    private readonly IRepositoryFactory _factory;
    public AuthWindow()
    {
        InitializeComponent();
        _factory = ServerConstants.GetRepository();

        ReadFile();

        InitData();
    }

    private async void ReadFile()
    {
        (string email, string password, bool isRememberMe) = await AuthController.ReadFromFileEmailAndPasswordAsync();
         
        Email.Text = email;
        Password.Text = password;
        IsRememberMe.IsChecked = isRememberMe;
    }
    
    private async void InitData()
    {
        await InitLinks();
    }

    private async Task InitLinks()
    {
        const string networkException = "ошибка подключения...";
        const bool disableLogoBorder = false;
        
        var linkRepository = _factory.CreateLinkRepository();

        try
        {
            await linkRepository.GetAllAsync();
            LogoBorder.IsVisible = disableLogoBorder;
        }
        catch (HttpRequestException)
        {
            LogoText.Text = networkException;
        }
    }
    
    private async void TryEnterToApplication(object? sender, RoutedEventArgs e)
    {
        const string wrongEmailOrPasswordExceptionMessage = "Неправильный логин или пароль!";
        
        string email = Email.Text!.ToLower().Trim();
        string password = Password.Text!;

        try
        {
            var authController = new AuthController(email, password);
            await authController.CheckEmailAndPasswordAsync();
            authController.SaveInConstants();
            
            if (IsRememberMe.IsChecked == true)
            {
                await authController.SaveEmailAndPasswordToFileAsync();
            }

            ShowWindow();
        }
        catch (AuthException ex)
        {
            SendErrorMessage(ex.Message);
        }
        catch (Exception)
        {
            SendErrorMessage(wrongEmailOrPasswordExceptionMessage);
        }
    }

    private void SendErrorMessage(string message)
    {
        ErrorText.Text = message;
        BorderError.IsVisible = true;
    }
    

    private void ShowWindow()
    {
        var container = new MainContainer();
        container.Show();
        Close();
    }

    private void DisableErrorMessage(object? sender, RoutedEventArgs e)
    {
        const bool disableBorderError = false;
        
        BorderError.IsVisible = disableBorderError;
    }

    private void DisableWindow(object? sender, RoutedEventArgs e)
    {
        ForbiddenContainer.IsVisible = false;
    }
}