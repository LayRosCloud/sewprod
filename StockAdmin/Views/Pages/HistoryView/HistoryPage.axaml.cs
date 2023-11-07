using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Utils;
using Avalonia.Threading;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.HistoryView;

public partial class HistoryPage : UserControl
{
    private readonly List<History> _histories; 
    DispatcherTimer _timer = new DispatcherTimer();
    public HistoryPage()
    {
        InitializeComponent();
        _histories = new List<History>();
        _timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
        _timer.Tick += StartFind;
        Init();
    }

    private void StartFind(object? sender, EventArgs e)
    {
        string findText = Finder.Text!.Trim().ToLower();
        List.ItemsSource = _histories.Where(history => history.person.lastName.ToLower().Contains(findText) 
                                                       || history.person.uid.ToLower().Contains(findText)
                                                       || history.person.firstName.ToLower().Contains(findText));
        _timer.Stop();
    }

    private async void Init()
    {
        HistoryRepository repository = new HistoryRepository();
        _histories.AddRange(await repository.GetAllAsync());
        List.ItemsSource = _histories;
    }

    private void FindOnUserName(object? sender, TextChangedEventArgs e)
    {
        if (_timer.IsEnabled)
        {
            _timer.Stop();
        }
        
        _timer.Start();
    }

    public override string ToString()
    {
        return "История действий";
    }
}