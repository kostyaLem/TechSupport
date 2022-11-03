using DevExpress.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Models;
using TechSupport.BusinessLogic.Models.RequestModels;
using TechSupport.BusinessLogic.Models.UserModels;
using TechSupport.UI.Mapping;
using TechSupport.UI.Models;

namespace TechSupport.UI.ViewModels.EditViewModels;

public class EditRequestViewModel : BaseViewModel
{
    private readonly ICategoryService _categoryService;
    private readonly IDepartmentService _departmentService;
    private readonly IUserService _userService;

    public IEnumerable<StrRequestStatus> RequestStatuses { get; } = new List<StrRequestStatus>
    {
        new StrRequestStatus(RequestStatus.Created),
        new StrRequestStatus(RequestStatus.InProgress),
        new StrRequestStatus(RequestStatus.Paused),
        new StrRequestStatus(RequestStatus.Completed),
    };

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
    public IReadOnlyList<User> Users
    {
        get => GetValue<IReadOnlyList<User>>(nameof(Users));
        set => SetValue(value, nameof(Users));
    }

    public IconCategory SelectedCategory
    {
        get => GetValue<IconCategory>(nameof(SelectedCategory));
        set => SetValue(value, () => Request.Category = value.Category, nameof(SelectedCategory));
    }

    public StrRequestStatus SelectedStatus
    {
        get => GetValue<StrRequestStatus>(nameof(SelectedStatus));
        set => SetValue(value, () => Request.RequestStatus = value.RequestStatus, nameof(SelectedStatus));
    }

    public ExtendedRequest Request { get; }

    public ICommand LoadViewDataCommand { get; }

    public EditRequestViewModel(
        ExtendedRequest request,
        ICategoryService categoryService,
        IDepartmentService departmentService,
        IUserService userService)
    {
        Request = request;

        _categoryService = categoryService;
        _departmentService = departmentService;
        _userService = userService;

        LoadViewDataCommand = new AsyncCommand(LoadView);
    }

    private async Task LoadView()
    {
        await Execute(async () =>
        {
            Departments = await _departmentService.GetDepartments();
            Categories = (await _categoryService.GetCategories()).MapToIcons();
            Users = await _userService.GetUsers();

            SelectedCategory = Categories.First(x => x.Category.Id == Request.Category.Id);
            SelectedStatus = RequestStatuses.First(x => x.RequestStatus == Request.RequestStatus);
        });
    }
}
