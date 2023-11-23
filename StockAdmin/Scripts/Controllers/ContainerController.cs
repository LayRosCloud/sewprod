using System.Collections.Generic;
using Avalonia.Controls;

namespace StockAdmin.Scripts.Controllers;

public class ContainerController
{
    private readonly Panel _panel;
    
    public ContainerController(Panel panel)
    {
        _panel = panel;
        Controls = new();
    }
    public List<Control> Controls { get; private set; }

    public void PushElementsToPanel()
    {
        foreach (var control in Controls)
        {
            _panel.Children.Add(control);
        }
    }

    public void AddPanelToParent(Panel panel, int index)
    {
        panel.Children.Insert(index, _panel);
    }
    
    public void AddPanelToParent(Panel panel)
    {
        panel.Children.Add(_panel);
    }
}