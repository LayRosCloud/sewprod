using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace StockAdmin.Views.UserControllers;

public partial class DeletedBorder : UserControl
{
    private const string DefaultTitle = "Вы уверены?";
    public DeletedBorder()
    {
        Title = DefaultTitle;
        InitializeComponent();
    }

    public event EventHandler<RoutedEventArgs>? ClickOnAnswerYes;
    public event EventHandler<RoutedEventArgs>? ClickOnAnswerNo;

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<DeletedBorder, string>(nameof(Text));
    
    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<DeletedBorder, string>(nameof(Title));

    private void SendYesOnAnswerDelete(object? sender, RoutedEventArgs e)
    {
        ClickOnAnswerYes?.Invoke(this, e);
        IsVisible = false;
    }
    
    private void SendNoOnAnswerDelete(object? sender, RoutedEventArgs e)
    {
        ClickOnAnswerNo?.Invoke(this, e);
        IsVisible = false;
    }
}