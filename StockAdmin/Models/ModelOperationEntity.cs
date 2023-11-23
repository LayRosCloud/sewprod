using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class ModelOperationEntity : Entity
{
    [JsonPropertyName(ServerConstants.ModelOperation.FieldOperationId)]
    public int OperationId { get; set; }
    [JsonPropertyName(ServerConstants.ModelOperation.FieldModelId)]
    public int ModelId { get; set; }
}