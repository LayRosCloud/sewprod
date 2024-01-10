using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class Entity
{
    [JsonPropertyName(ServerConstants.FieldId)]
    public int Id { get; set; }

    public override bool Equals(object? obj)
    {
        if(obj is Entity entity)
        {
            return entity.Id == Id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Id;
    }
}