using TechSupport.BusinessLogic.Models.RequestModels;
using TechSupport.UI.Models;

namespace TechSupport.UI.Mapping;

public static class RequestMapping
{
    public static CreateRequest MapToCreateRequest(this SlimRequest request)
        => new CreateRequest
        {
            CategoryId = request.Category.Id,            
            DepartmentId = request.Department.Id,
            Description = request.Description,
            Computer = request.Computer,
            Title = request.Title
        };
}
