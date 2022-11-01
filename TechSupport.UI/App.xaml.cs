using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using TechSupport.BusinessLogic;
using TechSupport.BusinessLogic.Models.UserModels;
using TechSupport.UI.Views;
using TechSupport.UI.Views.EditableViews;

namespace TechSupport.UI;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public static CurrentUser CurrentUser { get; set; } = new CurrentUser("temp", UserType.Admin);

    public App()
    {
        _serviceProvider = CreateServiceCollection();
    }

    private IServiceProvider CreateServiceCollection()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddBusinessLogicServices();
        serviceCollection.AddUIServices();
        serviceCollection.AddViewItems();

        return serviceCollection.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _serviceProvider.GetRequiredService<MainView>().ShowDialog();
        //_serviceProvider.GetRequiredService<RequestCreationView>().ShowDialog();
    }
}
