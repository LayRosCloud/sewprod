using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Views.Pages.PersonView;

public partial class RolePage : UserControl
{
    private readonly PersonEntity _person;
    private readonly ContentControl _frame;
    private readonly IRepositoryFactory _factory;
    private readonly List<PostEntity> _permissions;
    private readonly List<PostEntity> _posts;
    public RolePage(ContentControl frame, PersonEntity person)
    {
        InitializeComponent();
        _person = person;
        _factory = ServerConstants.GetRepository();
        _permissions = new List<PostEntity>(person.Posts);
        _frame = frame;
        FullName.Text = _person.FullName;
        _posts = new List<PostEntity>();
        PersonPosts.ItemsSource = _person.Posts;
        Init();
    }

    private async void Init()
    {
        var repo = _factory.CreatePostRepository();
        _posts.AddRange(await repo.GetAllAsync());
        Posts.ItemsSource = _posts.Where(x => _permissions.FirstOrDefault(p => p.Id == x.Id) == null);
    }

    public override string ToString()
    {
        return "роли сотрудника " + _person.FullName;
    }

    private void DeleteRole(object? sender, RoutedEventArgs e)
    {
        if (PersonPosts.SelectedItem is not PostEntity post)
        {
            return;
        }
        
        var db = Scripts.Repositories.DataContext.Instance;
        var permission = db.permissions.FirstOrDefault(x => x.PersonId == _person.Id && x.PostId == post.Id);
        if (permission == null)
        {
            return;
        }

        db.permissions.Remove(permission);
        db.SaveChanges();
        _permissions.Remove(post);
        PersonPosts.ItemsSource = _permissions;
        PersonPosts.SelectedIndex = 0;
        Posts.ItemsSource = _posts.Where(x => _permissions.FirstOrDefault(p => p.Id == x.Id) == null);
    }

    private void Back(object? sender, RoutedEventArgs e)
    {
        _frame.Content = new PersonPage(_frame);
    }

    private async void AddPost(object? sender, RoutedEventArgs e)
    {
        if (Posts.SelectedItem is not PostEntity post)
        {
            return;
        }
        var repo = _factory.CreatePermissionRepository();
        var permission = new PermissionEntity
        {
            PersonId = _person.Id,
            PostId = post.Id
        };
        await repo.CreateAsync(permission);
        _permissions.Add(post);
        PersonPosts.ItemsSource = _permissions;
        PersonPosts.SelectedIndex = 0;
        Posts.ItemsSource = _posts.Where(x => _permissions.FirstOrDefault(p => p.Id == x.Id) == null);
    }
}