using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Extensions;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Repositories.Server;
using StockAdmin.Scripts.Server;
using StockAdmin.Scripts.Vectors;

namespace StockAdmin.Views.Pages.SizeView;

public partial class AddedSizePage : UserControl
{
    private readonly ContentControl _frame;
    private readonly SizeEntity _sizeEntity;
    private readonly IRepositoryFactory _factory;
    
    public AddedSizePage(ContentControl frame)
        : this(frame, new SizeEntity()){}
    
    public AddedSizePage(ContentControl frame, SizeEntity sizeEntity)
    {
        InitializeComponent();
        _factory = ServerConstants.GetRepository();
        _frame = frame;
        _sizeEntity = sizeEntity;
        Init();
    }

    private async void Init()
    {
        var repository = _factory.CreateAgeRepository();
        CbTypes.ItemsSource = (await repository.GetAllAsync()).OrderBy(x=>x.Name);
        DataContext = _sizeEntity;
    }
    
    private async void TrySaveChanges(object? sender, RoutedEventArgs e)
    {
        try
        {
            CheckFields();
            await SaveChanges();
            
            _frame.Content = new SizePage(_frame);
        }
        catch (Scripts.Exceptions.ValidationException ex)
        {
            ElementConstants.ErrorController.AddErrorMessage(ex.Message);
        }
    }

    private async Task SaveChanges()
    {
        var sizeRepository = _factory.CreateSizeRepository();

        if (_sizeEntity.Id == 0)
        {
            await sizeRepository.CreateAsync(_sizeEntity);
        }
        else
        {
            await sizeRepository.UpdateAsync(_sizeEntity);
        }
    }

    private void CheckFields()
    {
        string sizeName = TbNumber.Text!;

        sizeName.ContainLengthBetweenValues(new LengthVector(1, 10), "Число от 1 до 10 символов");
        
        if (CbTypes.SelectedItem == null)
        {
            throw new ValidationException("Выберите растовку!");
        }
    }
    
    public override string ToString()
    {
        return PageTitles.AddSize;
    }
    
}