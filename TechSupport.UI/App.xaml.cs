﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using TechSupport.BusinessLogic;
using TechSupport.BusinessLogic.Models.UserModels;
using TechSupport.DataAccess.Context;
using TechSupport.UI.Views;

namespace TechSupport.UI;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public static CurrentUser CurrentUser { get; set; }
    public static bool IsAdmin => CurrentUser.UserType == UserType.Admin;

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

        _serviceProvider.GetRequiredService<TechSupportContext>().Database.Migrate();
        _serviceProvider.GetRequiredService<AuthView>().ShowDialog();
    }
}
