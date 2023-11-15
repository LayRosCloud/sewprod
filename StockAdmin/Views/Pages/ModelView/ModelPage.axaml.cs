using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.ModelView;

public partial class ModelPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly List<Model> _models;
    private readonly FinderController _finderController;
    public ModelPage(ContentControl frame)
    {
        InitializeComponent();
        _frame = frame;
        _models = new List<Model>();
        _finderController = new FinderController(500, () =>
        {
            List.ItemsSource = _models
                .Where(x => x.title.ToLower().Contains(Finded.Text.ToLower()))
                .ToList();
        });
        InitData();
    }

    private async void InitData()
    {
        ModelRepository repository = new ModelRepository();
        _models.AddRange(await repository.GetAllAsync());
        List.ItemsSource = _models;
    }

    private void NavigateToCreatePage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedModelPage(_frame);
    }

    private void NavigateToEditPage(object? sender, RoutedEventArgs e)
    {
        Model model = (sender as Button).DataContext as Model;
        _frame.Content = new AddedModelPage(_frame, model);
    }
    
    private async void SendYesAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        var repository = new ModelRepository();
        
        if (List.SelectedItem is Model model)
        {
            await repository.DeleteAsync(model.id);
        }

        SendNoAnswerOnDeleteItem(sender, e);
        InitData();
    }

    private void SendNoAnswerOnDeleteItem(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = false;
    }

    private void ShowDeleteWindow(object? sender, RoutedEventArgs e)
    {
        DeletedContainer.IsVisible = true;
        DeletedMessage.Text =
            "вы действительно уверены, что хотите удалить модель?" +
            " Восстановить модель будет нельзя!";
    }
    
    public override string ToString()
    {
        return "Модели";
    }

    private void TextChanged(object? sender, TextChangedEventArgs e)
    {
        _finderController.ChangeText();
    }
}