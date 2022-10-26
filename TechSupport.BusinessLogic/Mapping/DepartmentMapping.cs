using TechSupport.BusinessLogic.Models;
using Domain = TechSupport.DataAccess.Models;

namespace TechSupport.BusinessLogic.Mapping;

internal static class DepartmentMapping
{
    public static Department ToBl(this Domain.Department department)
        => new Department
        {
            Id = department.Id,
            Title = department.Title,
            Room = department.Room,
            Place = department.Place
        };
}

