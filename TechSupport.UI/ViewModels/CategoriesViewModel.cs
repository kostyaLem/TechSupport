using DevExpress.Mvvm;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

    public ICollectionView CategoriesView { get; set; }

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

    public string SearchText
    {
        get => GetValue<string>(nameof(SearchText));
        set => SetValue(value, () => CategoriesView.Refresh(), nameof(SearchText));
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
        CreateCategoryCommand = new AsyncCommand(CreateCategory);
        UpdateCategoryCommand = new AsyncCommand(UpdateCategory, () => SelectedCategory is not null);
        RemoveCategoryCommand = new AsyncCommand(RemoveCategory, () => SelectedCategory is not null);

        RemoveImageCommand = new DelegateCommand(RemoveImage);
        UpdateImageCommand = new DelegateCommand(UpdateImage);

        _categories = new ObservableCollection<IconCategory>();
        CategoriesView = CollectionViewSource.GetDefaultView(_categories);
        CategoriesView.Filter += CanFilterCategory;
    }

    private bool CanFilterCategory(object obj)
    {
        if (SearchText is { } && obj is IconCategory category)
        {
            var predicates = new List<string>
            {
                category.Category.Title,
                category.Category.Description
            };

            return predicates.Any(x => x.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        return true;
    }

    public async Task CreateCategory()
    {
        try
        {
            await _categoryService.CreateEmpty();
            await LoadCategoories();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public async Task UpdateCategory()
    {
        var category = SelectedCategory.Category.Recreate(SelectedImage);

        await _categoryService.Update(category);

        await LoadCategoories();
    }

    public async Task RemoveCategory()
    {
        await _categoryService.Remove(SelectedCategory.Category.Id);
        await LoadCategoories();
    }

    private async Task LoadCategoories()
    {
        _categories.Clear();

        var categories = await _categoryService.GetCategories();
        var iconCategories = categories.Select(x =>
        {
            return new IconCategory
            {
                Category = x,
                Image = x.ImageData is not null ? ImageHelper.LoadImage(x.ImageData) : null
            };

        }).ToList();

        _categories.AddRange(iconCategories);
    }

    private void RemoveImage()
    {
        SelectedImage = null;
    }

    private void UpdateImage()
    {
        var dialog = new OpenFileDialog();
        dialog.Filter = "PNG (*.png)|*.png";
        var result = dialog.ShowDialog();

        if (result.HasValue && result.Value)
        {
            SelectedImage = ImageHelper.LoadImage(File.ReadAllBytes(dialog.FileName));
        }
    }
}
