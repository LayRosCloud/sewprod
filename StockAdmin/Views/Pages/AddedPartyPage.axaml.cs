using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Views.Pages;

public partial class AddedPartyPage : UserControl
{
    private readonly Party _party;
    private readonly ContentControl _frame;
    public AddedPartyPage(ContentControl frame): this(new Party(), frame)
    {
        
    }

    public AddedPartyPage(Party party, ContentControl frame)
    {
        InitializeComponent();
        _party = party;
        _party.personId = ServerConstants.Token.id;
        InitData();

        _frame = frame;
    }

    private async void InitData()
    {
        var modelRepository = new ModelRepository();
        List<Model> models = await modelRepository.GetAllAsync();
        CmbModel.ItemsSource = models;
        
        DataContext = _party;
    }

    private void SelectModel(object? sender, SelectionChangedEventArgs e)
    {
    }

    private async void SaveChanges(object? sender, RoutedEventArgs e)
    {
        PartyRepository repository = new PartyRepository();
        if (_party.id == 0)
        {
            await repository.CreateAsync(_party);
        }
        else
        {
            await repository.UpdateAsync(_party);
        }

        _frame.Content = new PartyPage(_frame);
    }
}