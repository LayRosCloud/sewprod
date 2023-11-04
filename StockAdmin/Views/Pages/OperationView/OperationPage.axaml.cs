using Avalonia.Controls;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;

namespace StockAdmin.Views.Pages.OperationView;

public partial class OperationPage : UserControl
{
    public OperationPage(ContentControl frame)
    {
        InitializeComponent();
        Init();
    }

    private async void Init()
    {
        IDataReader<Operation> operationRepository = new OperationRepository();
        List.ItemsSource = await operationRepository.GetAllAsync();
    }
    
    public override string ToString()
    {
        return "Операции";
    }
}