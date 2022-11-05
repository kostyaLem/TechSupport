using DevExpress.Mvvm;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Models.UserModels;
using TechSupport.UI.Mapping;
using TechSupport.UI.Services;
using TechSupport.UI.ViewModels.EditViewModels;
using TechSupport.UI.Views.EditableViews;

namespace TechSupport.UI.ViewModels;

public sealed class AdministrationViewModel : BaseViewModel
{
    private readonly IUserService _userService;
    private readonly IWindowDialogService _dialogService;

    private ObservableCollection<User> _users;

    public override string Title => "Управление пользователями";

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

        _users = new ObservableCollection<User>();
        ItemsView = CollectionViewSource.GetDefaultView(_users);
        ItemsView.Filter += CanFilterUser;
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

    private async Task LoadUsers()
    {
        await Execute(async () =>
        {
            _users.Clear();
            var users = await _userService.GetUsers();
            _users.AddRange(users);
        });
    }

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
                var user = userViewModel.User.MapToCreateRequest(userViewModel.Password);
                await _userService.Create(user);

                await LoadUsers();
            }
        });
    }

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
                await _userService.Update(userViewModel.User, userViewModel.Password);

                await LoadUsers();
            }
        });
    }

    private async Task RemoveUser()
    {
        await Execute(async () =>
        {
            await _userService.Remove(SelectedUser.Id);
            await LoadUsers();
        });
    }
}
