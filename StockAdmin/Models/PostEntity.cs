using System;
using System.Text.Json.Serialization;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class PostEntity : Entity
{
    [JsonPropertyName(ServerConstants.Post.FieldName)]
    public string Name { get; set; } = "";
    

    [JsonPropertyName(ServerConstants.Post.FieldDescription)] 
    public string Description { get; set; } = "";

    public override bool Equals(object? obj)
    {
        if (obj is PostEntity postEntity)
        {
            return Name == postEntity.Name;
        }

        throw new ArgumentException();
    }
}