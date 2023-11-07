namespace StockAdmin.Models;

public class Size
{
    public int id { get; set; }
    public string name { get; set; }
    public string number { get; set; }
    public int ageId { get; set; }
    public Age age { get; set; }
}