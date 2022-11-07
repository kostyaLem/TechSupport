using DevExpress.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace TechSupport.UI;

/// <summary>
/// Класс для регистрации UI типов
/// </summary>
internal static class IocExtensions
{
    // Регистрация окон (View) и вьюмоделей (ViewModel)
    public static void SetupViews(this IServiceCollection services)
    {
        var views = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => IsView(x))
            .ToList();

        views.ForEach(vm => services.AddTransient(vm));
    }

    public static void SetupViewModels(this IServiceCollection services)
    {
        var viewModels = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => IsViewModel(x))
            .ToList();

        viewModels.ForEach(vm => services.AddTransient(vm));
    }

    private static bool IsViewModel(Type objectType)
        => objectType.IsSubclassOf(typeof(ViewModelBase)) && !objectType.IsAbstract;

    private static bool IsView(Type objectType)
        => objectType.IsSubclassOf(typeof(Window));
}

