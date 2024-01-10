using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class LinkEntity : Entity
{
    [JsonPropertyName(ServerConstants.Link.FieldRel)]
    public string Rel { get; set; } = "";

    [JsonPropertyName(ServerConstants.Link.FieldHref)]
    public string Href { get; set; } = "";
}