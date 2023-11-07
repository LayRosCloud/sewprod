namespace StockAdmin.Models;

public class Operation
{
    public int id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string uid { get; set; }
    public int priceId { get; set; }
    public Price price { get; set; }
}