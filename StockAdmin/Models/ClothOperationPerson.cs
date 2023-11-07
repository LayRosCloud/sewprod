using System;

namespace StockAdmin.Models;

public class ClothOperationPerson
{
    public int id { get; set; }
    public int personId { get; set; }
    public int clothOperationId { get; set; }
    public DateTime dateStart { get; set; } = DateTime.Now;
    public bool isEnded { get; set; }
    public Person person { get; set; }
}