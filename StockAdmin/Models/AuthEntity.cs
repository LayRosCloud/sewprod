using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class AuthEntity : Entity
{
    [JsonPropertyName(ServerConstants.Person.FieldEmail)]
    public string Email { get; set; } = "";
    
    [JsonPropertyName(ServerConstants.Auth.FieldToken)]
    public string Token { get; set; } = "";

}