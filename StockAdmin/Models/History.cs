using System;

namespace StockAdmin.Models;

public class History
{
    public int id { get; set; }
    public int personId { get; set; }
    public int actionId { get; set; }
    public string tableName { get; set; }
    public string value { get; set; }
    public DateTime createdAt { get; set; }
    public Person person { get; set; }
    public Action action { get; set; }
}