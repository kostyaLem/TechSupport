using DevExpress.Mvvm;
using System.Threading.Tasks;
using System;
using HandyControl.Controls;

namespace TechSupport.UI.ViewModels.Base;

/// <summary>
/// Базоовая ViewModel для всех дочерних
/// </summary>
public abstract class BaseViewModel : ViewModelBase
{
    // Название View (окна)
    public virtual string Title { get; }

    // Флаг для отображения индикатора загрузки при выполнении асинхронных действий
    public bool IsUploading
    {
        get => GetValue<bool>(nameof(IsUploading));
        set => SetValue(value, nameof(IsUploading));
    }

    // Метод, устанавливающий флаг и выполняющий передаваемую функцию
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
            // Вывод ошибки в унифицированном окне
            MessageBox.Error(ex.Message, "Ошибка выполнения операции");
        }
        finally
        {
            IsUploading = false;
        }
    }
}
