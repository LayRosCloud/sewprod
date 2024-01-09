using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Enums;

namespace StockAdmin.Views.UserControllers;

public partial class MenuButton : UserControl
{
    private const int ActivatedWidth = 5;
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
        var color = ColorConstants.DarkBlue;
        LineSelected.Width = (int)MenuStates.Activate;
        BorderImage.Background = color;
        ButtonText.Foreground = color;
    }

    public void DisableButton()
    {
        var color = ColorConstants.Gray;
        LineSelected.Width = (int)MenuStates.Deactivate;
        BorderImage.Background = color;
        ButtonText.Foreground = color;
    }
    
    public EventHandler<RoutedEventArgs> OnClick { get; set; }
    private void ButtonClick(object? sender, RoutedEventArgs e)
    {
        OnClick?.Invoke(this, e);
    }
}