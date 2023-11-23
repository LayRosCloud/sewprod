using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using StockAdmin.Models;

namespace StockAdmin.Views.Pages.StatisticPeople;

public partial class StatisticPage : UserControl
{
    public StatisticPage()
    {
        InitializeComponent();
        var items = new List<string>(){"Пачки", "Операции"};
        GroupItems.ItemsSource = items;
        Chart.Series = new [] {Series};
        
        List<Axis> xAxes = new List<Axis>
        {
            new Axis
            {
                Labels = new string[] { "2010-10-10",  "2010-10-11",  "2010-10-12" } 
            }
        };
        Chart.XAxes = xAxes;
    }

    public ISeries Series {
        get
        {

            var series = new ColumnSeries<PackageEntity>()
            {
                Values = new[]
                {
                    new PackageEntity() { CreatedAt = new DateTime(2010, 10, 10), PersonId = 12 },
                    new PackageEntity() { CreatedAt = new DateTime(2010, 10, 11), PersonId = 13 },
                    new PackageEntity() { CreatedAt = new DateTime(2010, 10, 12), PersonId = 14 },
                },
                Mapping = (package, i) =>
                {
                    Coordinate point = new Coordinate(i, package.PersonId);
                    
                    return point;
                },
            };
            series.ChartPointPointerDown += (chart, point) =>
            {
                Text.Text = point.Model.PersonId.ToString();
            };
            return series;
        }
    }

    public override string ToString()
    {
        return "Статистика";
    }



    private void Chart_OnDataPointerDown(IChartView chart, IEnumerable<ChartPoint> points)
    {
        
    }
}