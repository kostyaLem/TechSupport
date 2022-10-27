using DevExpress.Mvvm;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Models;
using TechSupport.UI.Mapping;
using TechSupport.UI.Models;

namespace TechSupport.UI.ViewModels;

public sealed class RequestCreationViewModel : BaseViewModel
{
    private readonly IRequestService _requestService;
    private readonly ICategoryService _categoryService;
    private readonly IDepartmentService _departmentService;

    public override string Title => "Создание заявки";

    public SlimRequest Request { get; private set; }

    public IReadOnlyList<IconCategory> Categories
    {
        get => GetValue<IReadOnlyList<IconCategory>>(nameof(Categories));
        set => SetValue(value, nameof(Categories));
    }
    public IReadOnlyList<Department> Departments
    {
        get => GetValue<IReadOnlyList<Department>>(nameof(Departments));
        set => SetValue(value, nameof(Departments));
    }

    public IconCategory SelectedCategory
    {
        get => GetValue<IconCategory>(nameof(SelectedCategory));
        set => SetValue(value, nameof(SelectedCategory));
    }

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

    private async Task CreateRequest()
    {
        await _requestService.Create(Request.MapToCreateRequest());
    }

    private void SelectCategory(IconCategory category)
    {
        SelectedCategory = category;
    }

    private async Task LoadView()
    {
        Departments = await _departmentService.GetDepartments();
        Categories = (await _categoryService.GetCategories()).MapToIcons();
    }
}
