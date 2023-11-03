using Avalonia.Controls;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages;

public partial class OperationPage : UserControl
{
    public OperationPage()
    {
        InitializeComponent();
        Init();
    }

    private async void Init()
    {
        IDataReader<Operation> operationRepository = new OperationRepository();
        List.ItemsSource = await operationRepository.GetAllAsync();
    }
}