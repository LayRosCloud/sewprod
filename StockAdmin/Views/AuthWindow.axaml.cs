using System;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
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
        InitializeData();
    }

    private async void InitializeData()
    {
        await FetchLinks();
        await ReadAuthorizationFieldsFromFile();
    }

    private async Task FetchLinks()
    {
        const string networkException = "ошибка подключения...";
        const bool disableLogoBorder = false;

        var linkRepository = _factory.CreateLinkRepository();

        try
        {
            await linkRepository.GetAllAsync();
            LogoBorder.IsVisible = disableLogoBorder;
        }
        catch (Exception)
        {
            LogoText.Text = networkException;
        }
    }

    private async Task ReadAuthorizationFieldsFromFile()
    {
        var (email, password, isRememberMe) = await AuthController.ReadFromFileEmailAndPasswordAsync();
         
        Email.Text = email;
        Password.Text = password;
        IsRememberMe.IsChecked = isRememberMe;
    }


    private async void TryEnterToApplication(object? sender, RoutedEventArgs e)
    {
        const string wrongEmailOrPasswordExceptionMessage = "Неправильный логин или пароль!";
        
        var email = Email.Text!.ToLower().Trim();
        var password = Password.Text!;

        try
        {
            var authController = new AuthController(email, password);
            await authController.CheckEmailAndPasswordAsync();
            authController.SaveInConstants();
            
            if (ServerConstants.AuthorizationUser.Posts != null && !ServerConstants.AuthorizationUser.Posts.Contains(new PostEntity { Id = 2 } ))
            {
                ForbiddenContainer.IsVisible = true;
                return;
            }
            
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