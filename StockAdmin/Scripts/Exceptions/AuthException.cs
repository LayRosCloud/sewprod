using System;

namespace StockAdmin.Scripts.Exceptions;

public class AuthException : Exception
{
    private const string DefaultExceptionMessage = "Исключение авторизации";
    
    public AuthException(): this(DefaultExceptionMessage) { }
    
    public AuthException(string message): base(message) { }
}