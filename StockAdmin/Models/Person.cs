using System;
using System.Collections.Generic;

namespace StockAdmin.Models;

public class Person
{
    public int id { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public string lastName { get; set; }
    public string firstName { get; set; }
    public string? patronymic { get; set; }
    public DateTime birthDay { get; set; } = DateTime.Now;
    public string uid { get; set; }
}