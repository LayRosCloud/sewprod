using System.Text.Json.Serialization;

namespace StockAdmin.Models;

public class ActionEntity : Entity
{
    [JsonPropertyName("name")] 
    public string Name { get; set; } = "";
}