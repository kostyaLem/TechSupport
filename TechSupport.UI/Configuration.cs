using Microsoft.Extensions.DependencyInjection;
using TechSupport.UI.Services;

namespace TechSupport.UI;

public static class Configuration
{
    public static void AddUIServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.SetupPages();
        serviceCollection.SetupViews();

        serviceCollection.AddTransient<IWindowDialogService, WindowDialogService>();
    }
}
