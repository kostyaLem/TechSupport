using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Input;
using TechSupport.UI.Models;

namespace TechSupport.UI.ViewModels;

public class MainViewModel : BaseItemsViewModel<ViewItem>
{
    private readonly IServiceProvider _serviceProvider;

    public override string Title => "Меню администратора";

    public ICommand OpenViewCommand { get; }

    public MainViewModel(ViewItem[] viewItems, IServiceProvider serviceProvider)
    {
        _items.AddRange(viewItems);
        _serviceProvider = serviceProvider;

        OpenViewCommand = new DelegateCommand<ViewItem>(OpenView);
    }

    private void OpenView(ViewItem viewItem)
    {
        IsUploading = true;

        App.Current.MainWindow.Visibility = System.Windows.Visibility.Hidden;
        var view = _serviceProvider.GetRequiredService(viewItem.ViewType) as Window;
        view.ShowDialog();

        IsUploading = false;

        App.Current.MainWindow.Visibility = System.Windows.Visibility.Visible;
    }
}
