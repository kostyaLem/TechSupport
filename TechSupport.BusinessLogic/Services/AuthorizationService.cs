using TechSupport.BusinessLogic.Exceptions;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Models.UserModels;
using TechSupport.DataAccess.Context;
using TechSupport.BusinessLogic.Mapping;

namespace TechSupport.BusinessLogic.Services;

internal class AuthorizationService : IAuthorizationService
{
    private readonly TechSupportContext _context;

    public AuthorizationService(TechSupportContext context)
    {
        _context = context;
    }

    public async Task<CurrentUser> Authorize(string nickname, string password)
    {
        var passwordHash = PasswordGenerator.Generate(password);

        var user = await _context.Users.FindAsync(nickname);

        if (user is null)
        {
            throw new UserNotFoundAuthorizeException();
        }

        if (user.PasswordHash != passwordHash)
        {
            throw new AuthorizeException();
        }

        return user.ToCurrentUser();
    }
}
