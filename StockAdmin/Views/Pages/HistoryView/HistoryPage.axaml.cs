using System;
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
    public HistoryPage()
    {
        InitializeComponent();
        _histories = new List<HistoryEntity>();
        _finderController = new FinderController(500, () =>
        {
            string findText = Finder.Text!.Trim().ToLower();
            List.ItemsSource = _histories.Where(history => history.Person.LastName.ToLower().Contains(findText) 
                                                           || history.Person.Uid.ToLower().Contains(findText)
                                                           || history.Person.FirstName.ToLower().Contains(findText));
        });
        Init();
    }

    private async void Init()
    {
        HistoryRepository repository = new HistoryRepository();
        _histories.AddRange(await repository.GetAllAsync());
        List.ItemsSource = _histories;
        LoadingBorder.IsVisible = false;
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
            switch (history.Action.Name.ToLower())
            {
                case "чтение":
                    row.Background = new SolidColorBrush(Color.FromRgb(149, 192, 160));
                    break;
                case "добавление":
                    row.Background = new SolidColorBrush(Color.FromRgb(225, 185, 0));
                    break;
                case "редактирование":
                    row.Background = new SolidColorBrush(Color.FromRgb(163, 202, 255));
                    break;
                case "удаление":
                    row.Background = new SolidColorBrush(Color.FromRgb(225, 193, 193));
                    break;
            }
        }
    }
}