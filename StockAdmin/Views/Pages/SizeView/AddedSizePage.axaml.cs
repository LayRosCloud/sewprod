using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.SizeView;

public partial class AddedSizePage : UserControl
{
    private readonly ContentControl _frame;
    private readonly Size _size;
    
    public AddedSizePage(ContentControl frame)
        : this(frame, new Size()){}
    
    public AddedSizePage(ContentControl frame, Size size)
    {
        InitializeComponent();
        _frame = frame;
        _size = size;
        Init();
    }

    private async void Init()
    {
        AgeRepository repository = new AgeRepository();
        CbTypes.ItemsSource = await repository.GetAllAsync();
        DataContext = _size;
    }
    
    private async void SaveChanges(object? sender, RoutedEventArgs e)
    {
        SizeRepository sizeRepository = new SizeRepository();
        if (_size.id == 0)
        {
            await sizeRepository.CreateAsync(_size);
        }
        else
        {
            await sizeRepository.UpdateAsync(_size);
        }

        _frame.Content = new SizePage(_frame);
    }

    public override string ToString()
    {
        return "Добавление / Обновление размеров";
    }
}