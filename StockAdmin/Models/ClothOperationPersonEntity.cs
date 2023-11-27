using System;
using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class ClothOperationPersonEntity : Entity
{
    [JsonPropertyName(ServerConstants.ClothOperationPerson.FieldPersonId)]
    public int PersonId { get; set; }
    [JsonPropertyName(ServerConstants.ClothOperationPerson.FieldClothOperationId)]
    public int ClothOperationId { get; set; }
    [JsonPropertyName(ServerConstants.ClothOperationPerson.FieldDateStart)]
    public DateTime DateStart { get; set; } = DateTime.Now;
    [JsonPropertyName(ServerConstants.ClothOperationPerson.FieldIsEnded)]
    public bool IsEnded { get; set; }
    
    [JsonPropertyName(ServerConstants.ClothOperationPerson.FieldPerson)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PersonEntity? Person { get; set; }
    
    [JsonPropertyName(ServerConstants.ClothOperationPerson.FieldClothOperation)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ClothOperationEntity? ClothOperation { get; set; }
}