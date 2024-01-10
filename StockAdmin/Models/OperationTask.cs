using Avalonia.Media;

namespace StockAdmin.Models;

public class OperationTask
{
    public string Name { get; set; } = "";
    public double Opacity { get; set; }
    public SolidColorBrush Foreground { get; set; }
    public FontWeight StyleFont { get; set; }

    public static OperationTask GetNotCompleted(string name)
    {
        return new OperationTask()
        {
            Name = name + "*",
            Opacity = 0.6,
            Foreground = new SolidColorBrush(Colors.DarkRed),
            StyleFont = FontWeight.Regular
        };
    }
    
    public static OperationTask GetCompleted(string name)
    {
        return new OperationTask()
        {
            Name = "✓ "+name,
            Opacity = 1,
            Foreground = new SolidColorBrush(Colors.Black),
            StyleFont = FontWeight.Bold
        };
    }
    
}