using Microsoft.EntityFrameworkCore;
using TechSupport.BusinessLogic.Exceptions;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Mapping;
using TechSupport.BusinessLogic.Models;
using TechSupport.DataAccess.Context;
using Domain = TechSupport.DataAccess.Models;

namespace TechSupport.BusinessLogic.Services;

/// <summary>
/// Сервис для работы с отделами технической поддержки
/// </summary>
internal sealed class DepartmentService : IDepartmentService
{
    private readonly TechSupportContext _context;

    public DepartmentService(TechSupportContext context)
    {
        _context = context;
    }

    // Метод создания отдела технической поддержки
    public async Task Create(Department department)
    {
        // Создание сущности в базе данных
        _context.Departments.Add(new Domain.Department
        {
            Title = department.Title,
            Room = department.Room,
            Place = department.Place
        });

        // Сохранение данных в базе данных
        await _context.SaveChangesAsync();
    }

    // Получить отдел по Id
    public async Task<Department> GetDepartmentById(int departmentId)
    {
        var department = await GetDepartment(departmentId);

        // Преобразование модели
        return department.ToBl();
    }

    // Получить список всех отделов в базе данных
    public async Task<IReadOnlyList<Department>> GetDepartments()
    {
        var departments = await _context.Departments.ToListAsync();

        return departments.Select(x => x.ToBl()).ToList();
    }

    // Удалить отдел по Id
    public async Task Remove(int departmentId)
    {
        var department = await GetDepartment(departmentId);

        // Пометить сущность на удаление
        _context.Departments.Remove(department);
        // Сохранение данных в базе данных
        await _context.SaveChangesAsync();
    }


    public async Task Update(Department department)
    {
        var existingDepartment = await GetDepartment(department.Id);

        // Обновление некоторых полей сущности
        existingDepartment.Title = department.Title;
        existingDepartment.Place = department.Place;
        existingDepartment.Room = department.Room;

        // Сохранить изменения в базе
        await _context.SaveChangesAsync();
    }

    // Получить отдел по Id и выбросить ошибку, если сущности нет
    private async Task<Domain.Department> GetDepartment(int entityId)
    {
        // Поиск сущности в базе по Id
        var department = await _context.Departments.FindAsync(entityId);

        if (department is null)
        {
            // Выбросить исключение, если сущность не найдена
            throw new NotFoundException("Отдел не найден.");
        }

        return department;
    }
}
