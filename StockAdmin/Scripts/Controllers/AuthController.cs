using System;
using System.IO;
using System.Threading.Tasks;
using StockAdmin.Models;
using StockAdmin.Scripts.Exceptions;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Scripts.Controllers;

public class AuthController
{
    private readonly PersonAuth _auth;
    private const string FileName = "au_data.dat";
    public AuthController(string email, string password)
    {
        _auth = new PersonAuth
        {
            Email = email,
            Password = password
        };
    }

    public void SaveInConstants()
    {
        ServerConstants.Login = _auth.Email;
        ServerConstants.Password = _auth.Password;
    }
    
    public async Task CheckEmailAndPasswordAsync()
    {
        const string authMessageError = "Ошибка! Неверная почта или пароль!";
        
        var personEntity = new PersonEntity { Email = _auth.Email, Password = _auth.Password };

        var repository = new PersonRepository();
        var authPerson = await repository.LoginAsync(personEntity);
        
        if (authPerson == null || authPerson.Token == null)
        {
            throw new AuthException(authMessageError);
        }

        ServerConstants.AuthorizationUser = authPerson;
    }

    public async Task SaveEmailAndPasswordToFileAsync()
    {
        string textForFile = $"email: {_auth.Email}\r|\rpassword: {_auth.Password}";
        var controller = new CryptoController();
        string cryptText = controller.EncryptText(textForFile);

        await using var writer = new StreamWriter(FileName, false);
        await writer.WriteAsync(cryptText);
    }
    
    public static async Task<(string email, string password, bool successFul)> ReadFromFileEmailAndPasswordAsync()
    {
        try
        {
            var controller = new CryptoController();
            string sentence = await ReadTextFromFileAsync();
            string decoded = controller.DecodeAndDecrypt(sentence);

            (string email, string password) = ParseText(decoded);
            
            return (email, password, true);
        }
        catch(Exception)
        {
            return ("", "", false);
        }
    }

    private static async Task<string> ReadTextFromFileAsync()
    {
        using var reader = new StreamReader(FileName);
        string sentence = await reader.ReadToEndAsync();
        return sentence;
    }
    
    private static (string email, string password) ParseText(string decodedText)
    {
        const char symbolSeparatorForKeyAndValue = ':';
        const string textSeparatorForValues = "\r|\r";
        
        string[] keysValueEmailAndPassword = decodedText.Split(textSeparatorForValues);
        
        var email = keysValueEmailAndPassword[0]
            .Split(symbolSeparatorForKeyAndValue)[1]
            .Trim();
        
        var password = keysValueEmailAndPassword[1]
            .Split(symbolSeparatorForKeyAndValue)[1]
            .Trim();
        
        return (email, password);
    }
}