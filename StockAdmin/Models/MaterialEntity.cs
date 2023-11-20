using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class MaterialEntity : Entity
{
    [JsonPropertyName(ServerConstants.Material.FieldName)]
    public string Name { get; set; }  = "";
    [JsonPropertyName(ServerConstants.Material.FieldDescription)]
    public string Description { get; set; }  = "";

    [JsonPropertyName(ServerConstants.Material.FieldUid)]
    public string Uid { get; set; } = "";
}