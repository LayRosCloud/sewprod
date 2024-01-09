using System.Collections.Generic;

namespace StockAdmin.Models;

public class PersonGroup
{
    public PersonEntity Person { get; set; } = new();
    public List<ClothOperationEntity> Operations { get; set; } = new();
    public List<PackageEntity> Packages { get; set; } = new();
}