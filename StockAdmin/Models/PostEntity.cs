using System.Collections.Generic;
using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class PostEntity : Entity
{
    [JsonIgnore]
    public const string CutterName = "CUTTER";
    [JsonIgnore]
    public const string AdminName = "ADMIN";
    [JsonIgnore]
    public const string EmployeeName = "EMPLOYEE";

    [JsonPropertyName(ServerConstants.Post.FieldName)]
    public string Name { get; set; } = "";

    [JsonPropertyName(ServerConstants.Post.FieldDescription)] 
    public string Description { get; set; } = "";
    
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public List<PersonEntity> Persons { get; set; }
}