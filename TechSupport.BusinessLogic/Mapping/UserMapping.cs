using TechSupport.BusinessLogic.Models.UserModels;
using BL = TechSupport.BusinessLogic.Models.UserModels;
using Domain = TechSupport.DataAccess.Models;

namespace TechSupport.BusinessLogic.Mapping;

internal static class UserMapping
{
    public static BL.UserType ToBl(this Domain.UserType userType)
        => userType switch
        {
            Domain.UserType.Admin => BL.UserType.Admin,
            Domain.UserType.Employee => BL.UserType.Employee,
            _ => BL.UserType.Employee
        };

    public static Domain.UserType ToDomain(this BL.UserType userType)
        => userType switch
        {
            BL.UserType.Admin => Domain.UserType.Admin,
            BL.UserType.Employee => Domain.UserType.Employee,
            _ => Domain.UserType.Employee
        };

    public static BL.CurrentUser ToCurrentUser(this Domain.User user)
        => new BL.CurrentUser(user.Login, user.Type.ToBl());

    public static Domain.User ToDomain(this CreateUserRequest request)
    {
        var creationDate = DateTime.Now;
        return new Domain.User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Birthday = request.Birthday,
            Email = request.Email,
            Login = request.Login,
            PasswordHash = request.PasswordHash,
            Phone = request.Phone,
            Type = request.UserType.ToDomain(),
            CreatedOn = creationDate,
            LastActivity = creationDate,
            UpdatedOn = creationDate
        };
    }

    public static BL.User ToBl(this Domain.User user)
        => new BL.User
        {
            FirstName = user.FirstName,
            Login = user.Login,
            CreatedOn = user.CreatedOn,
            LastName = user.LastName,
            Phone = user.Phone,
            Birthday = user.Birthday,
            Email = user.Email,
            Id = user.Id,
            UserType = user.Type.ToBl(),
            UpdatedOn = user.UpdatedOn
        };
}
