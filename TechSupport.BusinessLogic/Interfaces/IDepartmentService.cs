using TechSupport.BusinessLogic.Models;

namespace TechSupport.BusinessLogic.Interfaces;

public interface IDepartmentService
{
    Task<IReadOnlyList<Department>> GetDepartments();
    Task Create(Department department);
    Task<Department> GetDepartmentById(int departmentId);
    Task Update(Department department);
    Task Remove(int departmentId);
}