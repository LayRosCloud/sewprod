using System;
using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class HistoryEntity : Entity
{
    [JsonPropertyName(ServerConstants.History.FieldPersonId)]
    public int PersonId { get; set; }
    
    [JsonPropertyName(ServerConstants.History.FieldActionId)]
    public int ActionId { get; set; }

    [JsonPropertyName(ServerConstants.History.FieldTableName)]
    public string TableName { get; set; } = "";

    [JsonPropertyName(ServerConstants.History.FieldValue)]
    public string Value { get; set; } = "";
    
    [JsonPropertyName(ServerConstants.History.FieldCreatedAt)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime CreatedAt { get; set; }
    
    [JsonPropertyName(ServerConstants.History.FieldPerson)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PersonEntity? Person { get; set; }
    
    [JsonPropertyName(ServerConstants.History.FieldAction)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ActionEntity? Action { get; set; }
}