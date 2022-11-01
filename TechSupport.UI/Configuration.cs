using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using TechSupport.BusinessLogic.Models.UserModels;
using TechSupport.UI.Models;
using TechSupport.UI.Services;
using TechSupport.UI.ViewModels;
using TechSupport.UI.Views;

namespace TechSupport.UI;

public static class Configuration
{
    public static void AddUIServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.SetupPages();
        serviceCollection.SetupViews();

        serviceCollection.Remove(new ServiceDescriptor(typeof(MainViewModel), typeof(MainViewModel), ServiceLifetime.Singleton));

        serviceCollection.AddTransient<MainViewModel>(x =>
        {
            var viewItems = x.GetServices<ViewItem>();

            viewItems = App.CurrentUser.UserType != UserType.Admin
                ? viewItems.Where(x => x.IsProtected == false)
                : viewItems;

            return new MainViewModel(viewItems.ToArray());
        });

        serviceCollection.AddTransient<IWindowDialogService, WindowDialogService>();
    }

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
            IsProtected = true
        });

        serviceCollection.AddSingleton(new ViewItem
        {
            Title = "Управление отделами",
            Description = "Физические отделы/кабинеты",
            ViewType = typeof(DepartmentsView),
            ImageName = "DepartmentsImage",
            IsProtected = true
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
