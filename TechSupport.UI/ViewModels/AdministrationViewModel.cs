using DevExpress.Mvvm;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Models.UserModels;
using TechSupport.UI.Mapping;
using TechSupport.UI.Services;
using TechSupport.UI.ViewModels.Base;
using TechSupport.UI.ViewModels.EditViewModels;
using TechSupport.UI.Views.EditableViews;

namespace TechSupport.UI.ViewModels;

/// <summary>
/// Класс для работы с пользователями в системе с UI
/// </summary>
public sealed class AdministrationViewModel : BaseItemsViewModel<User>
{
    private readonly IUserService _userService;
    // Серивс для работы с диалоговыси окнами
    private readonly IWindowDialogService _dialogService;

    public override string Title => "Управление пользователями";

    // Выбранный пользователь в таблице
    public User SelectedUser
    {
        get => GetValue<User>(nameof(SelectedUser));
        set => SetValue(value, nameof(SelectedUser));
    }

    public ICommand LoadViewDataCommand { get; }
    public ICommand CreateUserCommand { get; }
    public ICommand EditUserCommand { get; }
    public ICommand RemoveUserCommand { get; }

    public AdministrationViewModel(IUserService userService, IWindowDialogService dialogService)
    {
        _userService = userService;
        _dialogService = dialogService;

        LoadViewDataCommand = new AsyncCommand(LoadUsers);
        CreateUserCommand = new AsyncCommand(CreateUser, () => App.IsAdmin);
        EditUserCommand = new AsyncCommand(EditUser, () => SelectedUser is not null && App.IsAdmin);
        RemoveUserCommand = new AsyncCommand(RemoveUser, () => SelectedUser is not null && App.IsAdmin);

        ItemsView.Filter = CanFilterUser;
    }

    private bool CanFilterUser(object obj)
    {
        if (SearchText is { } && obj is User user)
        {
            var predicates = new List<string>
            {
                user.Login,
                user.FirstName,
                user.LastName,
                user.Birthday.ToString(),
                user.Phone,
                user.Email,
                $"{user.FirstName} {user.LastName}"
            };

            return predicates.Any(x => x.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        return true;
    }

    // Метод загрузки предварительных данных после появления окна на экране
    private async Task LoadUsers()
    {
        await Execute(async () =>
        {
            _items.Clear();
            var users = await _userService.GetUsers();
            _items.AddRange(users);
        });
    }

    // Метод вызова окна для создания пользователя в системе
    private async Task CreateUser()
    {
        await Execute(async () =>
        {
            var userViewModel = new EditCustomerViewModel();

            var result = _dialogService.ShowDialog(
                "Создание нового пользователя",
                typeof(EditCustomerPage),
                userViewModel);

            if (result == Models.DialogResult.OK)
            {
                // Подготовка данных
                var user = userViewModel.User.MapToCreateRequest(userViewModel.Password);
                // Вызов создания пользователя в сервисе
                await _userService.Create(user);

                // Обновить коллекцию на интерфейсе
                await LoadUsers();
            }
        });
    }

    // Метод вызова окна для редактирования пользователя
    private async Task EditUser()
    {
        await Execute(async () =>
        {
            var user = await _userService.GetUserById(SelectedUser.Id);
            var userViewModel = new EditCustomerViewModel(user);

            var result = _dialogService.ShowDialog(
                "Редактирование пользователя",
                typeof(EditCustomerPage),
                userViewModel);

            if (result == Models.DialogResult.OK)
            {
                // Вызвать обновление пользователя, если есть подтверждение
                await _userService.Update(userViewModel.User, userViewModel.Password);

                // Обновить коллекцию на интерфейсе
                await LoadUsers();
            }
        });
    }

    // Меотд удаления выбранного пользователя
    private async Task RemoveUser()
    {
        await Execute(async () =>
        {
            // Вызвать удаление пользователя в сервисе
            await _userService.Remove(SelectedUser.Id);

            // Обновить коллекцию на интерфейсе
            await LoadUsers();
        });
    }
}
