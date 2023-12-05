using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Utils;
using Avalonia.Media;
using Avalonia.Threading;
using StockAdmin.Models;
using StockAdmin.Scripts;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Repositories;
using Color = Avalonia.Media.Color;

namespace StockAdmin.Views.Pages.HistoryView;

public partial class HistoryPage : UserControl
{
    private readonly List<HistoryEntity> _histories;
    private readonly FinderController _finderController;
    private readonly Hashtable _colors;
    public HistoryPage()
    {
        InitializeComponent();
        _histories = new List<HistoryEntity>();
        _finderController = new FinderController(500, FilteringArrayOnText);
        
        _colors = new Hashtable
        {
            { "чтение", new SolidColorBrush(Color.FromRgb(149, 192, 160)) },
            { "добавление", new SolidColorBrush(Color.FromRgb(225, 185, 0)) },
            { "редактирование", new SolidColorBrush(Color.FromRgb(163, 202, 255)) },
            { "удаление", new SolidColorBrush(Color.FromRgb(225, 193, 193)) }
        };
        
        Init();
    }

    private async void Init()
    {
        var dataController = new DataController<HistoryEntity>(new HistoryRepository(), _histories, List);
        var loadingController = new LoadingController<HistoryEntity>(LoadingBorder, dataController);
        await loadingController.FetchDataAsync();
    }

    private void FilteringArrayOnText()
    {
        string findText = Finder.Text!.Trim().ToLower();
        List.ItemsSource = _histories.Where(history => history.Person.LastName.ToLower().Contains(findText) 
                                                       || history.Person.Uid.ToLower().Contains(findText)
                                                       || history.Person.FirstName.ToLower().Contains(findText));
    }
    private void FindOnUserName(object? sender, TextChangedEventArgs e)
    {
        _finderController.ChangeText();
    }

    public override string ToString()
    {
        return "История действий";
    }

    private void LoadRowHistory(object? sender, DataGridRowEventArgs e)
    {
        var row = e.Row;
        if (row.DataContext is HistoryEntity history)
        {
            row.Background = (SolidColorBrush)_colors[history.Action.Name.ToLower()];
        }
    }
}