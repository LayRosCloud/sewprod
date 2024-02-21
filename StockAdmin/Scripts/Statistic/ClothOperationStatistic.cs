using System;
using System.Collections.Generic;
using LiveChartsCore.SkiaSharpView.Avalonia;
using StockAdmin.Models;
using StockAdmin.Scripts.Records;

namespace StockAdmin.Scripts.Statistic;

public class ClothOperationStatistic : Statistic<ClothOperationPersonEntity>
{
    
    public ClothOperationStatistic(CartesianChart chart, IEnumerable<ClothOperationPersonEntity> source, Action<List<WalletOperation>, double> graphicClick)
    :base(chart, source, graphicClick)
    { }


    protected override DateTime FilterOnAttribute(ClothOperationPersonEntity item)
    {
        return item.DateStart;
    }

    protected override bool FilterForAddedItem(DateTime currentDate, ClothOperationPersonEntity item)
    {
        DateTime date = new DateTime(item.DateStart.Year, item.DateStart.Month, item.DateStart.Day);
        return currentDate == date && item.IsEnded;
    }


    protected override WalletOperation CreateItem(ClothOperationPersonEntity item)
    {
        var walletOperation = new WalletOperation()
        {
            Name = item.ClothOperation?.Operation?.Name ?? "",
            Cost = item.ClothOperation?.Price?.Number ?? 0
        };
        return walletOperation;
    }

}