using System;

namespace StockAdmin.Models;

public class Party
{
    public int id { get; set; }
    public int modelId { get; set; }
    public int personId { get; set; } = 2;
    public int count { get; set; } = 1;
    public DateTime dateStart { get; set; } = DateTime.Now;
    public DateTime? dateEnd { get; set; } = DateTime.Now;
    public bool isDefected { get; set; }
    public int cutNumber { get; set; } = 1;
    public int sizeId { get; set; }
    
    public Person person { get; set; }
    public Model model { get; set; }
    public Size size { get; set; }
}