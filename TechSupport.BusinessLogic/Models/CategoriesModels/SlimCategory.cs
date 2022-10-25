namespace TechSupport.BusinessLogic.Models.CategoriesModels;

public record SlimCategory
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
}
