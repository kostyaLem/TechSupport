﻿namespace TechSupport.BusinessLogic.Models.CategoriesModels;

public record Category
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public byte[] ImageData { get; init; }
}
