using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace StockAdmin.Views.UserControllers;

public partial class DeletedBorder : UserControl
{
    public DeletedBorder()
    {
        Title = "Вы уверены?";
        InitializeComponent();
    }

    public event EventHandler<RoutedEventArgs> ClickOnAnswerYes;
    public event EventHandler<RoutedEventArgs> ClickOnAnswerNo;
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
        DeletedContainer.IsVisible = false;
    }

    private void SendNoOnAnswerDelete(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = false;
        ClickOnAnswerNo?.Invoke(this, e);
    }
}