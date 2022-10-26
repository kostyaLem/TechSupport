using System.Runtime.CompilerServices;
using TechSupport.BusinessLogic.Models;
using Domain = TechSupport.DataAccess.Models;

namespace TechSupport.BusinessLogic.Mapping;

internal static class CategoryMapping
{
    public static Category ToBl(this Domain.RequestCategory category)
        => new Category
        {
            Id = category.Id,
            Title = category.Title,
            Description = category.Description,
            ImageData = category.ImageData
        };
}
