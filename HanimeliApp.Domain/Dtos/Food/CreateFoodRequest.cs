﻿namespace HanimeliApp.Domain.Dtos.Food;

public class CreateFoodRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}