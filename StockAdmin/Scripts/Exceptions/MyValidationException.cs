using System;

namespace StockAdmin.Scripts.Exceptions;

public class MyValidationException : Exception
{
    private const string DefaultExceptionMessage = "Ошибка валидации";
    
    public MyValidationException(): this(DefaultExceptionMessage) { }
    
    public MyValidationException(string message): base(message) { }
}