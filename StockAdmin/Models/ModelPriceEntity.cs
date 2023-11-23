using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class ModelPriceEntity : Entity
{
    [JsonPropertyName(ServerConstants.ModelPrice.FieldPriceId)]
    public int PriceId { get; set; }
    
    [JsonPropertyName(ServerConstants.ModelPrice.FieldModelId)]
    public int ModelId { get; set; }
}