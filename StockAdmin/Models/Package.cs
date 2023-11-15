using System;
using System.Collections.Generic;
using Avalonia.Media;

namespace StockAdmin.Models;

public class Package
{
    public int id { get; set; }
    public int partyId { get; set; }
    public int sizeId { get; set; }
    public int personId { get; set; }
    public int materialId { get; set; }
    public int count { get; set; }
    public bool isEnded { get; set; }
    public bool isRepeat { get; set; }
    public bool overlockEnded { get; set; }
    public bool ploskayaEnded { get; set; }
    public bool unversalkaEnded { get; set; }
    public bool glazkaEnded { get; set; }
    public bool isUpdated { get; set; }
    public string uid { get; set; }
    
    
    public Size size { get; set; }
    public Party party { get; set; }
    
    public PersonWithPosts person { get; set; }
    public Material material { get; set; }
    private List<ClothOperation> _clothOperations;
    public DateTime createdAt { get; set; } = DateTime.Now;
    public List<ClothOperation> clothOperations
    {
        get => _clothOperations;
        set
        {
            foreach (var clothOperation in value)
            {
                switch (clothOperation.operationId)
                {
                    case 1:
                        overlockEnded = clothOperation.isEnded;
                        break;
                    case 2:
                        unversalkaEnded = clothOperation.isEnded;
                        break;
                    case 3:
                        ploskayaEnded = clothOperation.isEnded;
                        break;
                    case 4:
                        glazkaEnded = clothOperation.isEnded;
                        break;
                }
            }

            _clothOperations = value;
        }
    }


}