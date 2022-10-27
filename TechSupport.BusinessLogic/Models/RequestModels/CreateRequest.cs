namespace TechSupport.BusinessLogic.Models.RequestModels;

public record CreateRequest
{
    public string Title { get; init; }
    public string Description { get; init; }
    public string Computer { get; init; }

    public int DepartmentId { get; init; }
    public int CategoryId { get; init; }
}
