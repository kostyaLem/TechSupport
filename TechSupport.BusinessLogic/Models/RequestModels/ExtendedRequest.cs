using TechSupport.BusinessLogic.Models.UserModels;

namespace TechSupport.BusinessLogic.Models.RequestModels;

public record ExtendedRequest
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; } = string.Empty;
    public string Computer { get; init; }
    public Department Department { get; init; }
    public Category Category { get; init; }
    public User User { get; init; }
    public RequestStatus RequestStatus { get; init; }
    public DateTime CreatedOn { get; init; }
    public DateTime? StatusUpdatedOn { get; init; }
}
