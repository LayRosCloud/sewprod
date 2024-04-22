using System;
using StockAdmin.Models;
using StockAdmin.Scripts.Records;
using StockAdmin.Scripts.Statistic.Records;

namespace StockAdmin.Scripts.Statistic;

public class PartyStatistic : Statistic<PartyEntity>
{
    public PartyStatistic(StatisticOptions<PartyEntity> options) : base(options)
    {
    }

    protected override DateTime GetAttribute(PartyEntity item)
    {
        return item.DateStart;
    }

    protected override bool FilterByDate(DateTime currentDate, PartyEntity item)
    {
        DateTime date = new DateTime(item.DateStart.Year, item.DateStart.Month, item.DateStart.Day);
        return currentDate == date;
    }

    protected override WalletOperation ConvertToOperation(PartyEntity item)
    {
        var walletOperation = new WalletOperation()
        {
            Name = item.CutNumber,
            Cost = GetPrice(item)
        };
        
        return walletOperation;
    }

    protected override double GetPrice(PartyEntity item)
    {
        return item.Price?.Number ?? 0;
    }
}