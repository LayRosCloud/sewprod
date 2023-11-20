using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Views.Pages.PackageView;

public partial class AddedPackagesPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly Hashtable _actionList;
    
    public AddedPackagesPage(ContentControl frame)
    {
        InitializeComponent();
        _frame = frame;
        _actionList = new Hashtable();
        
        InitActionList();
        
        Init();
    }

    private void InitActionList()
    {
        _actionList.Add(Key.Enter, CreateNewElement);
        _actionList.Add(Key.Up, NavigateToUp);
        _actionList.Add(Key.Down, NavigateToDown);
        _actionList.Add(Key.Decimal, RemoveSelectedTextBox);
    }

    private async void Init()
    {
        var repository = new AgeRepository();
        var repositoryMaterials = new MaterialRepository();
        
        CmbAges.ItemsSource = await repository.GetAllAsync();
        CmbMaterials.ItemsSource = await repositoryMaterials.GetAllAsync();
        
    }
    
    private async void TrySaveElements(object? sender, RoutedEventArgs e)
    {
        PackageRepository packageRepository = new PackageRepository();

        try
        {
            var packagesList = ReadAllPackagesTextBox();
            await packageRepository.CreateAsync(packagesList);
            
            _frame.Content = new PackagePage(_frame);
        }
        catch (Exception)
        {
            // TODO: ignored
        }
    }

    private List<PackageEntity> ReadAllPackagesTextBox()
    {
        SizeEntity sizeEntity = (CmbSizes.SelectedItem as SizeEntity)!;
        List<PackageEntity> packages = new List<PackageEntity>();

        foreach (Control control in MainPanel.Children)
        {
            if (control is StackPanel stackPanel)
            {
                TextBox tbCount = (stackPanel.Children[0] as TextBox)!;
                ComboBox cbMaterials = (stackPanel.Children[1] as ComboBox)!;

                int count = Convert.ToInt32(tbCount.Text);
                MaterialEntity materialEntity = cbMaterials.SelectedItem as MaterialEntity;

                PackageEntity packageEntity = new PackageEntity()
                    { Count = count, SizeId = sizeEntity.Id, 
                        PersonId = ServerConstants.Token.Id, 
                        MaterialId = materialEntity!.Id, };
                packages.Add(packageEntity);
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

    private void RemoveSelectedTextBox(object sender)
    {
        if (MainPanel.Children.Count != 1 && sender is TextBox textBox)
        {
            MainPanel.Children.Remove(textBox.Parent as StackPanel);
            MainPanel.Children[^1].Focus();
        }
    }
    
    private void CreateNewElement(object sender)
    {
        StackPanel stack = (MainPanel.Children[^1] as StackPanel)!;
        StackPanel grid = new StackPanel()
        {
            Orientation = stack.Orientation
        };
        
        TextBox lastText = stack.Children[0] as TextBox;
        ComboBox cbComboMaterials = stack.Children[1] as ComboBox;

        var countTextBox = CreateCopyTextBox(lastText);
        var cbMaterial = CreateCopyComboBox(cbComboMaterials);
        
        countTextBox.KeyDown += InputSymbol;
        
        grid.Children.Add(countTextBox);
        grid.Children.Add(cbMaterial);
        
        MainPanel.Children.Add(grid);
        countTextBox.Focus();
        
        countTextBox.SelectionStart = countTextBox.Text!.Length;
    }

    private TextBox CreateCopyTextBox(TextBox template)
    {
        TextBox textBox = new TextBox()
        {
            Watermark = template.Watermark,
            Text = template.Text,
            Width = template.Width
        };
        return textBox;
    }
    
    private ComboBox CreateCopyComboBox(ComboBox template)
    {
        ComboBox comboBox = new ComboBox()
        {
            DisplayMemberBinding = template.DisplayMemberBinding,
            SelectedValueBinding = template.SelectedValueBinding,
            SelectedValue = template.SelectedValue,
            ItemsSource = template.ItemsSource,
            HorizontalAlignment = template.HorizontalAlignment,
            Width = template.Width
        };
        
        return comboBox;
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
    
    public override string ToString()
    {
        return "Добавление пачек";
    }

    private void CheckOnPartyAdded(object? sender, RoutedEventArgs e)
    {
        PanelSelectedItem.IsVisible = IsNewCut.IsChecked == false;
    }

    private async void SelectedTypeOfSize(object? sender, SelectionChangedEventArgs e)
    {
        if (CmbAges.SelectedItem is AgeEntity entity)
        {
            var repository = new SizeRepository();

            CmbSizes.IsVisible = true;
            CmbSizes.ItemsSource = await repository.GetAllAsync(entity.Id);
        }
        
    }
}