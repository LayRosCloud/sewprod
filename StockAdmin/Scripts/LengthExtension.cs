using StockAdmin.Scripts.Exceptions;

namespace StockAdmin.Scripts;

public static class LengthExtension
{
    public static bool ContainLengthBetweenValues(this string field, LengthVector vector, string message)
    {
        
        if(field.Length <= vector.Minimum - 1 || field.Length >= vector.Maximum + 1)
        {
            throw new ValidationException(message);
        }

        return true;
    }
}