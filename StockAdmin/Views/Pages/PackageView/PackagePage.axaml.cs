using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Views.Pages.PartyView;

namespace StockAdmin.Views.Pages.PackageView;

public partial class PackagePage : UserControl
{
    private readonly ContentControl _frame;
    private readonly Party _party;
    public PackagePage(ContentControl frame, Party party)
    {
        InitializeComponent();
        _frame = frame;
        _party = party;
        Init();
    }

    private async void Init()
    {
        PackageRepository repository = new PackageRepository();
        List.ItemsSource = await repository.GetAllAsync(_party.id);
    }
    
    private void NavigateToAddedPackagesPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new AddedPackagesPage(_party, _frame);
    }
    
    private void LoadRowPackage(object? sender, DataGridRowEventArgs e)
    {
        DataGridRow row = e.Row;
        if (row.DataContext is Package package)
        {
            bool isAdmin = package.person.posts.Contains(new Post(){name = "ADMIN"});
            row.Background = new SolidColorBrush(isAdmin ? Color.Parse("#427D9D") : Color.FromRgb(255, 255, 255));
            row.Foreground = new SolidColorBrush(isAdmin ? Color.FromRgb(255, 255, 255) : Color.FromRgb(0, 0, 0));
        }
    }
    
    private void NavigateToMoreInformation(object? sender, TappedEventArgs e)
    {
        if (List.SelectedItem is Package package)
        {
            var page = new ClothOperationView.ClothOperationPage(package, _frame);
            _frame.Content = page;
        }
    }
    
    public override string ToString()
    {
        return "Пачки";
    }
}