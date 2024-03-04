using System.Collections.Generic;
using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class ClothOperationEntity : Entity
{
    [JsonPropertyName(ServerConstants.ClothOperation.FieldOperationId)]
    public int OperationId { get; set; }
    
    [JsonPropertyName(ServerConstants.ClothOperation.FieldPackageId)]
    public int PackageId { get; set; }
    
    [JsonPropertyName(ServerConstants.ClothOperation.FieldPriceId)]
    public int PriceId { get; set; }
    
    [JsonPropertyName(ServerConstants.ClothOperation.FieldIsEnded)]
    public bool IsEnded { get; set; }
    
    [JsonPropertyName(ServerConstants.ClothOperation.FieldOperation)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public OperationEntity? Operation { get; set; }
    
    [JsonPropertyName(ServerConstants.ClothOperation.FieldPrice)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PriceEntity? Price { get; set; }

    [JsonPropertyName(ServerConstants.ClothOperation.FieldClothOperationPersons)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<ClothOperationPersonEntity> ClothOperationPersons { get; set; } = new();
    
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public OperationTask OperationTask => this.IsEnded
        ? OperationTask.GetCompleted(Operation?.Name!)
        : OperationTask.GetNotCompleted(Operation?.Name!);

}