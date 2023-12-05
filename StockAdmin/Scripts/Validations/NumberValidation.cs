using System;
using System.Collections;
using Avalonia.Input;

namespace StockAdmin.Scripts.Validations;

public class NumberValidation
{
    private readonly Hashtable _hashtable = new();

    public NumberValidation()
    {
        _hashtable.Add(Validation.Number, false);
        _hashtable.Add(Validation.Point, false);
    }
    private enum Validation
    {
        Number = 0,
        Point
    }
    
    public NumberValidation AddNumberValidation()
    {
        _hashtable[Validation.Number] = true;
        return this;
    }
    
    public NumberValidation AddPointValidation()
    {
        _hashtable[Validation.Point] = true;
        return this;
    }

    public bool Validate(Key key)
    {
        bool numberCheckout = (bool)(_hashtable[Validation.Number] ?? false);
        bool pointCheckout = (bool)(_hashtable[Validation.Point] ?? false);

        if (numberCheckout && pointCheckout)
        {
            return CheckOnPressedKeyNumber(key) && CheckOnPressedKeyPoint(key);
        }
        
        if (numberCheckout)
        {
            return CheckOnPressedKeyNumber(key);
        }
        
        if (pointCheckout)
        {
            return CheckOnPressedKeyPoint(key);
        }

        return true;
    }

    private bool CheckOnPressedKeyPoint(Key key)
    {
        return key != Key.Oem2
               && key != Key.OemComma
               && key != Key.OemPeriod;
    }

    private bool CheckOnPressedKeyNumber(Key key)
    {
        return key < Key.D0 || key > Key.D9 && key < Key.NumPad0 || key > Key.NumPad9;
    }
}