using System.Collections.Generic;
using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class AuthEntity : Entity
{
    [JsonPropertyName(ServerConstants.Person.FieldEmail)]
    public string Email { get; set; } = "";
    
    [JsonPropertyName(ServerConstants.Auth.FieldToken)]
    public string Token { get; set; } = "";
    
    [JsonPropertyName(ServerConstants.Person.FieldFirstName)]
    public string FirstName { get; set; } = "";
    
    [JsonPropertyName(ServerConstants.Person.FieldLastName)]
    public string LastName { get; set; } = "";
    
    [JsonPropertyName(ServerConstants.Auth.FieldPosts)]
    public List<PostEntity>? Posts { get; set; }
}