using DevExpress.Mvvm;
using HandyControl.Tools.Extension;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Models;
using TechSupport.BusinessLogic.Models.RequestModels;
using TechSupport.BusinessLogic.Models.UserModels;
using TechSupport.UI.Mapping;
using TechSupport.UI.Models;

namespace TechSupport.UI.ViewModels;

public sealed partial class RequestsViewModel : BaseViewModel
{
    private readonly IRequestService _requestService;
    private readonly ICategoryService _categoryService;
    private readonly IDepartmentService _departmentService;
    private readonly IUserService _userService;

    private ObservableCollection<ExtendedRequest> _requests;
    public ICollectionView RequestsView { get; }

    public override string Title => "Заявки технической поддержки";

    #region Search bars

    public IEnumerable<StrRequestStatus> RequestStatuses { get; } = new List<StrRequestStatus>
    {
        new StrRequestStatus(RequestStatus.Created),
        new StrRequestStatus(RequestStatus.InProgress),
        new StrRequestStatus(RequestStatus.Paused),
        new StrRequestStatus(RequestStatus.Completed),
    };

    public string SearchText
    {
        get => GetValue<string>(nameof(SearchText));
        set => SetValue(value, () => RequestsView.Refresh(), nameof(SearchText));
    }

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

    #endregion

    public ICommand LoadViewDataCommand { get; }

    public RequestsViewModel(
        IRequestService requestService,
        ICategoryService categoryService,
        IDepartmentService departmentService,
        IUserService userService)
    {
        _requestService = requestService;
        _categoryService = categoryService;
        _departmentService = departmentService;
        _userService = userService;

        LoadViewDataCommand = new AsyncCommand(LoadView);
        RequestsView = CollectionViewSource.GetDefaultView(_requests);
    }

    private async Task LoadView()
    {
        _requests.AddRange(await _requestService.GetRequests());
        Departments = await _departmentService.GetDepartments();
        Categories = (await _categoryService.GetCategories()).MapToIcons();
        Users = await _userService.GetUsers();
    }
}
