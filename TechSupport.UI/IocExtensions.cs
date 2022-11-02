using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using TechSupport.UI.ViewModels;

namespace TechSupport.UI;

internal static class IocExtensions
{
    public static void SetupViews(this IServiceCollection services)
    {
        var viewModels = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => IsViewModel(x) || IsView(x))
            .ToList();

        viewModels.ForEach(vm => services.AddTransient(vm));
    }

    public static void SetupPages(this IServiceCollection services)
    {
        var viewModels = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => IsPage(type))
            .ToList();

        viewModels.ForEach(vm => services.AddTransient(vm));
    }

    private static bool IsViewModel(Type objectType)
        => objectType.IsSubclassOf(typeof(BaseViewModel));

    private static bool IsView(Type objectType)
        => objectType.IsSubclassOf(typeof(Window));

    private static bool IsPage(Type objectType)
        => objectType.IsSubclassOf(typeof(Page));
}

