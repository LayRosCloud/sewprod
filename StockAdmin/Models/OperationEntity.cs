using System.Collections.Generic;
using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class OperationEntity : Entity
{
    [JsonPropertyName(ServerConstants.Operation.FieldName)]
    public string Name { get; set; } = "";

    [JsonPropertyName(ServerConstants.Operation.FieldDescription)]
    public string Description { get; set; } = "";

    [JsonPropertyName(ServerConstants.Operation.FieldUid)]
    public string Uid { get; set; } = "";
    [JsonPropertyName(ServerConstants.Operation.FieldPercent)] 
    public double Percent { get; set; }
    
    [JsonPropertyName(ServerConstants.Operation.FieldModelOperation)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] 
    public ModelPriceEntity? ModelOperation { get; set; }

}