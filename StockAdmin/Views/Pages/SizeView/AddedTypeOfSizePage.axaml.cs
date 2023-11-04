using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.SizeView;

public partial class AddedTypeOfSizePage : UserControl
{
    private readonly ContentControl _frame;
    private readonly Age _age;
    public AddedTypeOfSizePage(ContentControl frame)
        :this(frame, new Age()){}
    
    public AddedTypeOfSizePage(ContentControl frame, Age age)
    {
        InitializeComponent();
        _frame = frame;
        _age = age;
        DataContext = _age;
    }

    private async void SaveChanges(object? sender, RoutedEventArgs e)
    {
        AgeRepository ageRepository = new AgeRepository();
        if (_age.id == 0)
        {
            await ageRepository.CreateAsync(_age);
        }
        else
        {
            await ageRepository.UpdateAsync(_age);
        }

        _frame.Content = new SizePage(_frame);
    }

    public override string ToString()
    {
        return "Добавление / Обновление типов размеров";
    }
}