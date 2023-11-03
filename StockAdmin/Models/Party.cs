using System;
using System.Collections.Generic;

namespace StockAdmin.Models;

public class Party
{
    public int id { get; set; }
    public int modelId { get; set; }
    public int personId { get; set; }
    public DateTime dateStart { get; set; } = DateTime.Now;
    public DateTime? dateEnd { get; set; } = DateTime.Now;
    public int cutNumber { get; set; } = 1;
    
    public Person person { get; set; }
    public Model model { get; set; }
    public List<Package> packages { get; set; }
}