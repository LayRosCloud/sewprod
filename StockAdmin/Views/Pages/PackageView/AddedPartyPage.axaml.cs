using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Extensions;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Vectors;

namespace StockAdmin.Views.Pages.PackageView;

public partial class AddedPartyPage : UserControl
{
    private readonly ContentControl _frame;
    private readonly PartyEntity _party;
    public AddedPartyPage(ContentControl frame):this(frame, new PartyEntity())
    {
    }
    
    public AddedPartyPage(ContentControl frame, PartyEntity party)
    {
        InitializeComponent();
        _frame = frame;
        _party = party;
        Init();
    }

    private async void Init()
    {
        var repositoryPerson = new PersonRepository();
        var repositoryModel = new ModelRepository();
        var models = await repositoryModel.GetAllAsync();
        CbModels.ItemsSource = models;
        CbPersons.ItemsSource = await repositoryPerson.GetAllAsync();

        var item = models.FirstOrDefault(x => x.Id == _party.ModelId);
        if (item != null)
        {
            CbPrices.ItemsSource = item.Prices;
        }

        DataContext = _party;
    }

    private async void TrySaveChanges(object? sender, RoutedEventArgs e)
    {
        try
        {
            CheckFields();
            await SaveChanges();
            _frame.Content = new PackagePage(_frame);
        }
        catch (ValidationException ex)
        {
            ElementConstants.ErrorController.AddErrorMessage(ex.Message);
        }
    }

    private void CheckFields()
    {
        string cutNumber = TbCutNumber.Text!;
        cutNumber.ContainLengthBetweenValues(new LengthVector(1, 10), "Номер кроя должен быть от 1 до 10 символов");
        if (CbModels.SelectedItem == null)
        {
            throw new ValidationException("Выберите модель!");
        }
        if (CbPrices.SelectedItem == null)
        {
            throw new ValidationException("Выберите цену!");
        }
        if (CbPersons.SelectedItem == null)
        {
            throw new ValidationException("Выберите человека!");
        }

        if (CdpDateStart.SelectedDate == null)
        {
            throw new ValidationException("Введите дату начала!");
        }
    }
    
    private async Task SaveChanges()
    {
        var partyRepository = new PartyRepository();
        if (_party.Id == 0)
        {
            await partyRepository.CreateAsync(_party);
        }
        else
        {
            await partyRepository.UpdateAsync(_party);
        }
    }
    
    private void SelectedModel(object? sender, SelectionChangedEventArgs e)
    {
        if (CbModels.SelectedItem is not ModelEntity model)
        {
            return;
        }

        CbPrices.ItemsSource = model.Prices;
    }

    public override string ToString()
    {
        return PageTitles.AddParty;
    }
}