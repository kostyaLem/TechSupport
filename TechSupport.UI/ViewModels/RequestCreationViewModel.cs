using DevExpress.Mvvm;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Models;
using TechSupport.UI.Mapping;
using TechSupport.UI.Models;
using TechSupport.UI.Views;

namespace TechSupport.UI.ViewModels;

/// <summary>
/// Класс для создания заявки технической поддержки
/// </summary>
public sealed class RequestCreationViewModel : BaseViewModel
{
    private readonly IRequestService _requestService;
    private readonly ICategoryService _categoryService;
    private readonly IDepartmentService _departmentService;

    public override string Title => "Создание заявки";

    // Заявка, отображаемая на UI
    public SlimRequest Request { get; private set; }

    // Список категорий для выбора
    public IReadOnlyList<IconCategory> Categories
    {
        get => GetValue<IReadOnlyList<IconCategory>>(nameof(Categories));
        set => SetValue(value, nameof(Categories));
    }
    // Список отделов для выбора
    public IReadOnlyList<Department> Departments
    {
        get => GetValue<IReadOnlyList<Department>>(nameof(Departments));
        set => SetValue(value, nameof(Departments));
    }

    // Выбраянная категория
    public IconCategory SelectedCategory
    {
        get => GetValue<IconCategory>(nameof(SelectedCategory));
        set => SetValue(value, nameof(SelectedCategory));
    }

    // Выбранный отдел
    public Department SelectedDepartment
    {
        get => GetValue<Department>(nameof(SelectedDepartment));
        set => SetValue(value, nameof(SelectedDepartment));
    }

    public ICommand LoadViewDataCommand { get; }
    public ICommand CreateRequestCommand { get; }
    public ICommand SelectCategoryCommand { get; }

    public RequestCreationViewModel(
        IRequestService requestService,
        ICategoryService categoryService,
        IDepartmentService departmentService)
    {
        _requestService = requestService;
        _categoryService = categoryService;
        _departmentService = departmentService;

        Request = new SlimRequest();

        LoadViewDataCommand = new AsyncCommand(LoadView);
        CreateRequestCommand = new AsyncCommand(CreateRequest);
        SelectCategoryCommand = new DelegateCommand<IconCategory>(SelectCategory);
    }

    // Метод сбора введённых данных и создания заявки в системе
    private async Task CreateRequest()
    {
        await Execute(async () =>
        {
            // Установка выбранных данных
            Request.Category = SelectedCategory.Category;
            Request.Department = SelectedDepartment;

            // Вызов создания заявки в сервисе
            await _requestService.Create(Request.MapToCreateRequest());

            // Закрытие окна создания заявки
            foreach(var window in App.Current.Windows)
            {
                if (window is RequestCreationView view)
                {
                    view.Close();
                    return;
                }
            }
        });
    }

    private void SelectCategory(IconCategory category)
    {
        SelectedCategory = category;
    }

    // Метод загрузки предварительных данных после появления окна на экране
    private async Task LoadView()
    {
        await Execute(async () =>
        {
            Departments = await _departmentService.GetDepartments();
            Categories = (await _categoryService.GetCategories()).MapToIcons();
        });
    }
}
