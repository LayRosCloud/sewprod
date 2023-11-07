using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;
using StockAdmin.Views.Pages.PartyView;
using Color = StockAdmin.Models.Color;

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

            row.Background = new SolidColorBrush(
                isAdmin 
                ? Avalonia.Media.Color.Parse("#427D9D") 
                : Avalonia.Media.Color.FromRgb(255, 255, 255));
            
            row.Foreground = new SolidColorBrush(
                isAdmin 
                ? Avalonia.Media.Color.FromRgb(255, 255, 255) 
                : Avalonia.Media.Color.FromRgb(0, 0, 0));
            if (package.isEnded)
            {
                row.Foreground = new SolidColorBrush(Colors.Black);
                row.Background = new SolidColorBrush(Colors.White);
            }
            else if (package.isRepeat)
            {
                row.Foreground = new SolidColorBrush(Colors.White);
                row.Background = new SolidColorBrush(Avalonia.Media.Color.Parse("#f49e31"));
                
            }
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

    private void NavigateToEditPage(object? sender, RoutedEventArgs e)
    {
        Package package = (sender as Button).DataContext as Package;
        _frame.Content = new EditPackagesPage(_frame, package, _party);
    }
}