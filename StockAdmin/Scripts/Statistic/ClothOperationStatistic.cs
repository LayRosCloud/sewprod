using System;
using System.Collections.Generic;
using LiveChartsCore.SkiaSharpView.Avalonia;
using StockAdmin.Models;
using StockAdmin.Scripts.Records;
using StockAdmin.Scripts.Statistic.Records;

namespace StockAdmin.Scripts.Statistic;

public class ClothOperationStatistic : Statistic<ClothOperationPersonEntity>
{
    
    public ClothOperationStatistic(StatisticOptions<ClothOperationPersonEntity> options)
    :base(options)
    { }


    protected override DateTime GetAttribute(ClothOperationPersonEntity item)
    {
        return item.DateStart;
    }

    protected override bool FilterByDate(DateTime currentDate, ClothOperationPersonEntity item)
    {
        DateTime date = new DateTime(item.DateStart.Year, item.DateStart.Month, item.DateStart.Day);
        return currentDate == date && item.IsEnded;
    }


    protected override WalletOperation ConvertToOperation(ClothOperationPersonEntity item)
    {
        var walletOperation = new WalletOperation()
        {
            Name = item.ClothOperation?.Operation?.Name ?? "",
            Cost = GetPrice(item)
        };
        return walletOperation;
    }

    protected override double GetPrice(ClothOperationPersonEntity item)
    {
        return item.ClothOperation?.Price?.Number ?? 0;
    }
}