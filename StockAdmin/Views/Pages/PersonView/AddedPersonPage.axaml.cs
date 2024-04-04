﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using StockAdmin.Models;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Controllers;
using StockAdmin.Scripts.Extensions;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;
using StockAdmin.Scripts.Vectors;
using Zen.Barcode;

namespace StockAdmin.Views.Pages.PersonView;

public partial class AddedPersonPage : UserControl
{
    private readonly PersonEntity _personEntity;
    private readonly ContentControl _frame;
    private readonly IRepositoryFactory _factory;

    public AddedPersonPage(ContentControl frame)
        : this(frame, new PersonEntity())
    {
    }
    
    public AddedPersonPage(ContentControl frame, PersonEntity personEntity)
    {
        InitializeComponent();
        _factory = ServerConstants.GetRepository();
        
        _personEntity = personEntity;
        if (_personEntity.Id != 0)
        {
            RolesPanel.IsVisible = false;
            TitleRoles.IsVisible = false;
        }

        personEntity.Password = "";
        _frame = frame;
        DataContext = _personEntity;
        Initialize();
    }

    private async void Initialize()
    {
        var postRepository = _factory.CreatePostRepository();
        var list = await postRepository.GetAllAsync();
        CutterRole.ItemsSource = list;
    }
    
    private async void TrySaveChanges(object? sender, RoutedEventArgs e)
    {
        try
        {
            CheckFields();
            var list = GetPosts();
            await SaveChanges(list);
            _frame.Content = new PersonPage(_frame);
        }
        catch (Scripts.Exceptions.MyValidationException ex)
        {
            ElementConstants.ErrorController.AddErrorMessage(ex.Message);
        }
        catch (Exception ex)
        {
            ElementConstants.ErrorController.AddErrorMessage(Constants.UnexpectedAdminExceptionMessage);
        }
    }
    
    private async Task SaveChanges(List<PostEntity> posts)
    {
        var personRepository = _factory.CreatePersonRepository();

        if (_personEntity.Id == 0)
        {
            _personEntity.Posts = null;
            var person = await personRepository.CreateAsync(_personEntity);
            await SavePosts(posts, person);
        }
        else
        {
            await personRepository.UpdateAsync(_personEntity);
        }

    }

    private async Task SavePosts(List<PostEntity> posts, PersonEntity person)
    {
        var permissionRepository = _factory.CreatePermissionRepository();

        foreach (var post in posts)
        {
            var item = new PermissionEntity
            {
                PersonId = person.Id,
                PostId = post.Id
            };
            await permissionRepository.CreateAsync(item);
        }
        
    }

    private List<PostEntity> GetPosts()
    {
        List<PostEntity> posts = new List<PostEntity>();
        if (_personEntity.Id != 0)
        {
            return posts;
        }
        var countItems = RolesPanel.Children.Count - 1;
        const string exceptionValidationMessage = "Выберите роли!";
        const int firstItem = 0;
        
        for (int indexChild = firstItem; indexChild < countItems; indexChild++)
        {
            var comboBox = (RolesPanel.Children[indexChild] as StackPanel)!.Children[firstItem] as ComboBox;
            if (comboBox!.SelectedItem is PostEntity entity)
            {
                posts.Add(entity);
            }
            else
            {
                throw new ValidationException(exceptionValidationMessage);
            }
        }

        return posts;
    }

    private void CheckFields()
    {
        TbEmail.Text!.ContainLengthBetweenValues(new LengthVector(1, 50), "Почта от 1 до 50 символов");
        TbPassword.Text!.ContainLengthBetweenValues(new LengthVector(1, 30), "Пароль от 1 до 30 символов");
        TbLastName.Text!.ContainLengthBetweenValues(new LengthVector(1, 40), "Фамилия от 1 до 40 символов");
        TbFirstName.Text!.ContainLengthBetweenValues(new LengthVector(1, 40), "Имя от 1 до 40 символов");
        if (CdpBirthDay.SelectedDate == null)
        {
            throw new ValidationException("Выберите дату рождения!");
        }

        TbUid.Text!.ContainLengthBetweenValues(new LengthVector(1, 20), "Индентификатор от 1 до 20 символов");
    }
    
    public override string ToString()
    {
        return PageTitles.AddPerson;
    }

    private void GenerateCode(object? sender, RoutedEventArgs e)
    {
        try
        {
            var bitmap = CreateAvaloniaBitmap(CreateBitmap());

            BarCodeImage.Source = bitmap;

            SaveButton.IsVisible = true;
        }
        catch (Exception)
        {
            ElementConstants.ErrorController.AddErrorMessage("Ошибка! В вашем email или пароле содержатся русские символы");
        }
        
    }
    
    private Bitmap CreateAvaloniaBitmap(System.Drawing.Bitmap bitmap)
    {
        using var stream = new MemoryStream();
        
        bitmap.Save(stream, ImageFormat.Png);

        stream.Seek(0, SeekOrigin.Begin);

        return new Bitmap(stream);
    }

    private System.Drawing.Bitmap CreateBitmap()
    {
        var controller = new CryptoController();
        var exportJson = "{\n\r" +
                            $"\"email\": \"{TbEmail.Text}\",\n\r" +
                            $"\"password\": \"{controller.EncryptText(TbPassword.Text!)}\"\n\r" +
                            "}\n\r";
        
        var qrCode = BarcodeDrawFactory.CodeQr;
        var image = qrCode.Draw(exportJson, 100);
        var bitmap = (System.Drawing.Bitmap) image;
        return bitmap;
    }

    [Obsolete("Method used on only Windows, because he is old")]
    private async void SaveCode(object? sender, RoutedEventArgs e)
    {
        var path = await GetPathToSaveQrCode();
        
        if (path == null)
        {
            return;
        }
        
        var bitmap = CreateBitmap();

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

    private void AddNewPostField(object? sender, RoutedEventArgs e)
    {
        var parent = (sender as Button)?.Parent as StackPanel;
        var insidePanel = parent!.Children[^2] as StackPanel;
        
        var controller = new ItemControlController();
        var containerController = new ContainerController(controller.CreateStackPanel(insidePanel!))
        {
            Controls =
            {
                controller.CreateComboBox(insidePanel!.Children[0] as ComboBox),
                controller.CreateButton(insidePanel.Children[1] as Button, DeleteCurrentPostRole)
            }
        };
        
        containerController.PushElementsToPanel();
        
        containerController.AddPanelToParent(parent, parent.Children.Count - 1);
    }

    private void DeleteCurrentPostRole(object? sender, RoutedEventArgs e)
    {
        var currentStackPanel = (sender as Button)?.Parent as StackPanel;
        var parent = currentStackPanel!.Parent as StackPanel;
        if (parent?.Children.Count - 1 > 1)
        {
            parent.Children.Remove(currentStackPanel);
        }
    }

    private void CloseCurrentPage(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new PersonPage(_frame);
    }
}