using TechSupport.BusinessLogic.Models.UserModels;

namespace TechSupport.BusinessLogic.Models.RequestModels;

public record ExtendedRequest
{
    public int Id { get; init; }
    public string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Computer { get; set; }
    public Department Department { get; set; }
    public Category Category { get; set; }
    public User User { get; set; }
    public RequestStatus RequestStatus { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? StatusUpdatedOn { get; set; }
}
