using Avalonia.Media;

namespace StockAdmin.Models;

public class Package
{
    public int id { get; set; }
    public int partyId { get; set; }
    public int sizeId { get; set; }
    public int count { get; set; }
    public bool isEnded { get; set; }
    public Size size { get; set; }
    public int personId { get; set; }
    
    public Person person { get; set; }

}