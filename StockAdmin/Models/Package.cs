using System;
using Avalonia.Media;

namespace StockAdmin.Models;

public class Package
{
    public int id { get; set; }
    public int partyId { get; set; }
    public int sizeId { get; set; }
    public int personId { get; set; }
    public int materialId { get; set; }
    public int colorId { get; set; }
    public int count { get; set; }
    public bool isEnded { get; set; }
    public bool isRepeat { get; set; }
    public bool isUpdated { get; set; }
    public string uid { get; set; }
    public Size size { get; set; }
    public PersonWithPosts person { get; set; }
    public Material material { get; set; }
    public Color color { get; set; }

}