﻿using System;
using System.Windows.Controls;
using TechSupport.UI.Models;
using TechSupport.UI.Views.EditableViews;

namespace TechSupport.UI.Services;

public interface IWindowDialogService
{
    DialogResult ShowDialog(string title, Type controlType, object dataContext);
}

public sealed class WindowDialogService : IWindowDialogService
{
    public DialogResult ShowDialog(string title, Type controlType, object dataContext)
    {
        var control = (ContentControl)Activator.CreateInstance(controlType, dataContext);

        var editView = new EditView(title, control);
        var dialogResult = editView.ShowDialog();

        if (dialogResult.HasValue)
        {
            return editView.DialogResult;
        }

        return DialogResult.Cancel;
    }
}
