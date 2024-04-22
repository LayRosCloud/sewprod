using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Avalonia;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using StockAdmin.Models;
using StockAdmin.Scripts.Extensions;
using StockAdmin.Scripts.Records;
using StockAdmin.Scripts.Statistic.Records;

namespace StockAdmin.Scripts.Statistic;

public abstract class Statistic<TSource>
{
    private readonly IEnumerable<TSource> _source;
    private readonly Action<List<WalletOperation>, double> _onGraphicClick;
    private readonly CartesianChart _chart;
    private readonly TextBlock _currentMonth;

    protected Statistic(StatisticOptions<TSource> options)
    {
        _source = options.Source;
        _chart = options.Chart;
        _onGraphicClick = options.OnGraphicClick;
        _currentMonth = options.CurrentFilterMonthText;
    }

    public void Generate()
    {
        (var entities, var sum) = GenerateArray();
        
        var series = CreateSeries(entities);
        _chart.Series = new []
        {
            series
        };
        
        var xAxes = CreateLabels(entities);
        
        _chart.XAxes = xAxes;
        _currentMonth.Text = sum.ToString();
        series.ChartPointPointerDown += SeriesOnChartPointPointerDown;
    }

    private void SeriesOnChartPointPointerDown(IChartView chart, ChartPoint<IGrouping<string, TSource>, RoundedRectangleGeometry, LabelGeometry>? point)
    {
        double sum = 0;
        
        var walletOperations =  new List<WalletOperation>();
        foreach (TSource entity in point.Model)
        {
            WalletOperation walletOperation = ConvertToOperation(entity);
            walletOperations.Add(walletOperation);

            sum += walletOperation.Cost;
        }
        _onGraphicClick.Invoke(walletOperations, sum);
    }


    private (List<Group<TSource>>, double) GenerateArray()
    {
        var list = new List<Group<TSource>>();
        var date = DateTime.Now;
        
        (DateTime first, DateTime last) = date.GetTwoDates();
        double fullSum = 0;
        while (first <= last)
        {
            var group = new Group<TSource>(first.ToShortDateString(), new List<TSource>());

            foreach (TSource item in _source)
            {
                if (FilterByDate(first, item))
                {
                    group.Entities.Add(item);
                    fullSum += GetPrice(item);
                }
            }
            list.Add(group);
            
            first = first.AddDays(1);
        }
        
        return (list, fullSum);
    }
    
    private List<Axis> CreateLabels(IEnumerable<IGrouping<string, TSource>> entities)
    {
        var list = entities.Select(entity => entity.Key).ToList();
        
        var xAxes = new List<Axis>
        {
            new()
            {
                Labels = list 
            }
        };
        
        return xAxes;
    }
    
    private ColumnSeries<IGrouping<string, TSource>> CreateSeries(IEnumerable<IGrouping<string, TSource>> list)
    {
        var series = new ColumnSeries<IGrouping<string, TSource>>()
        {
            Values = list,
            Mapping = (items, index) =>
            {
                var point = new Coordinate(index, items.Count());
                
                return point;
            },
        };
        
        return series;
    }

    protected abstract DateTime GetAttribute(TSource item);
    protected abstract bool FilterByDate(DateTime currentDate, TSource item);
    protected abstract WalletOperation ConvertToOperation(TSource item);
    protected abstract double GetPrice(TSource item);

}