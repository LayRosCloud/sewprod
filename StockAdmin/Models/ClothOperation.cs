﻿using System;

namespace StockAdmin.Models;

public class ClothOperation
{
    public int id { get; set; }
    public int operationId { get; set; }
    public int packageId { get; set; }
    public int personId { get; set; }
    public int priceId { get; set; }
    public DateTime dateStart { get; set; } = DateTime.Now;
    public bool isEnded { get; set; }
    public Operation operation { get; set; }
    public Party party { get; set; }
    public Person person { get; set; }
    public Price price { get; set; }
}