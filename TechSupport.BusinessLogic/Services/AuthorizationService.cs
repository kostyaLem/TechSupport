using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Models;

namespace TechSupport.BusinessLogic.Services;

internal class AuthorizationService : IAuthorizationService
{
    public Task<CurrentUser> Authorize(string nickname, string password)
    {
        throw new NotImplementedException();
    }
}
