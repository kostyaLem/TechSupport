using Microsoft.EntityFrameworkCore;
using TechSupport.BusinessLogic.Exceptions;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Mapping;
using TechSupport.BusinessLogic.Models;
using TechSupport.DataAccess.Context;
using Domain = TechSupport.DataAccess.Models;

namespace TechSupport.BusinessLogic.Services;

internal sealed class CategoryService : ICategoryService
{
    private readonly TechSupportContext _context;

    public CategoryService(TechSupportContext context)
    {
        _context = context;
    }

    public async Task<Category> GetById(int categoryId)
    {
        var category = await GetCategory(categoryId);

        return category.ToBl();
    }

    public async Task<IReadOnlyList<Category>> GetCategories()
    {
        var categories = await _context.RequestCategories
            .AsNoTracking()
            .ToListAsync();

        return categories.Select(x => x.ToBl()).ToList();
    }

    public async Task Remove(int categoryId)
    {
        var category = await GetCategory(categoryId);

        _context.RequestCategories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task CreateEmpty()
    {
        _context.RequestCategories.Add(new Domain.RequestCategory
        {
            Title = $"Категория {DateTime.Now}"
        });

        await _context.SaveChangesAsync();
    }

    public async Task Update(Category category)
    {
        var existingCategory = await GetCategory(category.Id);

        existingCategory.Title = category.Title;
        existingCategory.Description = category.Description;
        existingCategory.ImageData = category.ImageData;

        await _context.SaveChangesAsync();
    }

    private async Task<Domain.RequestCategory> GetCategory(int entityId)
    {
        var caategory = await _context.RequestCategories.FindAsync(entityId);

        if (caategory is null)
        {
            throw new NotFoundException("Категория не найдена.");
        }

        return caategory;
    }
}
