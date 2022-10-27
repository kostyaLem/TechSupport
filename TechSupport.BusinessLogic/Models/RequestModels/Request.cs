using TechSupport.BusinessLogic.Models.UserModels;

namespace TechSupport.BusinessLogic.Models.RequestModels;

public record Request
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string Computer { get; init; }
    public string Category { get; init; }
    public string Department { get; init; }
    public RequestStatus Status { get; init; }
    public DateTime CreatedOn { get; init; }
    public DateTime PausedOn { get; init; }
    public DateTime CompletedOn { get; init; }
    public User User { get; init; }
}
