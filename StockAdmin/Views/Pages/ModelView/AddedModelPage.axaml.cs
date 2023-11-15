using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.ModelView;

public partial class AddedModelPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly Model _model;
    private readonly List<Price> _prices;
    private readonly PriceRepository _priceRepository;

    public AddedModelPage(ContentControl frame) : this(frame, new Model())
    {
    }
    
    public AddedModelPage(ContentControl frame, Model model)
    {
        InitializeComponent();
        _frame = frame;
        _model = model;
        _prices = new List<Price>();
        _priceRepository = new PriceRepository();
        Init();
    }

    private async void Init()
    {
        _prices.AddRange(await _priceRepository.GetAllAsync());
        DataContext = _model;
    }
    
    private async void SaveChanges(object? sender, RoutedEventArgs e)
    {
        var repository = new ModelRepository();
        Price price = await _priceRepository.CreateAsync(new Price() { date = DateTime.Now, number = Convert.ToDouble(CbPrice.Text) });
        _model.priceId = price.id;
        
        if (_model.id == 0)
        {
            await repository.CreateAsync(_model);
        }
        else
        {
            await repository.UpdateAsync(_model);
        }

        _frame.Content = new ModelPage(_frame);
    }
    
    public override string ToString()
    {
        return "Добавление / Обновление моделей";
    }
}