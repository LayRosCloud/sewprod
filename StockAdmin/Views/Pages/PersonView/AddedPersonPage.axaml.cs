using System.Drawing.Imaging;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories;
using Zen.Barcode;
using Image = System.Drawing.Image;

namespace StockAdmin.Views.Pages.PersonView;

public partial class AddedPersonPage : UserControl
{
    private readonly Person _person;
    private readonly ContentControl _frame;

    public AddedPersonPage(ContentControl frame)
        : this(frame, new Person()) { }
    
    public AddedPersonPage(ContentControl frame, Person person)
    {
        InitializeComponent();
        _person = person;
        _frame = frame;
        DataContext = _person;
    }

    private async void SaveChanges(object? sender, RoutedEventArgs e)
    {
        PersonRepository personRepository = new PersonRepository();
        
        if (_person.id == 0)
        {
            await personRepository.CreateAsync(_person);
        }
        else
        {
            await personRepository.UpdateAsync(_person);
        }

        _frame.Content = new PersonPage(_frame);
    }
    
    public override string ToString()
    {
        return "Добавление / Обновление персонала";
    }

    private void SaveCode(object? sender, RoutedEventArgs e)
    {
        string exportJSON = "{\n\r" +
                            $"\"email\": \"{TbEmail.Text}\",\n\r" +
                            $"\"password\": \"{TbPassword.Text}\"\n\r" +
                            "}\n\r";
        CodeQrBarcodeDraw qrCode = BarcodeDrawFactory.CodeQr;
        Image image = qrCode.Draw(exportJSON, 100);
        System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)image;

        Bitmap avaloniaBitmap = CreateAvaloniaBitmap(bitmap);
        
        BarCodeImage.Source = avaloniaBitmap;

    }
    
    private Bitmap CreateAvaloniaBitmap(System.Drawing.Bitmap bitmap)
    {
        using var stream = new System.IO.MemoryStream();
        
        bitmap.Save(stream, ImageFormat.Png);

        // Сбросьте позицию потока на начало
        stream.Seek(0, System.IO.SeekOrigin.Begin);

        // Создайте Avalonia Bitmap из потока
        return new Bitmap(stream);
    }
}