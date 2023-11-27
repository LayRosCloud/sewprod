using System.Collections.Generic;

namespace StockAdmin.Scripts.Records;

public class Counter<TEntity>
{
    private readonly string _name;

    private readonly List<TEntity> _entities;

    public Counter(string name, List<TEntity> entities)
    {
        _name = name;
        _entities = entities;
    }
    
    public string Name { get; set; }
    public List<TEntity> Entities { get; set; }
}
