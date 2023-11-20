using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class SizeEntity : Entity
{
    [JsonPropertyName(ServerConstants.Size.FieldNumber)] public string Number { get; set; } = "";
    [JsonPropertyName(ServerConstants.Size.FieldAgeId)] public int AgeId { get; set; }
    
    [JsonPropertyName(ServerConstants.Size.FieldAge)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public AgeEntity? Age { get; set; }
}