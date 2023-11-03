using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages;

public partial class AddedPackagesPage : UserControl
{
    private readonly Party _party;
    private readonly ContentControl _frame;
    private readonly Hashtable _actionList;
    
    public AddedPackagesPage(Party party, ContentControl frame)
    {
        InitializeComponent();
        _party = party;
        _frame = frame;
        _actionList = new Hashtable();
        
        InitActionList();
        
        Init();
    }

    private void InitActionList()
    {
        _actionList.Add(Key.Enter, CreateNewTextBox);
        _actionList.Add(Key.Up, NavigateToUp);
        _actionList.Add(Key.Down, NavigateToDown);
    }

    private async void Init()
    {
        var repository = new SizeRepository();
        CmbSizes.ItemsSource = await repository.GetAllAsync();
    }
    
    private async void TrySaveElements(object? sender, RoutedEventArgs e)
    {
        PackageRepository packageRepository = new PackageRepository();

        try
        {
            var packagesList = ReadAllPackagesTextBox();
            await packageRepository.CreateAsync(packagesList);
            _frame.Content = new PartyPage(_frame);
        }
        catch (Exception)
        {
            // TODO: ignored
        }
    }

    private List<Package> ReadAllPackagesTextBox()
    {
        Size size = (CmbSizes.SelectedItem as Size)!;
        List<Package> packages = new List<Package>();

        foreach (Control control in MainPanel.Children)
        {
            if (control is TextBox textBox)
            {
                int count = Convert.ToInt32(textBox.Text);
                Package package = new Package()
                    { partyId = _party.id, count = count, sizeId = size.id };
                packages.Add(package);
            }
        }

        return packages;
    }

    private void InputSymbol(object? sender, KeyEventArgs e)
    {
        object? obj = _actionList[e.Key];
        
        if (obj is Action<object> action)
        {
            action.Invoke(sender!);
        }

        if (e.Key is (< Key.D0 or > Key.D9) and (< Key.NumPad0 or > Key.NumPad9))
        {
            e.Handled = true;
        }
    }

    private void CreateNewTextBox(object sender)
    {
        TextBox lastText = (MainPanel.Children[^1] as TextBox)!;
        
        var newTextBox = new TextBox
        {
            Watermark = NudCount.Watermark,
            Text = lastText.Text
        };

        newTextBox.KeyDown += InputSymbol;
        
        MainPanel.Children.Add(newTextBox);
        newTextBox.Focus();
        
        newTextBox.SelectionStart = newTextBox.Text!.Length;
    }

    private void NavigateToUp(object sender)
    {
        int index = GetIndexOfElement(sender);
        
        if (index > 0)
        {
            MainPanel.Children[index - 1].Focus();
        }
        else
        {
            MainPanel.Children[^1].Focus();
        }
    }
    
    private void NavigateToDown(object sender)
    {
        int index = GetIndexOfElement(sender);
        
        if (index < MainPanel.Children.Count - 1)
        {
            MainPanel.Children[index + 1].Focus();
        }
        else
        {
            MainPanel.Children[0].Focus();
        }
    }

    private int GetIndexOfElement(object sender)
    {
        TextBox textBox = (sender as TextBox)!;
        return MainPanel.Children.IndexOf(textBox);
    }
}