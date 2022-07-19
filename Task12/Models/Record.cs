﻿namespace Task12.Models;

public class Record 
{
    public int Id { get; set; }
    public decimal Value { get; set; }
    public string? Description { get; set; }
    public int TypeId { get; set; }
    public DateTime Date { get; set; }
}
