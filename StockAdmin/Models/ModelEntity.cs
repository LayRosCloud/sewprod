using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class ModelEntity : Entity
{
    [JsonPropertyName(ServerConstants.Model.FieldTitle)] 
    public string Title { get; set; } = "";
    [JsonPropertyName(ServerConstants.Model.FieldCodeVendor)] 
    public string CodeVendor { get; set; } = "";
    [JsonPropertyName(ServerConstants.Model.FieldPriceId)] 
    public int PriceId { get; set; }
    [JsonPropertyName(ServerConstants.Model.FieldPrice)] 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PriceEntity? Price { get; set; }
}