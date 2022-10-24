using DevExpress.Mvvm;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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

    public override string Title => "Управления пользователями";

    public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();

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
        CreateUserCommand = new AsyncCommand(CreateUser);
        RemoveUserCommand = new AsyncCommand(RemoveUser, () => SelectedUser is not null);
    }

    private async Task LoadUsers()
    {
        Users.Clear();
        var users = await _userService.GetUsers();
        Users.AddRange(users);
    }

    private async Task CreateUser()
    {
        var userViewModel = new EditCustomerViewModel();

        var result = _dialogService.ShowDialog(
            "Создание нового пользователя",
            typeof(EditCustomerPage),
            userViewModel);

        if (result == Models.DialogResult.OK)
        {
            try
            {
                var user = userViewModel.User.MapToCreateRequest(userViewModel.Password);
                await _userService.Create(user);

                await LoadUsers();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }

    private async Task RemoveUser()
    {
        await _userService.Remove(SelectedUser.Id);
    }
}
