using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using StockAdmin.Scripts.Constants;
using StockAdmin.Scripts.Records;
using StockAdmin.Scripts.Server;

namespace StockAdmin.Models;

public class PackageEntity : Entity
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [NotMapped]
    private List<ClothOperationEntity> _clothOperations;
    
    [JsonPropertyName(ServerConstants.Package.FieldPartyId)] 
    public int PartyId { get; set; }
    
    [JsonPropertyName(ServerConstants.Package.FieldSizeId)] 
    public int SizeId { get; set; }
    [JsonPropertyName(ServerConstants.Package.FieldPersonId)] 
    public int PersonId { get; set; }
    [JsonPropertyName(ServerConstants.Package.FieldMaterialId)] 
    public int MaterialId { get; set; }

    [JsonPropertyName(ServerConstants.Package.FieldCount)]
    public int Count { get; set; } = 0;

    [JsonPropertyName(ServerConstants.Package.FieldIsEnded)]
    public bool IsEnded { get; set; } = false;
    [JsonPropertyName(ServerConstants.Package.FieldIsRepeat)] 
    public bool IsRepeat { get; set; } = false;

    [JsonPropertyName(ServerConstants.Package.FieldIsUpdated)]
    public bool IsUpdated { get; set; } = false;

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
                    ? OperationTask.GetCompleted(operationName) 
                    : OperationTask.GetNotCompleted(operationName));
            }

            _clothOperations = value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public List<OperationTask> CompletedTasks { get; set; } = new();
    
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public Status? Status
    {
        get
        {
            
            if (IsEnded)
            {
                return new Status("Закончена", ColorConstants.Completed);
            }
            
            if (IsRepeat)
            {
                return new Status("Повтор", ColorConstants.Repeat);
            }
            
            if (Person!.Posts != null)
            {
                foreach (var post in Person.Posts)
                {
                    if (post.Name == PostEntity.AdminName)
                    {
                        return new Status("Добавлена админом", ColorConstants.Admin);
                    }
                }
            }


            if (IsUpdated)
            {
                return new Status("Обновлена", ColorConstants.Updated);
            }
            

            return new Status("Добавлена кройщиком", ColorConstants.Cutter);
        }
        private set{}
    }
    
}