using System;
using System.Collections.Generic;
using LiveChartsCore.SkiaSharpView.Avalonia;
using StockAdmin.Models;
using StockAdmin.Scripts.Records;

namespace StockAdmin.Scripts.Statistic;

public class PackagesStatistic : Statistic<PackageEntity>
{
    public PackagesStatistic(CartesianChart chart, IEnumerable<PackageEntity> source) : base(chart, source)
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
        var walletOperation = new WalletOperation();
        foreach (var clothOperation in item.ClothOperations)
        {
            double cost = clothOperation.Price?.Number ?? 0;
            walletOperation.Name += $"{clothOperation.Operation?.Name} {cost}\n";
            walletOperation.Cost += cost;
        }
        return walletOperation;
    }
}