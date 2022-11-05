using Microsoft.Extensions.DependencyInjection;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Services;
using TechSupport.DataAccess;

namespace TechSupport.BusinessLogic;

public static class Configuration
{
    public static void AddBusinessLogicServices(this IServiceCollection serviceCollection)
    {
        // Регистрация сервисов уровня бизнес-логики в контейнере
        serviceCollection.AddTransient<IAuthorizationService, AuthorizationService>();
        serviceCollection.AddTransient<IUserService, UserService>();
        serviceCollection.AddTransient<ICategoryService, CategoryService>();
        serviceCollection.AddTransient<IDepartmentService, DepartmentService>();
        serviceCollection.AddTransient<IRequestService, RequestService>();

        serviceCollection.AddDataAccessLayer();
    }
}
