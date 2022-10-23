using Microsoft.Extensions.DependencyInjection;
using TechSupport.UI.ViewModels;

namespace TechSupport.UI;

public static class Configuration
{
    public static void AddUIServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<AuthViewModel>();
        serviceCollection.SetupPages();
        serviceCollection.SetupViews();
    }
}
