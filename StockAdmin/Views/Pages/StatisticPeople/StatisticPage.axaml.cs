using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalonia.Controls;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Records;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.StatisticPeople;

public partial class StatisticPage : UserControl
{
    private readonly List<PackageEntity> _packages;
    private readonly List<ClothOperationPersonEntity> _clothOperationPersons;
    public StatisticPage(PersonEntity person)
    {
        InitializeComponent();
        var items = new List<string> { "Пачки", "Операции" };
        
        _packages = new List<PackageEntity>();
        _clothOperationPersons = new List<ClothOperationPersonEntity>();
        Text.Text = person.FullName;
        GroupItems.ItemsSource = items;
        GroupItems.SelectedIndex = 1;
        InitAsync(person.Id);
    }

    private async void InitAsync(int personId)
    {
        var repository = new ClothOperationPersonRepository();
        var packageRepository = new PackageRepository();
        var list = await repository.GetAllAsync(personId);
        double fullSum = 0;
        DateTime now = DateTime.Now;
        DateTime firstDay = new DateTime(now.Year, now.Month, 1);
        DateTime lastDay = new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1);
        foreach (var item in list)
        {
            if (item.DateStart >= firstDay && item.DateStart <= lastDay && item.IsEnded)
            {
                _clothOperationPersons.Add(item);
                fullSum += item.ClothOperation.Price.Number;
            }
        }
        var packageList = await packageRepository.GetAllAsync(personId);
        foreach (var item in packageList)
        {
            if (item.CreatedAt >= firstDay && item.CreatedAt <= lastDay)
            {
                _packages.Add(item);
            }
        }
        
        FullSum.Text = "Полная сумма: " + fullSum.ToString("F2");

        SetClothOperations();
    }

    private void SetPackages()
    {
        var entities = _packages.GroupBy(x => x.CreatedAt.ToShortDateString()).ToList();
        
        var series = new ColumnSeries<IGrouping<string, PackageEntity>>()
        {
            Values = entities,
            Mapping = (packageEntities, i) =>
            {
                var point = new Coordinate(i, packageEntities.Count());
                
                return point;
            },
        };
        
        Chart.Series = new [] {series};
        
        var list = entities.Select(entity => entity.Key).ToList();
        var xAxes = new List<Axis>
        {
            new()
            {
                Labels = list 
            }
        };
        Chart.XAxes = xAxes;
        series.ChartPointPointerDown += SeriesOnChartPointPointerDown;
    }



    private void SetClothOperations()
    {
        var entities = _clothOperationPersons.Where(x => x.IsEnded)
            .GroupBy(x => x.DateStart.ToShortDateString()).ToList();
        
        var series = new ColumnSeries<IGrouping<string, ClothOperationPersonEntity>>()
        {
            Values = entities,
            Mapping = (clothOperation, i) =>
            {
                var point = new Coordinate(i, clothOperation.Count());
                
                return point;
            },
        };
        
        Chart.Series = new [] {series};
        
        var list = entities.Select(entity => entity.Key).ToList();
        var xAxes = new List<Axis>
        {
            new()
            {
                Labels = list 
            }
        };
        Chart.XAxes = xAxes;
        series.ChartPointPointerDown += SeriesOnChartPointPointerDown;
    }
    
    private void SeriesOnChartPointPointerDown(IChartView chart, ChartPoint<IGrouping<string, ClothOperationPersonEntity>, RoundedRectangleGeometry, LabelGeometry>? point)
    {
        Title.Text = point.Model.Key;
        
        double sum = 0;
        
        var walletOperations =  new List<WalletOperation>();
        foreach (var entity in _clothOperationPersons)
        {
            walletOperations.Add(new WalletOperation()
            {
                Name = entity.ClothOperation.Operation.Name,
                Cost = entity.ClothOperation.Price.Number
            });
            sum += entity.ClothOperation.Price.Number;
        }

        Result.Text = sum.ToString("F2") + " Р.";
        List.ItemsSource = walletOperations;
        
    }
    
    private void SeriesOnChartPointPointerDown(IChartView chart, ChartPoint<IGrouping<string, PackageEntity>, RoundedRectangleGeometry, LabelGeometry>? point)
    {
        Title.Text = point.Model.Key;
        double sum = 0;
        var walletOperations =  new List<WalletOperation>();
        foreach (var entity in point.Model)
        {
            foreach (var item in entity.ClothOperations)
            {
                walletOperations.Add(new WalletOperation {Name = item.Operation.Name, Cost = item.Price.Number});
                sum += item.Price.Number;
            }
        }
        Result.Text = sum.ToString("F2") + " Р.";
        List.ItemsSource = walletOperations;
    }

    public override string ToString()
    {
        return PageTitles.Statistic;
    }

    private void GroupItems_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
         string item = GroupItems.SelectedItem as string;
         switch (item)
         {
             case "Пачки":
                 SetPackages();
                 break;
             case "Операции":
                 SetClothOperations();
                 break;
         }

         Title.Text = "";
         List.ItemsSource = null;
         Result.Text = "";
    }
}