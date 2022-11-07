using DevExpress.Mvvm;
using HandyControl.Tools.Extension;
using System;
using System.Collections;
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
using TechSupport.UI.Services;
using TechSupport.UI.ViewModels.Base;
using TechSupport.UI.ViewModels.EditViewModels;
using TechSupport.UI.Views.EditableViews;

namespace TechSupport.UI.ViewModels;

/// <summary>
/// Класс для управления заявками технической поддержки с UI
/// </summary>
public sealed class RequestsViewModel : BaseItemsViewModel<ExtendedRequest>
{
    // Серивс для работы с диалоговыси окнами
    private readonly IWindowDialogService _dialogService;
    private readonly IRequestService _requestService;
    private readonly ICategoryService _categoryService;
    private readonly IDepartmentService _departmentService;
    private readonly IUserService _userService;

    public override string Title => "Заявки технической поддержки";

    #region Search bars

    // Список доступных статусов заявки для выбора
    public IEnumerable<StrRequestStatus> RequestStatuses { get; } = new List<StrRequestStatus>
    {
        new StrRequestStatus(RequestStatus.Created),
        new StrRequestStatus(RequestStatus.InProgress),
        new StrRequestStatus(RequestStatus.Paused),
        new StrRequestStatus(RequestStatus.Completed),
    };

    // Список категорий заявки для выбора
    public IReadOnlyList<IconCategory> Categories
    {
        get => GetValue<IReadOnlyList<IconCategory>>(nameof(Categories));
        set => SetValue(value, nameof(Categories));
    }
    // Список отделов заявки для выбора
    public IReadOnlyList<Department> Departments
    {
        get => GetValue<IReadOnlyList<Department>>(nameof(Departments));
        set => SetValue(value, nameof(Departments));
    }
    // Список пользователей для выбора в заявку
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

        LoadViewDataCommand = new AsyncCommand(LoadView);

        SearchText = string.Empty;
    }

    // Метод загрузки предварительных данных после появления окна на экране
    private async Task LoadView()
    {
        await Execute(async () =>
        {
            _items.Clear();
            _items.AddRange(await _requestService.GetRequests());
            Departments = await _departmentService.GetDepartments();
            Categories = (await _categoryService.GetCategories()).MapToIcons();
            Users = await _userService.GetUsers();
            SearchRequests(default);
        });
    }

    // Проверка возможности удаления/закрытия заявки
    private bool CanTerminateRequest(ExtendedRequest er)
        => er is not null && er.RequestStatus != RequestStatus.Completed;

    // Метод удаления выбранной заявки
    private async Task RemoveRequest(ExtendedRequest extendedRequest)
    {
        await Execute(async () =>
        {
            await _requestService.Remove(extendedRequest.Id);
            _items.Remove(extendedRequest);
        });

        // Обновить коллекцию на интерфейсе
        await LoadView();
    }

    // Метод вызова окна для редактирования заявки
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
                // Обновить заявку, если есть подтверждение
                await _requestService.Update(requestViewModel.Request);
            }
        });

        // Обновить коллекцию на интерфейсе
        await LoadView();
    }

    // Метод поиска заявок по передаваемому фильтру
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

        // Вызвать обновление
        ItemsView.Refresh();
    }

    // Очистка фильтра поиска
    private async Task ClearSearchFilter(IList[] boxes)
    {
        foreach (var box in boxes)
        {
            box.Clear();
        }

        SearchText = string.Empty;

        // Обновить коллекцию на интерфейсе
        await LoadView();
    }

    private async Task CompleteRequest(ExtendedRequest extendedRequest)
    {
        await Execute(async () =>
        {
            // Вызов закрытия заявки
            await _requestService.CompleteRequest(extendedRequest.Id);
        });

        // Обновить коллекцию на интерфейсе
        await LoadView();
    }
}
