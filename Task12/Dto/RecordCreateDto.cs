﻿namespace Task12.Dto;

public class RecordCreateDto
{
    public decimal Value { get; set; }
    public string? Description { get; set; }
    public int TypeId { get; set; }
    public DateTime Date { get; set; }
}
