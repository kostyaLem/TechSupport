using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Models.UserModels;
using TechSupport.DataAccess.Context;

namespace TechSupport.BusinessLogic.Services;

internal class UserService : IUserService
{
    private readonly TechSupportContext _context;

    public UserService(TechSupportContext context)
    {
        _context = context;
    }

    public Task Create(CreateUserRequest user)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<User>> GetUsers()
    {
        throw new NotImplementedException();
    }

    public Task Remove(int userId)
    {
        throw new NotImplementedException();
    }

    public Task Update(User user, string passwordHash)
    {
        throw new NotImplementedException();
    }
}
