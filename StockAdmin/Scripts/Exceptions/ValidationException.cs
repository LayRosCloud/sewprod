using System;

namespace StockAdmin.Scripts.Exceptions;

public class ValidationException : Exception
{
    private const string DefaultExceptionMessage = "Ошибка валидации";
    
    public ValidationException(): this(DefaultExceptionMessage) { }
    
    public ValidationException(string message): base(message) { }
}