using System;
using System.Collections.Generic;
using LiveChartsCore.SkiaSharpView.Avalonia;
using StockAdmin.Models;
using StockAdmin.Scripts.Records;

namespace StockAdmin.Scripts.Statistic;

public class PackagesStatistic : Statistic<PackageEntity>
{
    public PackagesStatistic(CartesianChart chart, IEnumerable<PackageEntity> source, Action<List<WalletOperation>, double> graphicClick
    ) : base(chart, source, graphicClick)
    {
    }

    protected override DateTime FilterOnAttribute(PackageEntity item)
    {
        return item.CreatedAt;
    }

    protected override bool FilterForAddedItem(DateTime currentDate, PackageEntity item)
    {
        DateTime date = new DateTime(item.CreatedAt.Year, item.CreatedAt.Month, item.CreatedAt.Day);
        return currentDate == date;
    }

    protected override WalletOperation CreateItem(PackageEntity item)
    {
        var walletOperation = new WalletOperation()
        {
            Name = item.Party?.CutNumber + " " + item.Size.FullName,
            Cost = item.Party?.Price?.Number ?? 0
        };
        
        return walletOperation;
    }
}