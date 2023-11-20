using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class PermissionEntity : Entity
{
    [JsonPropertyName(ServerConstants.Permission.FieldPersonId)] 
    public int PersonId { get; set; }
    
    [JsonPropertyName(ServerConstants.Permission.FieldPostId)] 
    public int PostId { get; set; }
}