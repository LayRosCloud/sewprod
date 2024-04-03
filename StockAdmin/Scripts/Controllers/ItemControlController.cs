using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace StockAdmin.Scripts.Controllers;

public class ItemControlController
{
    
    public ComboBox CreateComboBox(ComboBox template)
    {
        var comboBox = new ComboBox()
        {
            DisplayMemberBinding = template.DisplayMemberBinding,
            SelectedValueBinding = template.SelectedValueBinding,
            SelectedValue = template.SelectedValue,
            SelectedItem = template.SelectedItem,
            ItemsSource = template.ItemsSource,
            HorizontalAlignment = template.HorizontalAlignment,
            Width = template.Width,
            Margin = template.Margin
        };
        
        return comboBox;
    }
    
    public Button CreateButton(Button template, EventHandler<RoutedEventArgs> onClickEvent)
    {
        var button = new Button()
        {
            HorizontalAlignment = template.HorizontalAlignment,
            Width = template.Width,
            Height = template.Height,
            Margin = template.Margin,
            Content = template.Content,
            Background = template.Background,
            Foreground = template.Foreground
        };
        button.Click += onClickEvent;
        
        return button;
    }
    
    public TextBox CreateTextBox(TextBox template)
    {
        var textBox = new TextBox()
        {
            Watermark = template.Watermark,
            Text = template.Text,
            Width = template.Width,
            Margin = template.Margin
        };
        return textBox;
    }
    
    public TextBox CreateTextBox(TextBox template, EventHandler<KeyEventArgs> keyDown)
    {
        var textBox = CreateTextBox(template);
        textBox.KeyDown += keyDown;
        return textBox;
    }
    
    public StackPanel CreateStackPanel(StackPanel template)
    {
        var stackPanel = new StackPanel()
        {
            Orientation = template.Orientation,
            Margin = template.Margin
        };
        return stackPanel;
    }
}