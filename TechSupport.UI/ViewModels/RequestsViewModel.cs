using DevExpress.Mvvm;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Models;
using TechSupport.BusinessLogic.Models.RequestModels;
using TechSupport.BusinessLogic.Models.UserModels;
using TechSupport.UI.Mapping;
using TechSupport.UI.Models;
using MessageBox = HandyControl.Controls.MessageBox;

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
    public ICommand RemoveRequestCommand { get; }
    public ICommand UpdateRequestCommand { get; }
    public ICommand SearchRequestsCommand { get; }
    public ICommand ClearSearchFilterCommand { get; }
    public ICommand CompleteRequestCommand { get; }

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

        RemoveRequestCommand = new AsyncCommand<ExtendedRequest>(RemoveRequest, CanTerminateRequest);
        UpdateRequestCommand = new AsyncCommand<ExtendedRequest>(EditRequest, CanTerminateRequest);
        SearchRequestsCommand = new AsyncCommand<RequestFilter>(SearchRequests);
        ClearSearchFilterCommand = new AsyncCommand<IList[]>(ClearSearchFilter);
        CompleteRequestCommand = new AsyncCommand<ExtendedRequest>(CompleteRequest, CanTerminateRequest);

        _requests = new ObservableCollection<ExtendedRequest>();
        LoadViewDataCommand = new AsyncCommand(LoadView);
        RequestsView = CollectionViewSource.GetDefaultView(_requests);
    }

    private async Task LoadView()
    {
        try
        {
            _requests.AddRange(await _requestService.GetRequests());
            Departments = await _departmentService.GetDepartments();
            Categories = (await _categoryService.GetCategories()).MapToIcons();
            Users = await _userService.GetUsers();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private bool CanTerminateRequest(ExtendedRequest er)
        => er is not null && er.RequestStatus != RequestStatus.Completed;

    private async Task RemoveRequest(ExtendedRequest extendedRequest)
    {
        await _requestService.Remove(extendedRequest.Id);
        _requests.Remove(extendedRequest);
    }

    private async Task EditRequest(ExtendedRequest extendedRequest)
    {
        await Task.CompletedTask;
    }

    private async Task SearchRequests(RequestFilter filter)
    {

    }

    private async Task ClearSearchFilter(IList[] boxes)
    {
        foreach (var boox in boxes)
        {
            boox.Clear();
        }

        SearchText = string.Empty;
    }

    private async Task CompleteRequest(ExtendedRequest extendedRequest)
    {
        await _requestService.CompleteRequest(extendedRequest.Id);
        RaisePropertiesChanged(nameof(extendedRequest.RequestStatus));
    }
}
