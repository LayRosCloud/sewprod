using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using StockAdmin.Models;
using StockAdmin.Scripts;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Exceptions;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.SizeView;

public partial class AddedTypeOfSizePage : UserControl
{
    private readonly ContentControl _frame;
    private readonly AgeEntity _ageEntity;
    public AddedTypeOfSizePage(ContentControl frame)
        :this(frame, new AgeEntity()){}
    
    public AddedTypeOfSizePage(ContentControl frame, AgeEntity ageEntity)
    {
        InitializeComponent();
        _frame = frame;
        _ageEntity = ageEntity;
        DataContext = _ageEntity;
    }

    private async void TrySaveChanges(object? sender, RoutedEventArgs e)
    {
        try
        {
            CheckFields();
            await SaveChanges();
            _frame.Content = new SizePage(_frame);
        }
        catch (ValidationException ex)
        {
            ElementConstants.ErrorController.AddErrorMessage(ex.Message);
        }

    }

    private async Task SaveChanges()
    {
        var ageRepository = new AgeRepository();
        if (_ageEntity.Id == 0)
        {
            await ageRepository.CreateAsync(_ageEntity);
        }
        else
        {
            await ageRepository.UpdateAsync(_ageEntity);
        }
    }
    
    private void CheckFields()
    {
        string name = TbName.Text!;
        string description = TbDesc.Text!;

        name.ContainLengthBetweenValues(new LengthVector(1, 30), "Название от 1 до 30 символов");
        description.ContainLengthBetweenValues(new LengthVector(1, 255), "Описание от 1 до 255 символов");
    }
    
    public override string ToString()
    {
        return "Добавление / Обновление типов размеров";
    }
}