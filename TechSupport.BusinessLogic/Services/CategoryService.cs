using Microsoft.EntityFrameworkCore;
using TechSupport.BusinessLogic.Exceptions;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Mapping;
using TechSupport.BusinessLogic.Models;
using TechSupport.DataAccess.Context;
using Domain = TechSupport.DataAccess.Models;

namespace TechSupport.BusinessLogic.Services;

/// <summary>
/// Сервис управления категориями технической поддержки
/// </summary>
internal sealed class CategoryService : ICategoryService
{
    private readonly TechSupportContext _context;

    public CategoryService(TechSupportContext context)
    {
        _context = context;
    }

    // Получить категорию по Id
    public async Task<Category> GetById(int categoryId)
    {
        var category = await GetCategory(categoryId);

        // Преобразование модели
        return category.ToBl();
    }

    // Получить список категорий
    public async Task<IReadOnlyList<Category>> GetCategories()
    {
        var categories = await _context.RequestCategories.ToListAsync();

        // Преобразование модели
        return categories.Select(x => x.ToBl()).ToList();
    }

    // Удалить категорию по Id
    public async Task Remove(int categoryId)
    {
        var category = await GetCategory(categoryId);

        // Пометить сущность на удаление
        _context.RequestCategories.Remove(category);
        // Сохранить изменения в базе
        await _context.SaveChangesAsync();
    }

    // Создать пустую категорию для дальнейшего обновления
    public async Task CreateEmpty()
    {
        // Добавление новой категории в базу
        _context.RequestCategories.Add(new Domain.RequestCategory
        {
            Title = $"Категория {DateTime.Now}"
        });

        // Сохранить изменения в базе
        await _context.SaveChangesAsync();
    }

    // Обновить выбранную категорию
    public async Task Update(Category category)
    {
        var existingCategory = await GetCategory(category.Id);

        // Обновление некоторых полей сущности
        existingCategory.Title = category.Title;
        existingCategory.Description = category.Description;
        existingCategory.ImageData = category.ImageData;

        // Сохранить изменения в базе
        await _context.SaveChangesAsync();
    }

    // Получить категорию по Id и выбросить ошибку, если сущности нет
    private async Task<Domain.RequestCategory> GetCategory(int entityId)
    {
        // Поиск сущности в базе по Id
        var caategory = await _context.RequestCategories.FindAsync(entityId);

        if (caategory is null)
        {
            // Выбросить исключение, если сущность не найдена
            throw new NotFoundException("Категория не найдена.");
        }

        return caategory;
    }
}
