namespace StockAdmin.Models;

public class Model
{
    public int id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string codeVendor { get; set; }
    public int percent { get; set; } = 0;
    public Size[] sizes { get; set; }
}