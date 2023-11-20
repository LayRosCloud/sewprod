using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class Entity
{
    [JsonPropertyName(ServerConstants.FieldId)]
    public int Id { get; set; }
}