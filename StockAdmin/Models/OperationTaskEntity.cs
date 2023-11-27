using Avalonia.Media;

namespace StockAdmin.Models;

public class OperationTaskEntity
{
    public string Name { get; set; } = "";
    public double Opacity { get; set; }
    public SolidColorBrush Foreground { get; set; }
    public FontWeight StyleFont { get; set; }

    public static OperationTaskEntity GetNotCompleted(string name)
    {
        return new OperationTaskEntity()
        {
            Name = name + "*",
            Opacity = 0.6,
            Foreground = new SolidColorBrush(Colors.DarkRed),
            StyleFont = FontWeight.Regular
        };
    }
    
    public static OperationTaskEntity GetCompleted(string name)
    {
        return new OperationTaskEntity()
        {
            Name = "✓ "+name,
            Opacity = 1,
            Foreground = new SolidColorBrush(Colors.Black),
            StyleFont = FontWeight.Bold
        };
    }
    
}