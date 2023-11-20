using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Avalonia.Media;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class PackageEntity : Entity
{
    private List<ClothOperationEntity> _clothOperations = new();

    [JsonPropertyName(ServerConstants.Package.FieldPartyId)] public int PartyId { get; set; }
    
    [JsonPropertyName(ServerConstants.Package.FieldSizeId)] public int SizeId { get; set; }
    [JsonPropertyName(ServerConstants.Package.FieldPersonId)] public int PersonId { get; set; }
    [JsonPropertyName(ServerConstants.Package.FieldMaterialId)] public int MaterialId { get; set; }
    [JsonPropertyName(ServerConstants.Package.FieldCount)] public int Count { get; set; }
    [JsonPropertyName(ServerConstants.Package.FieldIsEnded)] public bool IsEnded { get; set; }
    [JsonPropertyName(ServerConstants.Package.FieldIsRepeat)] public bool IsRepeat { get; set; }
    [JsonPropertyName(ServerConstants.Package.FieldIsUpdated)] public bool IsUpdated { get; set; }

    [JsonPropertyName(ServerConstants.Package.FieldUid)]
    public string Uid { get; set; } = "";
    
    [JsonPropertyName(ServerConstants.Package.FieldSize)] 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SizeEntity? Size { get; set; }
    
    [JsonPropertyName(ServerConstants.Package.FieldParty)] 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PartyEntity? Party { get; set; }
    
    [JsonPropertyName(ServerConstants.Package.FieldPerson)] 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PersonEntity? Person { get; set; }
    
    [JsonPropertyName(ServerConstants.Package.FieldMaterial)] 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public MaterialEntity? Material { get; set; }
    
    [JsonPropertyName(ServerConstants.Package.FieldCreatedAt)] 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    [JsonPropertyName(ServerConstants.Package.FieldClothOperations)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<ClothOperationEntity> ClothOperations
    {
        get => _clothOperations;
        set
        {
            foreach (var clothOperation in value)
            {
                switch (clothOperation.OperationId)
                {
                    case 1:
                        OverlockEnded = clothOperation.IsEnded;
                        break;
                    case 2:
                        UniversalEnded = clothOperation.IsEnded;
                        break;
                    case 3:
                        FlatEnded = clothOperation.IsEnded;
                        break;
                    case 4:
                        IroningEnded = clothOperation.IsEnded;
                        break;
                }
            }

            _clothOperations = value;
        }
    }
    
    [JsonIgnore] public bool OverlockEnded { get; set; }
    
    [JsonIgnore] public bool UniversalEnded { get; set; }
    
    [JsonIgnore] public bool FlatEnded { get; set; }
    
    [JsonIgnore] public bool IroningEnded { get; set; }

    [JsonIgnore]
    public SolidColorBrush StatusColor { get; set; } = new(Color.Parse("#EEF5FF"));
    
    [JsonIgnore]
    public string Status
    {
        get
        {
            if (IsEnded)
            {
                StatusColor = new SolidColorBrush(Color.Parse("#95c0a0"));
                return "Закончена";
            }
            
            if (IsRepeat)
            {
                StatusColor = new SolidColorBrush(Color.Parse("f49e31"));
                return "Повтор";
            }
            
            bool isAdmin = Person!.Posts.Contains(new PostEntity() { Name = "ADMIN" });
            if (isAdmin)
            {
                StatusColor = new SolidColorBrush(Color.Parse("#7db5fb"));
                return "Добавлена администратором";
            }
            
            if (IsUpdated)
            {
                StatusColor = new SolidColorBrush(Color.Parse("#282828"));
                return "Обновлена";
            }

            return "Добавлена кройщиком";
        }
    }
}