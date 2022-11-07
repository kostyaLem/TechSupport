using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using HandyControl.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TechSupport.BusinessLogic.Exceptions;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.UI.ViewModels.Base;
using TechSupport.UI.Views;
using PasswordBox = HandyControl.Controls.PasswordBox;

namespace TechSupport.UI.ViewModels;

/// <summary>
/// Класс авторизации пользователя в системе с UI
/// </summary>
public sealed class AuthViewModel : BaseViewModel
{
    private readonly IAuthorizationService _authService;
    private readonly IServiceProvider _serviceProvider;

    public override string Title => "Авторизация";

    // Логин пользователя
    public string Login
    {
        get => GetValue<string>(nameof(Login));
        set => SetValue(value, nameof(Login));
    }

    public ICommand LoginCommand { get; }

    public AuthViewModel(IAuthorizationService authSerivce, IServiceProvider serviceProvider)
    {
        _authService = authSerivce;
        _serviceProvider = serviceProvider;
        LoginCommand = new AsyncCommand<object>(TryLogin, x => !string.IsNullOrWhiteSpace(Login));
    }

    // Метод авторизации пользователя
    public async Task TryLogin(object passwordControl)
    {
        await Execute(async () =>
        {
            var pswrdBox = (PasswordBox)passwordControl;

            // Проверка полей
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(pswrdBox.Password))
            {
                MessageBox.Error("Заполните все поля.", "Ошибка авторизации");
                return;
            }

            try
            {
                // Попытка авторизовать пользователя
                var user = await _authService.Authorize(Login, pswrdBox.Password);
                // Установка текущего пользователя
                App.CurrentUser = user;
                // Отображение окна с меню
                _serviceProvider.GetRequiredService<MainView>().Show();
                // Закрыть окно авторизации
                App.Current.Windows[0].Close();
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
