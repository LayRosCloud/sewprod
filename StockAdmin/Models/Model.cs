namespace StockAdmin.Models;

public class Model
{
    public int id { get; set; }
    public string title { get; set; }
    public string codeVendor { get; set; }
    
    public int priceId { get; set; }
    public Price price { get; set; }
}