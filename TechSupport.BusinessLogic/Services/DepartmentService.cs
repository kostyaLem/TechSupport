using Microsoft.EntityFrameworkCore;
using TechSupport.BusinessLogic.Exceptions;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Mapping;
using TechSupport.BusinessLogic.Models;
using TechSupport.DataAccess.Context;
using Domain = TechSupport.DataAccess.Models;

namespace TechSupport.BusinessLogic.Services;

internal sealed class DepartmentService : IDepartmentService
{
    private readonly TechSupportContext _context;

    public DepartmentService(TechSupportContext context)
    {
        _context = context;
    }

    public async Task Create(Department department)
    {
        _context.Departments.Add(new Domain.Department
        {
            Title = department.Title,
            Room = department.Room,
            Place = department.Place
        });

        await _context.SaveChangesAsync();
    }

    public async Task<Department> GetDepartmentById(int departmentId)
    {
        var department = await GetDepartment(departmentId);

        return department.ToBl();
    }

    public async Task<IReadOnlyList<Department>> GetDepartments()
    {
        var departments = await _context.Departments
            .AsNoTracking()
            .ToListAsync();

        return departments.Select(x => x.ToBl()).ToList();
    }

    public async Task Remove(int departmentId)
    {
        var department = await GetDepartment(departmentId);

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Department department)
    {
        var existingDepartment = await GetDepartment(department.Id);

        existingDepartment.Title = department.Title;
        existingDepartment.Place = department.Place;
        existingDepartment.Room = department.Room;

        await _context.SaveChangesAsync();
    }

    private async Task<Domain.Department> GetDepartment(int entityId)
    {
        var department = await _context.Departments.FindAsync(entityId);

        if (department is null)
        {
            throw new NotFoundException("Отдел не найден.");
        }

        return department;
    }
}
