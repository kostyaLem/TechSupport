using DevExpress.Mvvm;
using HandyControl.Tools.Extension;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.UI.Helpers;
using TechSupport.UI.Mapping;
using TechSupport.UI.Models;

namespace TechSupport.UI.ViewModels;

public sealed class CategoriesViewModel : BaseViewModel
{
    private readonly ICategoryService _categoryService;

    private readonly ObservableCollection<IconCategory> _categories;

    public override string Title => "Управление категориями";

    public IconCategory SelectedCategory
    {
        get => GetValue<IconCategory>(nameof(SelectedCategory));
        set => SetValue(value, () => SelectedImage = SelectedCategory?.Image, nameof(SelectedCategory));
    }

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

        _categories = new ObservableCollection<IconCategory>();
        ItemsView = CollectionViewSource.GetDefaultView(_categories);
        ItemsView.Filter += CanFilterCategory;
    }

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

    public async Task CreateCategory()
    {
        await Execute(async () =>
        {
            await _categoryService.CreateEmpty();
            await LoadCategoories();
        });
    }

    public async Task UpdateCategory()
    {
        await Execute(async () =>
        {
            var category = SelectedCategory.Category.Recreate(SelectedImage);

            await _categoryService.Update(category);
        });

        await LoadCategoories();
    }

    public async Task RemoveCategory()
    {
        await Execute(async () =>
        {
            await _categoryService.Remove(SelectedCategory.Category.Id);
            await LoadCategoories();
        });
    }

    private async Task LoadCategoories()
    {
        await Execute(async () =>
        {
            _categories.Clear();

            var categories = await _categoryService.GetCategories();

            _categories.AddRange(categories.MapToIcons());
        });
    }

    private void RemoveImage()
    {
        SelectedImage = null;
    }

    private async Task UpdateImage()
    {
        await Execute(async () =>
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "PNG (*.png)|*.png";
            var result = dialog.ShowDialog();

            await Task.CompletedTask;

            if (result.HasValue && result.Value)
            {
                SelectedImage = ImageHelper.LoadImage(File.ReadAllBytes(dialog.FileName));
            }
        });
    }
}
