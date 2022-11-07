using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Models.UserModels;
using TechSupport.UI.Models;
using TechSupport.UI.Services;
using TechSupport.UI.ViewModels;
using TechSupport.UI.Views;

namespace TechSupport.UI;

public static class Configuration
{
    // Регистрация типов для UI
    public static void AddUIServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.SetupViews();
        serviceCollection.SetupViewModels();

        serviceCollection.Remove(new ServiceDescriptor(typeof(MainViewModel), typeof(MainViewModel), ServiceLifetime.Transient));
        serviceCollection.Remove(new ServiceDescriptor(typeof(AuthViewModel), typeof(AuthViewModel), ServiceLifetime.Transient));

        serviceCollection.AddTransient<MainViewModel>(sp =>
        {
            // Получить список всех пункто меню пользователя
            var viewItems = sp.GetServices<ViewItem>();

            // Скрыть те, которые требуют прав администратора
            viewItems = App.CurrentUser.UserType != UserType.Admin
                ? viewItems.Where(x => x.IsProtected == false)
                : viewItems;

            return new MainViewModel(viewItems.ToArray(), sp);
        });

        serviceCollection.AddTransient(sp =>
        {
            var authService = sp.GetRequiredService<IAuthorizationService>();
            return new AuthViewModel(authService, sp);
        });

        serviceCollection.AddTransient<IWindowDialogService, WindowDialogService>();
    }

    // Создание списка разделов в меню пользоаателя
    public static void AddViewItems(this IServiceCollection serviceCollection)
    {        
        serviceCollection.AddSingleton(new ViewItem
        {
            Title = "Управление пользователями",
            Description = "Бла бла бла",
            ViewType = typeof(AdministrationView),
            ImageName = "UsersImage",
            IsProtected = true
        });

        serviceCollection.AddSingleton(new ViewItem
        {
            Title = "Управление категориями",
            Description = "Категории заявок технической поддержки",
            ViewType = typeof(CategoriesView),
            ImageName = "CategoriesImage",
            IsProtected = false
        });

        serviceCollection.AddSingleton(new ViewItem
        {
            Title = "Управление отделами",
            Description = "Физические отделы/кабинеты",
            ViewType = typeof(DepartmentsView),
            ImageName = "DepartmentsImage",
            IsProtected = false
        });

        serviceCollection.AddSingleton(new ViewItem
        {
            Title = "Зарегистрировать обращение",
            Description = "Создание заявки технической поддержки",
            ViewType = typeof(RequestCreationView),
            ImageName = "CreateRequestImage"
        });

        serviceCollection.AddSingleton(new ViewItem
        {
            Title = "Заявки технической поддержки",
            Description = "Общий список заявок",
            ViewType = typeof(RequestsView),
            ImageName = "RequestsImage"
        });
    }
}
