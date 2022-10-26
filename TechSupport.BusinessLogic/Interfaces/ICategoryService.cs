using TechSupport.BusinessLogic.Models;

namespace TechSupport.BusinessLogic.Interfaces;

public interface ICategoryService
{
    Task<IReadOnlyList<Category>> GetCategories();
    Task<Category> GetById(int categoryId);
    Task Update(Category category);
    Task Remove(int categoryId);
    Task CreateEmpty();
}
