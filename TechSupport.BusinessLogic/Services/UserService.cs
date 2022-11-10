using Microsoft.EntityFrameworkCore;
using TechSupport.BusinessLogic.Exceptions;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Mapping;
using TechSupport.BusinessLogic.Models.UserModels;
using TechSupport.DataAccess.Context;
using Domain = TechSupport.DataAccess.Models;

namespace TechSupport.BusinessLogic.Services;

/// <summary>
/// Сервис для управления пользователями
/// </summary>
internal class UserService : IUserService
{
    private readonly TechSupportContext _context;

    public UserService(TechSupportContext context)
    {
        _context = context;
    }

    // Создание пользователя в системе
    public async Task Create(CreateUserRequest request)
    {
        // Поиск существующего пользователя по поставляемым данным
        var user = await _context.Users
            .FirstOrDefaultAsync(user =>
                user.FirstName == request.FirstName &&
                user.LastName == request.LastName &&
                user.Email == request.Email &&
                user.Phone == request.Phone &&
                user.Birthday == request.Birthday);

        if (user is not null)
        {
            // Выбросить исключение, если попытка создать пользователя с похожими данными
            throw new DuplicateDataException("Такой пользователь уже существует.");
        }

        // Добавить сущность в базу данных
        _context.Users.Add(request.ToDomain());
        // Сохранить изменения в базе
        await _context.SaveChangesAsync();
    }

    // Удалить пользователя по Id
    public async Task Remove(int userId)
    {
        var user = await GetUser(userId);

        var otherAdmins = await _context.Users.CountAsync(x => x.Type == Domain.UserType.Admin);

        if (user.Type == Domain.UserType.Admin && otherAdmins == 1)
        {
            throw new Exception("Невозможно удалить последнего администратора.");
        }

        // Пометить сущность на удаление
        _context.Users.Remove(user);
        // Сохранить изменения в базе
        await _context.SaveChangesAsync();
    }

    // Получить список пользователей
    public async Task<IReadOnlyList<User>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();

        // Преобразование модели
        return users.Select(x => x.ToBl()).ToList();
    }

    // Получить пользователя по Id
    public async Task<User> GetUserById(int userId)
    {
        var user = await GetUser(userId);

        return user.ToBl();
    }

    // Обновить пользователя
    public async Task Update(User user, string passwordHash)
    {
        var existingUser = await GetUser(user.Id);

        // Обновить поля сущности на новые данные
        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Birthday = user.Birthday;
        existingUser.Phone = user.Phone;
        existingUser.Login = user.Login;
        existingUser.Email = user.Email;
        existingUser.UpdatedOn = DateTime.Now;
        existingUser.Type = user.UserType.ToDomain();

        if (!string.IsNullOrWhiteSpace(passwordHash))
        {
            // Обновить пароль, если есть изменения
            existingUser.PasswordHash = PasswordGenerator.Generate(passwordHash);
        }

        // Сохранить изменения в базе
        await _context.SaveChangesAsync();
    }

    // Получить пользователя по Id и выбросить ошибку, если сущности нет
    private async Task<Domain.User> GetUser(int userId)
    {
        // Поиск сущности в базе по Id
        var user = await _context.Users.FindAsync(userId);

        if (user is null)
        {
            // Выбросить исключение, если сущность не найдена
            throw new NotFoundException("Пользователь не найден.");
        }

        return user;
    }
}
