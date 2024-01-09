using System;

namespace StockAdmin.Scripts.Extensions;

public static class DateExtension 
{
    public static (DateTime firstDay, DateTime lastDay) GetTwoDates(this DateTime date)
    {
        DateTime firstDay = new DateTime(date.Year, date.Month, 1);
        DateTime lastDay = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        return (firstDay, lastDay);
    }
    
}