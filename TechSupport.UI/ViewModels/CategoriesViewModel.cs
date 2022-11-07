using DevExpress.Mvvm;
using HandyControl.Tools.Extension;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.UI.Helpers;
using TechSupport.UI.Mapping;
using TechSupport.UI.Models;
using TechSupport.UI.ViewModels.Base;

namespace TechSupport.UI.ViewModels;

/// <summary>
/// Класс управления категориями с UI
/// </summary>
public sealed class CategoriesViewModel : BaseItemsViewModel<IconCategory>
{
    private readonly ICategoryService _categoryService;

    public override string Title => "Управление категориями";

    // Выбранная категория
    public IconCategory SelectedCategory
    {
        get => GetValue<IconCategory>(nameof(SelectedCategory));
        set => SetValue(value, () => SelectedImage = SelectedCategory?.Image, nameof(SelectedCategory));
    }

    // Изображение выбранной категории
    public BitmapImage SelectedImage
    {
        get => GetValue<BitmapImage>(nameof(SelectedImage));
        set => SetValue(value, nameof(SelectedImage));
    }

    public ICommand LoadViewDataCommand { get; }
    public ICommand CreateCategoryCommand { get; }
    public ICommand UpdateCategoryCommand { get; }
    public ICommand RemoveCategoryCommand { get; }

    public ICommand RemoveImageCommand { get; }
    public ICommand UpdateImageCommand { get; }

    public CategoriesViewModel(ICategoryService categoryService)
    {
        _categoryService = categoryService;

        LoadViewDataCommand = new AsyncCommand(LoadCategoories);
        CreateCategoryCommand = new AsyncCommand(CreateCategory, () => App.IsAdmin);
        UpdateCategoryCommand = new AsyncCommand(UpdateCategory, () => SelectedCategory is not null && App.IsAdmin);
        RemoveCategoryCommand = new AsyncCommand(RemoveCategory, () => SelectedCategory is not null && App.IsAdmin);

        RemoveImageCommand = new DelegateCommand(RemoveImage, () => App.IsAdmin);
        UpdateImageCommand = new AsyncCommand(UpdateImage, () => App.IsAdmin);

        ItemsView.Filter += CanFilterCategory;
    }

    // Фильтр поиска категории
    private bool CanFilterCategory(object obj)
    {
        if (SearchText is { } && obj is IconCategory category)
        {
            var predicates = new List<string>
            {
                category.Category.Title,
                category.Category.Description ?? string.Empty,
            };

            return predicates.Any(x => x.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        return true;
    }

    // Метод создания пустой категории
    public async Task CreateCategory()
    {
        await Execute(async () =>
        {
            // Вызов создаания категории
            await _categoryService.CreateEmpty();

            // Обновить коллекцию на интерфейсе
            await LoadCategoories();
        });
    }

    // Метод обновления выбранной категории
    public async Task UpdateCategory()
    {
        await Execute(async () =>
        {
            // Подготовка данных для обновления
            var category = SelectedCategory.Category.Recreate(SelectedImage);
            // Вызов обновления категории
            await _categoryService.Update(category);
        });

        // Обновить коллекцию на интерфейсе
        await LoadCategoories();
    }

    // Метод удаления выбранной категории
    public async Task RemoveCategory()
    {
        await Execute(async () =>
        {
            // Удалить выбранную категорию
            await _categoryService.Remove(SelectedCategory.Category.Id);

            // Обновить коллекцию на интерфейсе
            await LoadCategoories();
        });
    }

    // Метод загрузки предварительных данных после появления окна на экране
    private async Task LoadCategoories()
    {
        await Execute(async () =>
        {
            _items.Clear();
            var categories = await _categoryService.GetCategories();
            _items.AddRange(categories.MapToIcons());
        });
    }

    // Очистить иконку изображения
    private void RemoveImage()
    {
        SelectedImage = null;
    }

    // Загрузить в иконку новое изображение
    private async Task UpdateImage()
    {
        await Execute(async () =>
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "PNG (*.png)|*.png";
            // Открыть диалоговое окно выбора изображения
            var result = dialog.ShowDialog();

            await Task.CompletedTask;

            if (result.HasValue && result.Value)
            {
                // Загрузить картинку в иконку, если есть подтверждение
                SelectedImage = ImageHelper.LoadImage(File.ReadAllBytes(dialog.FileName));
            }
        });
    }
}
