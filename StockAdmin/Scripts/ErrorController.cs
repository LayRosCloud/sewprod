using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace StockAdmin.Scripts;

public class ErrorController
{
    private readonly Panel _container;

    public ErrorController(Panel container)
    {
        _container = container;
    }

    public void AddErrorMessage(string message)
    {
        Border border = CreateBorder();
        
        StackPanel stackPanel = CreateStackPanel();
        stackPanel.Children.Add(CreateTextBlock(message));
        stackPanel.Children.Add(CreateButton());
        border.Child = stackPanel;
        _container.Children.Add(border);
    }

    private Border CreateBorder()
    {
        var border = new Border();
        border.Background = new SolidColorBrush(Colors.DarkRed);
        border.Padding = new Thickness(5, 0);
        border.Margin = new Thickness(0, 5, 0, 0);
        border.CornerRadius = new CornerRadius(10);
        return border;
    }
    
    private StackPanel CreateStackPanel()
    {
        var stackPanel = new StackPanel();
        stackPanel.Orientation = Orientation.Horizontal;
        return stackPanel;
    }
    
    private TextBlock CreateTextBlock(string text)
    {
        var textBlock = new TextBlock();
        textBlock.Foreground = new SolidColorBrush(Colors.Azure);
        textBlock.VerticalAlignment = VerticalAlignment.Center;
        textBlock.Text = text;
        textBlock.TextWrapping = TextWrapping.Wrap;
        textBlock.Width = 150;
        return textBlock;
    }

    private Button CreateButton()
    {
        var button = new Button();
        button.Content = "x";
        button.Click += (sender, args) =>
        {
            Border border = ((sender as Button).Parent as StackPanel).Parent as Border;
            _container.Children.Remove(border);
        };
        return button;
    }
}