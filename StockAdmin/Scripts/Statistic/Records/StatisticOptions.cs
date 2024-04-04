using System;
using System.Collections.Generic;
using Avalonia.Controls;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Avalonia;
using StockAdmin.Scripts.Records;

namespace StockAdmin.Scripts.Statistic.Records;

public record StatisticOptions<TEntity>
{
    
    public CartesianChart Chart { get; set; }
    public IEnumerable<TEntity> Source { get; set; }
    public Action<List<WalletOperation>, double> OnGraphicClick { get; set; }
    public TextBlock CurrentFilterMonthText { get; set; }
}