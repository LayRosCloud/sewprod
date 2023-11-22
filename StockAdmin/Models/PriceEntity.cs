using System;
using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class PriceEntity : Entity
{
    [JsonPropertyName(ServerConstants.Price.FieldNumber)] 
    public double Number { get; set; }
    [JsonPropertyName(ServerConstants.Price.FieldDate)] 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime Date { get; set; } = DateTime.Now;
}