using System;
using System.Collections.Generic;
using Avalonia.Controls;
using LiveChartsCore;
using LiveChartsCore.Kernel;
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
        
        Chart.Series = new []{Series};
        
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

            var series = new ColumnSeries<Package>()
            {
                Values = new[]
                {
                    new Package() { createdAt = new DateTime(2010, 10, 10), personId = 12 },
                    new Package() { createdAt = new DateTime(2010, 10, 11), personId = 13 },
                    new Package() { createdAt = new DateTime(2010, 10, 12), personId = 14 },
                },
                Mapping = (package, i) =>
                {
                    Coordinate point = new Coordinate(i, package.personId);
                    
                    return point;
                },
            };
            return series;
        }
    }

    public override string ToString()
    {
        return "Статистика";
    }
}