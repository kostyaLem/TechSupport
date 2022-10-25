using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using DevExpress.Mvvm;
using HandyControl.Tools.Extension;
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
using TechSupport.BusinessLogic.Models.CategoriesModels;
using TechSupport.UI.Models;

namespace TechSupport.UI.ViewModels;

public sealed class CategoriesViewModel : BaseViewModel
{
    private readonly ICategoryService _categoryService;

    private readonly ObservableCollection<IconCategory> _categories;

    public ICollectionView CategoriesView { get; }

    public override string Title => "Управление категориями";

    public IconCategory SelectedCategory { get; }

    public string SearchText
    {
        get => GetValue<string>(nameof(SearchText));
        set => SetValue(value, () => CategoriesView.Refresh(), nameof(SearchText));
    }

    public ICommand LoadViewDataCommand { get; }
    public ICommand CreateCategoryCommand { get; }
    public ICommand UpdateCategoryCommand { get; }
    public ICommand RemoveCategoryCommand { get; }

    public CategoriesViewModel(ICategoryService categoryService)
    {
        _categoryService = categoryService;

        LoadViewDataCommand = new AsyncCommand(LoadCategoories);
        CreateCategoryCommand = new AsyncCommand(CreateCategory);
        UpdateCategoryCommand = new AsyncCommand(UpdateCategory);

        CategoriesView = CollectionViewSource.GetDefaultView(_categories);
        CategoriesView.Filter += CanFilterCategory;
    }

    private bool CanFilterCategory(object obj)
    {
        if (SearchText is { } && obj is SlimCategory category)
        {
            var predicates = new List<string>
            {
                category.Title,
                category.Description
            };

            return predicates.Any(x => x.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        return true;
    }

    private async Task LoadCategoories()
    {
        _categories.Clear();

        var categories = await _categoryService.GetCategories();
        var iconCategories = categories.Select(x =>
        {
            using (var stream = new MemoryStream(x.ImageData))
            {
                var image = BitmapFrame.Create(
                    stream,
                    BitmapCreateOptions.None,
                    BitmapCacheOption.OnLoad);

                return new IconCategory
                {
                    Category = x,
                    Image = image
                };
            }
        }).ToList();

        _categories.AddRange(iconCategories);
    }
}
