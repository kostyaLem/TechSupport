using BL = TechSupport.BusinessLogic.Models.UserModels;
using Domain = TechSupport.DataAccess.Models;

namespace TechSupport.BusinessLogic.Mapping;

internal static class UserMapping
{
    public static BL.UserType MapUserType(this Domain.UserType userType)
        => userType switch
        {
            Domain.UserType.Admin => BL.UserType.Admin,
            Domain.UserType.Employee => BL.UserType.Employee,
            _ => BL.UserType.Employee,
        };

    public static BL.CurrentUser ToBl(this Domain.User user)
        => new BL.CurrentUser(user.Login, user.Type.MapUserType());
}
