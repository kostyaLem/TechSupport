using System;
using System.Windows.Controls;
using TechSupport.UI.Models;
using TechSupport.UI.Views.EditableViews;

namespace TechSupport.UI.Services;

public interface IWindowDialogService
{
    DialogResult ShowDialog(string title, Type controlType, object dataContext);
}

/// <summary>
/// Класс для открытия вспомогательного окна с выбором "Да/Нет"
/// </summary>
public sealed class WindowDialogService : IWindowDialogService
{
    public DialogResult ShowDialog(string title, Type controlType, object dataContext)
    {
        // Создать представление (View)
        var control = (ContentControl)Activator.CreateInstance(controlType, dataContext);

        // Заполнить заголовок и внутреннее содержимое окна с имзенениями
        var editView = new EditView(title, control);
        var dialogResult = editView.ShowDialog();

        if (dialogResult.HasValue)
        {
            return editView.DialogResult;
        }

        return DialogResult.Cancel;
    }
}
