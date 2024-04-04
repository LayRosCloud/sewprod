using StockAdmin.Scripts.Exceptions;
using StockAdmin.Scripts.Vectors;

namespace StockAdmin.Scripts.Extensions;

public static class LengthExtension
{
    public static bool ContainLengthBetweenValues(this string field, LengthVector vector, string message)
    {
        
        if(field.Length <= vector.Minimum - 1 || field.Length >= vector.Maximum + 1)
        {
            throw new MyValidationException(message);
        }

        return true;
    }
}