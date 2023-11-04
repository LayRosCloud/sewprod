using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace StockAdmin.Views.Pages.OperationView;

public partial class AddedOperationPage : UserControl
{
    public AddedOperationPage()
    {
        InitializeComponent();
    }

    public override string ToString()
    {
        return "Добавление / Обновление операции";
    }
}