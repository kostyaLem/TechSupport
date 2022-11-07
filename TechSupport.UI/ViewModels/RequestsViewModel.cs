using DevExpress.Mvvm;
using HandyControl.Tools.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Models;
using TechSupport.BusinessLogic.Models.RequestModels;
using TechSupport.BusinessLogic.Models.UserModels;
using TechSupport.UI.Mapping;
using TechSupport.UI.Models;
using TechSupport.UI.Services;
using TechSupport.UI.ViewModels.EditViewModels;
using TechSupport.UI.Views.EditableViews;

namespace TechSupport.UI.ViewModels;

public sealed class RequestsViewModel : BaseViewModel
{
    private readonly IWindowDialogService _dialogService;
    private readonly IRequestService _requestService;
    private readonly ICategoryService _categoryService;
    private readonly IDepartmentService _departmentService;
    private readonly IUserService _userService;

    private ObservableCollection<ExtendedRequest> _requests;

    public override string Title => "Заявки технической поддержки";

    #region Search bars

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

    #endregion

    public ICommand LoadViewDataCommand { get; }
    public ICommand RemoveRequestCommand { get; }
    public ICommand UpdateRequestCommand { get; }
    public ICommand SearchRequestsCommand { get; }
    public ICommand ClearSearchFilterCommand { get; }
    public ICommand CompleteRequestCommand { get; }

    public RequestsViewModel(
        IWindowDialogService dialogService,
        IRequestService requestService,
        ICategoryService categoryService,
        IDepartmentService departmentService,
        IUserService userService)
    {
        _dialogService = dialogService;
        _requestService = requestService;
        _categoryService = categoryService;
        _departmentService = departmentService;
        _userService = userService;

        RemoveRequestCommand = new AsyncCommand<ExtendedRequest>(RemoveRequest, CanTerminateRequest);
        UpdateRequestCommand = new AsyncCommand<ExtendedRequest>(UodateRequest, CanTerminateRequest);
        SearchRequestsCommand = new AsyncCommand<RequestFilter>(SearchRequests);
        ClearSearchFilterCommand = new AsyncCommand<IList[]>(ClearSearchFilter);
        CompleteRequestCommand = new AsyncCommand<ExtendedRequest>(CompleteRequest, CanTerminateRequest);

        _requests = new ObservableCollection<ExtendedRequest>();
        LoadViewDataCommand = new AsyncCommand(LoadView);
        ItemsView = CollectionViewSource.GetDefaultView(_requests);

        SearchText = string.Empty;
    }

    private async Task LoadView()
    {
        await Execute(async () =>
        {
            _requests.Clear();
            _requests.AddRange(await _requestService.GetRequests());
            Departments = await _departmentService.GetDepartments();
            Categories = (await _categoryService.GetCategories()).MapToIcons();
            Users = await _userService.GetUsers();
            SearchRequests(default);
        });
    }

    private bool CanTerminateRequest(ExtendedRequest er)
        => er is not null && er.RequestStatus != RequestStatus.Completed;

    private async Task RemoveRequest(ExtendedRequest extendedRequest)
    {
        await Execute(async () =>
        {
            await _requestService.Remove(extendedRequest.Id);
            _requests.Remove(extendedRequest);
        });

        await LoadView();
    }

    private async Task UodateRequest(ExtendedRequest extendedRequest)
    {
        await Execute(async () =>
        {
            var requestViewModel = new EditRequestViewModel(
                extendedRequest,
                _categoryService,
                _departmentService,
                _userService);

            var result = _dialogService.ShowDialog(
                "Редактирование заявки",
                typeof(EditRequestPage),
                requestViewModel);

            if (result == Models.DialogResult.OK)
            {
                await _requestService.Update(requestViewModel.Request);
            }
        });

        await LoadView();
    }

    private async Task SearchRequests(RequestFilter filter)
    {
        ItemsView.Filter = x =>
        {
            var isValid = true;
            var request = x as ExtendedRequest;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                isValid &= (request.Title + request.Description)
                .Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            }

            if (filter is null)
            {
                return isValid;
            }

            var users = filter.Users.SelectedItems.Cast<User>().ToList();
            var categories = filter.Categories.SelectedItems.Cast<IconCategory>().ToList();
            var department = filter.Departments.SelectedItems.Cast<Department>().ToList();
            var status = filter.RequestStatuses.SelectedItems.Cast<StrRequestStatus>().ToList();


            if (users.Count > 0)
            {
                isValid &= users.Exists(u => u.Id == request.User?.Id);
            }

            if (categories.Count > 0)
            {
                isValid &= categories.Exists(u => u.Category.Id == request.Category.Id);
            }

            if (department.Count > 0)
            {
                isValid &= department.Exists(u => u.Id == request.Department.Id);
            }

            if (status.Count > 0)
            {
                isValid &= status.Exists(u => u.RequestStatus == request.RequestStatus);
            }

            return isValid;
        };

        ItemsView.Refresh();
    }

    private async Task ClearSearchFilter(IList[] boxes)
    {
        foreach (var boox in boxes)
        {
            boox.Clear();
        }

        SearchText = string.Empty;

        await LoadView();
    }

    private async Task CompleteRequest(ExtendedRequest extendedRequest)
    {
        await Execute(async () =>
        {
            await _requestService.CompleteRequest(extendedRequest.Id);
        });

        await LoadView();
    }
}
