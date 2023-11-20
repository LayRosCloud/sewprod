using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class PersonEntity : Entity
{
    [JsonPropertyName(ServerConstants.Person.FieldEmail)] public string Email { get; set; } = "";
    
    [JsonPropertyName(ServerConstants.Person.FieldPassword)] public string Password { get; set; } = "";
    
    [JsonPropertyName(ServerConstants.Person.FieldLastName)] public string LastName { get; set; } = "";
    
    [JsonPropertyName(ServerConstants.Person.FieldFirstName)] public string FirstName { get; set; } = "";
    
    [JsonPropertyName(ServerConstants.Person.FieldPatronymic)] public string? Patronymic { get; set; }
    
    [JsonPropertyName(ServerConstants.Person.FieldBirthDay)] public DateTime BirthDay { get; set; } = DateTime.Now;
    
    [JsonPropertyName(ServerConstants.Person.FieldUid)] public string Uid { get; set; } = "";

    [JsonIgnore]
    public string FullName => $"{LastName} {FirstName?[0]}. {Patronymic?[0]}.";
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName(ServerConstants.Person.FieldPosts)] public List<PostEntity> Posts { get; set; }
}