using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class PartyEntity : Entity
{

    [JsonPropertyName(ServerConstants.Party.FieldModelId)] 
    public int ModelId { get; set; }
    
    [JsonPropertyName(ServerConstants.Party.FieldPersonId)] 
    public int PersonId { get; set; }
    
    [JsonPropertyName(ServerConstants.Party.FieldPriceId)] 
    public int PriceId { get; set; }
    
    [JsonPropertyName(ServerConstants.Party.FieldDateStart)] 
    public DateTime DateStart { get; set; } = DateTime.Now;
    
    [JsonPropertyName(ServerConstants.Party.FieldDateEnd)] 
    public DateTime? DateEnd { get; set; } = DateTime.Now;

    [JsonPropertyName(ServerConstants.Party.FieldCutNumber)]
    public string CutNumber { get; set; } = "";
    
    [JsonPropertyName(ServerConstants.Party.FieldPerson)] 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PersonEntity? Person { get; set; }
    
    [JsonPropertyName(ServerConstants.Party.FieldModel)] 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ModelEntity? Model { get; set; }
    
    [JsonPropertyName(ServerConstants.Party.FieldPrice)] 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PriceEntity? Price { get; set; }
    
    [JsonPropertyName(ServerConstants.Party.FieldPackages)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<PackageEntity>? Packages { get; set; }
}