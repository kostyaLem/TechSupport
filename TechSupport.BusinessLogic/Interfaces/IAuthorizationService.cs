using TechSupport.BusinessLogic.Models;

namespace TechSupport.BusinessLogic.Interfaces;

public interface IAuthorizationService
{
    Task<CurrentUser> Authorize(string nickname, string password);
}
