using TechSupport.BusinessLogic.Models.UserModels;

namespace TechSupport.BusinessLogic.Interfaces;

public interface IUserService
{
    Task<IReadOnlyList<User>> GetUsers();
    Task<User> GetUserById(int id);
    Task Create(CreateUserRequest user);
    Task Update(User user, string passwordHash);
    Task Remove(int userId);
}
