using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using StockAdmin.Models;
using StockAdmin.Scripts;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Repositories;
using Zen.Barcode;
using Image = System.Drawing.Image;

namespace StockAdmin.Views.Pages.PersonView;

public partial class AddedPersonPage : UserControl
{
    private readonly PersonEntity _personEntity;
    private readonly ContentControl _frame;

    public AddedPersonPage(ContentControl frame)
        : this(frame, new PersonEntity()) { }
    
    public AddedPersonPage(ContentControl frame, PersonEntity personEntity)
    {
        InitializeComponent();
        _personEntity = personEntity;
        _frame = frame;
        DataContext = _personEntity;
    }

    private async void SaveChanges(object? sender, RoutedEventArgs e)
    {
        PersonRepository personRepository = new PersonRepository();
        
        if (_personEntity.Id == 0)
        {
            PersonEntity personEntity = await personRepository.CreateAsync(_personEntity);
            var permissionRepository = new PermissionRepository();
            await permissionRepository.CreateAsync(new PermissionEntity(){PersonId = personEntity.Id, PostId = 3});
        }
        else
        {
            await personRepository.UpdateAsync(_personEntity);
        }

        _frame.Content = new PersonPage(_frame);
    }
    
    public override string ToString()
    {
        return "Добавление / Обновление персонала";
    }

    private void GenerateCode(object? sender, RoutedEventArgs e)
    {
        Bitmap avaloniaBitmap = CreateAvaloniaBitmap(CreateBitmap());
        
        BarCodeImage.Source = avaloniaBitmap;

        SaveButton.IsVisible = true;
    }

    private System.Drawing.Bitmap CreateBitmap()
    {
        CryptoController controller = new CryptoController();
        string exportJson = "{\n\r" +
                            $"\"email\": \"{TbEmail.Text}\",\n\r" +
                            $"\"password\": \"{controller.EncryptText(TbPassword.Text!)}\"\n\r" +
                            "}\n\r";
        
        CodeQrBarcodeDraw qrCode = BarcodeDrawFactory.CodeQr;
        Image image = qrCode.Draw(exportJson, 100);
        System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)image;
        return bitmap;
    }
    
   
    private Bitmap CreateAvaloniaBitmap(System.Drawing.Bitmap bitmap)
    {
        using var stream = new MemoryStream();
        
        bitmap.Save(stream, ImageFormat.Png);

        stream.Seek(0, SeekOrigin.Begin);

        return new Bitmap(stream);
    }

    [Obsolete("Method used on only Windows, because he is old")]
    private async void SaveCode(object? sender, RoutedEventArgs e)
    {
        string? path = await GetPathToSaveQrCode();

        System.Drawing.Bitmap bitmap = CreateBitmap();

        bitmap.Save(path, ImageFormat.Png);
    }

    [Obsolete("Method used on only Windows, because he is old")]
    private async Task<string?> GetPathToSaveQrCode()
    {
        var dialog = new SaveFileDialog();
        
        dialog.Filters = new List<FileDialogFilter>()
        {
            new()
            {
                
                Extensions = new List<string>(){"png"},
                Name = "PNG (*.png)"
            }
        };
        
        string? path = await dialog.ShowAsync(ElementConstants.MainContainer);

        return path;
    }
}