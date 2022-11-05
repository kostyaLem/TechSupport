using TechSupport.BusinessLogic.Exceptions;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Models.UserModels;
using TechSupport.DataAccess.Context;
using TechSupport.BusinessLogic.Mapping;
using Microsoft.EntityFrameworkCore;

namespace TechSupport.BusinessLogic.Services;

/// <summary>
/// Сервис для авторизации пользователей в системе
/// </summary>
internal class AuthorizationService : IAuthorizationService
{
    private readonly TechSupportContext _context;

    public AuthorizationService(TechSupportContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Метод авторизации пользователя
    /// </summary>
    /// <param name="nickname">Логин</param>
    /// <param name="password">Пароль</param>
    public async Task<CurrentUser> Authorize(string nickname, string password)
    {
        // Преобразование пароля в хэш
        var passwordHash = PasswordGenerator.Generate(password);

        // Поиск пользователя в базе
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == nickname);

        if (user is null)
        {
            // Выбросить исключение, если пользователь не найден
            throw new UserNotFoundAuthorizeException();
        }

        if (user.PasswordHash != passwordHash)
        {
            // Выбросить исключение, если пароль не совпадает
            throw new AuthorizeException();
        }

        // Преобразование модели
        return user.ToCurrentUser();
    }
}
