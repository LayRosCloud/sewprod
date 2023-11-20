using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class AgeEntity : Entity
{
    [JsonPropertyName(ServerConstants.Age.FieldName)]
    public string Name { get; set; } = "";

    [JsonPropertyName(ServerConstants.Age.FieldDescription)]
    public string Description { get; set; } = "";
}