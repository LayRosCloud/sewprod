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
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.StatisticPeople;

public partial class StatisticPage : UserControl
{
    private readonly List<PackageEntity> _packages;
    private readonly List<ClothOperationPersonEntity> _clothOperations;
    public StatisticPage(PersonEntity person)
    {
        InitializeComponent();
        var items = new List<string> { "Пачки", "Операции" };
        
        _packages = new List<PackageEntity>();
        _clothOperations = new List<ClothOperationPersonEntity>();
        Text.Text = person.FullName;
        GroupItems.ItemsSource = items;
        GroupItems.SelectedIndex = 1;
        InitAsync(person.Id);
    }

    private async void InitAsync(int personId)
    {
        var repository = new ClothOperationPersonRepository();
        var packageRepository = new PackageRepository();
        
        _clothOperations.AddRange(await repository.GetAllAsync(personId));
        _packages.AddRange(await packageRepository.GetAllAsync(DateTime.Now.Month, personId));

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
        var entities = _clothOperations.Where(x => x.IsEnded).GroupBy(x => x.DateStart.ToShortDateString()).ToList();
        
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
        var builder = new StringBuilder();
        double sum = 0;
        foreach (var entity in point.Model)
        {
            builder.Append(entity.ClothOperation.Operation.Name + " ");
            builder.Append(entity.ClothOperation.Price.Number.ToString("F2"));
            builder.Append("\n");
            sum += entity.ClothOperation.Price.Number;
        }

        Result.Text = sum.ToString("F2") + " Р.";
        Description.Text = builder.ToString();
        
    }
    
    private void SeriesOnChartPointPointerDown(IChartView chart, ChartPoint<IGrouping<string, PackageEntity>, RoundedRectangleGeometry, LabelGeometry>? point)
    {
        Title.Text = point.Model.Key;
        var builder = new StringBuilder();
        double sum = 0;
        foreach (var entity in point.Model)
        {
            builder.Append(entity.Party!.CutNumber + "\t");
            builder.Append(entity.Party.Price!.Number.ToString("F2"));
            builder.Append("\n");
            sum += entity.Party.Price!.Number;
        }
        Result.Text = sum.ToString("F2") + " Р.";
        Description.Text = builder.ToString();
    }

    public override string ToString()
    {
        return "Статистика";
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
         Description.Text = "";
         Result.Text = "";
    }
}