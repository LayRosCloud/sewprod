using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace StockAdmin.Views.UserControllers;

public partial class MenuButton : UserControl
{
    public MenuButton()
    {
        InitializeComponent();
        DisableButton();
    }
    public String Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    
    public static readonly StyledProperty<String> TextProperty =
        AvaloniaProperty.Register<MenuButton, String>(nameof(Text));
    
    public IImage? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }
    
    public static readonly StyledProperty<IImage?> SourceProperty =
        AvaloniaProperty.Register<MenuButton, IImage?>(nameof(Source));

    public void ActivateButton()
    {
        var color = new SolidColorBrush(Color.Parse("#427D9D"));
        LineSelected.Width = 5;
        BorderImage.Background = color;
        ButtonText.Foreground = color;
    }

    public void DisableButton()
    {
        var color = new SolidColorBrush(Color.FromRgb(136, 136, 136));
        LineSelected.Width = 0;
        BorderImage.Background = color;
        ButtonText.Foreground = color;
    }
    public EventHandler<RoutedEventArgs> OnClick { get; set; }
    private void ButtonClick(object? sender, RoutedEventArgs e)
    {
        OnClick?.Invoke(this, e);
    }
}