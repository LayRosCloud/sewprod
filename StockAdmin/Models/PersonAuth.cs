using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class PersonAuth
{
    [JsonPropertyName(ServerConstants.Person.FieldEmail)] 
    public string Email { get; set; } = "";
    

    [JsonPropertyName(ServerConstants.Person.FieldPassword)] 
    public string Password { get; set; } = "";
}