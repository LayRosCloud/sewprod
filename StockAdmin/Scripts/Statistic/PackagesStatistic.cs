using System;
using System.Collections.Generic;
using LiveChartsCore.SkiaSharpView.Avalonia;
using StockAdmin.Models;
using StockAdmin.Scripts.Records;
using StockAdmin.Scripts.Statistic.Records;

namespace StockAdmin.Scripts.Statistic;

public class PackagesStatistic : Statistic<PackageEntity>
{
    public PackagesStatistic(StatisticOptions<PackageEntity> options) : base(options)
    {
    }

    protected override DateTime GetAttribute(PackageEntity item)
    {
        return item.CreatedAt;
    }

    protected override bool FilterByDate(DateTime currentDate, PackageEntity item)
    {
        DateTime date = new DateTime(item.CreatedAt.Year, item.CreatedAt.Month, item.CreatedAt.Day);
        return currentDate == date;
    }

    protected override WalletOperation ConvertToOperation(PackageEntity item)
    {
        var walletOperation = new WalletOperation()
        {
            Name = item.Party?.CutNumber + " " + item.Size.FullName,
            Cost = GetPrice(item)
        };
        
        return walletOperation;
    }

    protected override double GetPrice(PackageEntity item)
    {
        return item.Party?.Price?.Number ?? 0;
    }
}