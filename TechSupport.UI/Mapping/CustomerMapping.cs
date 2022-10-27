using TechSupport.BusinessLogic.Models.UserModels;

namespace TechSupport.UI.Mapping;

public static class CustomerMapping
{
    public static CreateUserRequest MapToCreateRequest(this User user, string passwordHash)
    {
        return new CreateUserRequest
        {
            Login = user.Login,
            Birthday = user.Birthday,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PasswordHash = passwordHash,
            Phone = user.Phone,
            UserType = user.UserType,
        };
    }
}
