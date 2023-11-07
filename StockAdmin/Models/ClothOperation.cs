using System.Collections.Generic;

namespace StockAdmin.Models;

public class ClothOperation
{
    public int id { get; set; }
    public int operationId { get; set; }
    public int packageId { get; set; }
    public int priceId { get; set; }
    public bool isEnded { get; set; }
    public Operation operation { get; set; }
    public Price price { get; set; }
    
    public List<ClothOperationPerson> clothOperationPersons { get; set; }
}