using System;
using Avalonia.Threading;

namespace StockAdmin.Scripts.Controllers;

public class FinderController
{
    private readonly Action _findEvent;
    private readonly DispatcherTimer _timer;
    
    public FinderController(int ticks, Action findEvent)
    {
        _findEvent = findEvent;
        
        _timer = new DispatcherTimer
        {
            Interval = new TimeSpan(0, 0, 0, 0, ticks)
        };
        
        _timer.Tick += TimerOnTick;
        
    }

    private void TimerOnTick(object? sender, EventArgs e)
    {
        _findEvent.Invoke();
        _timer.Stop();
    }

    public void ChangeText()
    {
        if (_timer.IsEnabled)
        {
            _timer.Stop();
        }
        
        _timer.Start();
    }
}