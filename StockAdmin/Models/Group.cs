using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StockAdmin.Models;

public class Group<TSource> : IGrouping<string, TSource>
{
    public Group(string key, List<TSource> entities)
    {
        Key = key;
        Entities = entities;
    }
    
    public string Key { get; }
    public List<TSource> Entities { get; set; }
    
    public IEnumerator<TSource> GetEnumerator()
    {
        return Entities.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }


}