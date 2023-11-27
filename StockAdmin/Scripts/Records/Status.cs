using Avalonia.Media;

namespace StockAdmin.Scripts.Records;

public class Status
{
    private readonly string _name;
    private readonly SolidColorBrush _color;

    public Status(string name, Color color)
    {
        _name = name;
        _color = new SolidColorBrush(color);
    }

    public string Name => _name;
    public SolidColorBrush Color => _color;
}