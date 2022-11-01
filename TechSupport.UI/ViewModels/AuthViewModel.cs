using DevExpress.Mvvm;
using HandyControl.Controls;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TechSupport.BusinessLogic.Exceptions;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.UI.Views;
using PasswordBox = HandyControl.Controls.PasswordBox;

namespace TechSupport.UI.ViewModels;

public sealed class AuthViewModel : BaseViewModel
{
    private readonly IAuthorizationService _authService;
    private readonly MainView _mainView;

    public override string Title => "Авторизация";

    public string Login
    {
        get => GetValue<string>(nameof(Login));
        set => SetValue(value, nameof(Login));
    }

    public ICommand LoginCommand { get; }

    public AuthViewModel(IAuthorizationService authSerivce, MainView mainView)
    {
        _authService = authSerivce;
        _mainView = mainView;
        LoginCommand = new AsyncCommand<object>(TryLogin, x => !string.IsNullOrWhiteSpace(Login));
    }

    public async Task TryLogin(object passwordControl)
    {
        await Execute(async () =>
        {
            var pswrdBox = (PasswordBox)passwordControl;

            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(pswrdBox.Password))
            {
                MessageBox.Error("Заполните все поля.", "Ошибка авторизации");
                return;
            }

            try
            {
                var user = await _authService.Authorize(Login, pswrdBox.Password);
                App.CurrentUser = user;
                App.Current.Windows[1].Close();
                _mainView.Show();
            }
            catch (AuthorizeException)
            {
                MessageBox.Error("Неверный логин или пароль.", "Ошибка авторизации");
            }
            catch (UserNotFoundAuthorizeException)
            {
                MessageBox.Error("Пользователь с таким логином и паролем не существует.", "Ошибка авторизации");
            }
            catch (Exception e)
            {
                MessageBox.Error(e.Message, "Внутренняя ошибка");
            }
        });
    }
}
