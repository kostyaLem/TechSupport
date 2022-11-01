using DevExpress.Mvvm;
using HandyControl.Controls;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace TechSupport.UI.ViewModels;

public abstract class BaseViewModel : ViewModelBase
{
    public abstract string Title { get; }

    public ICollectionView ItemsView { get; protected set; }

    public string SearchText
    {
        get => GetValue<string>(nameof(SearchText));
        set => SetValue(value, () => ItemsView.Refresh(), nameof(SearchText));
    }

    public bool IsUploading
    {
        get => GetValue<bool>(nameof(IsUploading));
        set => SetValue(value, nameof(IsUploading));
    }

    public async Task Execute(Func<Task> action)
    {
        IsUploading = true;
        await Task.Delay(50);

        try
        {
            await action();
        }
        catch (Exception ex)
        {
            MessageBox.Error(ex.Message, "Ошибка выполнения операции");
        }
        finally
        {
            IsUploading = false;
        }
    }
}
