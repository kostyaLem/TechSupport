using TechSupport.BusinessLogic.Models.RequestModels;
using Domain = TechSupport.DataAccess.Models;

namespace TechSupport.BusinessLogic.Mapping;

internal static class RequestMapping
{
    public static Domain.Request ToDomain(this CreateRequest request)
        => new Domain.Request
        {
            Title = request.Title,
            DepartmentId = request.DepartmentId,
            CreatedOn = DateTime.Now,
            Description = request.Description,
            Computer = request.Computer,
            Status = Domain.RequestStatus.Created,
            RequestCategoryId = request.CategoryId
        };

    public static Request ToBl(this Domain.Request request)
        => new Request
        {
            Id = request.Id,
            Category = request.RequestCategory.Title,
            Department = request.Department.Room,
            Title = request.Title,
            Computer = request.Computer,
            CreatedOn = request.CreatedOn,
            CompletedOn = request.CompletedOn,
            Description = request.Description,
            PausedOn = request.PausedOn,
            Status = request.Status.ToBl(),
            User = request.User.ToBl()
        };

    public static RequestStatus ToBl(this Domain.RequestStatus status)
        => status switch
        {
            Domain.RequestStatus.Created => RequestStatus.Created,
            Domain.RequestStatus.InProgress => RequestStatus.InProgress,
            Domain.RequestStatus.Paused => RequestStatus.Paused,
            Domain.RequestStatus.Completed => RequestStatus.Completed,
            _ => throw new Exception("Не удалось преобразовать статус заявки")
        };
}
