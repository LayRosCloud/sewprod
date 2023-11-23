using System;
using System.Collections.Generic;
using System.Linq;
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
            foreach (var clothOperation in value.OrderBy(x=>x.OperationId))
            {
                string operationName = clothOperation.Operation?.Name!;
                CompletedTasks.Add(clothOperation.IsEnded 
                    ? OperationTaskEntity.GetCompleted(operationName) 
                    : OperationTaskEntity.GetNotCompleted(operationName));
            }

            _clothOperations = value;
        }
    }

    [JsonIgnore]
    public List<OperationTaskEntity> CompletedTasks { get; set; } = new();

    [JsonIgnore]
    public SolidColorBrush StatusColor { get; set; } = new(Color.FromRgb(238, 245, 255));
    
    [JsonIgnore]
    public string Status
    {
        get
        {
            if (IsEnded)
            {
                StatusColor = new SolidColorBrush(Color.FromRgb(149, 192, 160));
                return "Закончена";
            }
            
            if (IsRepeat)
            {
                StatusColor = new SolidColorBrush(Color.FromRgb(244, 158, 49));
                return "Повтор";
            }
            
            bool isAdmin = Person!.Posts.Contains(new PostEntity() { Name = "ADMIN" });
            if (isAdmin)
            {
                StatusColor = new SolidColorBrush(Color.FromRgb(125, 181, 251));
                return "Добавлена админом";
            }
            
            if (IsUpdated)
            {
                StatusColor = new SolidColorBrush(Color.FromRgb(40, 40, 40));
                return "Обновлена";
            }
            

            return "Добавлена кройщиком";
        }
    }
}