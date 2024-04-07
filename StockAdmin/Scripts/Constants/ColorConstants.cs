using Avalonia.Media;

namespace StockAdmin.Scripts.Constants;

public abstract class ColorConstants
{
    public static readonly SolidColorBrush Blue = new(Color.FromRgb(193, 221, 244));
    public static readonly SolidColorBrush Green = new(Color.FromRgb(149, 192, 160));
    public static readonly SolidColorBrush DarkBlue = new(Color.FromRgb(66, 125, 157));
    public static readonly SolidColorBrush Gray = new(Color.FromRgb(136, 136, 136));
    
    public static readonly Color Completed = Color.FromRgb(149, 192, 160);
    public static readonly Color Repeat = Color.FromRgb(244, 158, 49);
    public static readonly Color Updated = Color.FromRgb(255, 0, 150);
    public static readonly Color Admin =  Color.FromRgb(125, 181, 251);
    public static readonly Color Cutter =  Color.FromRgb(200, 200, 200);
}