using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Exceptions;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Views;

public partial class AuthWindow : Window
{
    public AuthWindow()
    {
        InitializeComponent();
        
        InitData();
    }
    
    private async void InitData()
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
            ShowWindow();
        }
        catch (AuthException ex)
        {
            SendErrorMessage(ex.Message);
        }
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
        
        Person person = new Person { email = email, password = password };

        PersonRepository repository = new PersonRepository();
        var authPerson = await repository.LoginAsync(person);
        
        if (authPerson!.token == null)
        {
            throw new AuthException(authMessageError);
        }

        ServerConstants.Token = authPerson;
    }

    private void ShowWindow()
    {
        MainContainer container = new MainContainer();
        container.Show();
        Close();
    }

    private void DisableErrorMessage(object? sender, RoutedEventArgs e)
    {
        const bool disableBorderError = false;
        
        BorderError.IsVisible = disableBorderError;
    }
}