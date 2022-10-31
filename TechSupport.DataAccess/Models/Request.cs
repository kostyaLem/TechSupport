namespace TechSupport.DataAccess.Models;

public class Request
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Computer { get; set; }
    public string Description { get; set; }
    public RequestStatus Status { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? StatusUpdatedOn { get; set; }

    public int? DepartmentId { get; set; }
    public int? RequestCategoryId { get; set; }
    public int? UserId { get; set; }

    public virtual Department Department { get; set; }
    public virtual RequestCategory RequestCategory { get; set; }
    public virtual User User { get; set; }
}
