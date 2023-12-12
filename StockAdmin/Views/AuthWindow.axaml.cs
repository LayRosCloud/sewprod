using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Exceptions;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Views;

public partial class AuthWindow : Window
{
    private const string FileName = "au_data.dat";
    public AuthWindow()
    {
        InitializeComponent();
        ReadFile();
        InitData();
    }

    private async void ReadFile()
    {
        try
        {
            var controller = new CryptoController();
            using var reader = new StreamReader(FileName);
            string sentence = await reader.ReadToEndAsync();
            string decoded = controller.DecodeAndDecrypt(sentence);
            string[] keysValueEmailAndPassword = decoded.Split("\r|\r");
        
            var email = keysValueEmailAndPassword[0].Split(':')[1].Trim();
            var password = keysValueEmailAndPassword[1].Split(':')[1].Trim();
            
            IsRememberMe.IsChecked = true;
            Email.Text = email;
            Password.Text = password;
        }
        catch(Exception)
        {
            // ignored
        }
    }
    
    private async void InitData()
    {
        await InitLinks();
    }

    private async Task InitLinks()
    {
        const string networkException = "ошибка подключения...";
        const bool disableLogoBorder = false;
        
        var linkRepository = new LinkRepository();

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
        string email = Email.Text!.ToLower().Trim();
        string password = Password.Text!;

        try
        {
            await CheckEmailAndPassword(email, password);
            if (IsRememberMe.IsChecked == true)
            {
                SaveEmailAndPasswordToFile(email, password);
            }

            ShowWindow();
        }
        catch (AuthException ex)
        {
            SendErrorMessage(ex.Message);
        }
        catch (Exception)
        {
            SendErrorMessage("Неправильный логин или пароль!");
        }
    }

    private async void SaveEmailAndPasswordToFile(string email, string password)
    {
        var controller = new CryptoController();
        string cryptText = controller.EncryptText($"email: {email}\r|\rpassword: {password}");

        await using var writer = new StreamWriter(FileName, false);
        await writer.WriteAsync(cryptText);
    }

    private void SendErrorMessage(string message)
    {
        ErrorText.Text = message;
        BorderError.IsVisible = true;
    }
    
    private async Task CheckEmailAndPassword(string email, string password)
    {
        const string authMessageError = "Ошибка! Неверная почта или пароль!";
        
        ServerConstants.Login = email;
        ServerConstants.Password = password;
        
        var personEntity = new PersonEntity { Email = email, Password = password };

        var repository = new PersonRepository();
        var authPerson = await repository.LoginAsync(personEntity);
        
        if (authPerson!.Token == null)
        {
            throw new AuthException(authMessageError);
        }

        ServerConstants.AuthorizationUser = authPerson;
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