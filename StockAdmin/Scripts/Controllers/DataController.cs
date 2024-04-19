using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using StockAdmin.Scripts.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace StockAdmin.Scripts.Controllers;

public class DataController<TEntity>
{
    private readonly IDataReader<TEntity> _repository;
    private readonly List<TEntity> _entities;
    private readonly DataGrid _control;
    public DataController(
        [NotNull] IDataReader<TEntity> repository, 
        [NotNull] List<TEntity> entities,
        [NotNull] DataGrid control
    )
    {
        _repository = repository;
        _entities = entities;
        _control = control;
    }

    public async Task FetchDataAsync()
    {
        _entities.Clear();
        _entities.AddRange(await _repository.GetAllAsync());
        _control.ItemsSource = _entities;
        
        if (_entities.Count > 0) 
        {
            _control.SelectedIndex = 0;
        }
    }
}