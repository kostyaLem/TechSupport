using DevExpress.Mvvm;
using HandyControl.Tools.Extension;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Models.UserModels;
using TechSupport.UI.ViewModels.EditViewModels;
using TechSupport.UI.Views.EditableViews;

namespace TechSupport.UI.ViewModels;

public sealed class AdministrationViewModel : BaseViewModel
{
    private readonly IUserService _userService;

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

    public AdministrationViewModel(IUserService userService)
    {
        _userService = userService;

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
        var editViewModel = new EditCustomerViewModel();
        var editView = new EditView(new EditCustomerPage(editViewModel));
        var windiowResult = editView.ShowDialog();

        if (windiowResult.HasValue)
        {
            if (editView.DialogResult == Models.DialogResult.OK)
            {

            }
        }
    }

    private async Task RemoveUser()
    {
        await _userService.Remove(SelectedUser.Id);
    }
}
