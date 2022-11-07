using DevExpress.Mvvm;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using TechSupport.BusinessLogic.Interfaces;
using TechSupport.BusinessLogic.Models;
using TechSupport.UI.Services;
using TechSupport.UI.ViewModels.EditViewModels;
using TechSupport.UI.Views.EditableViews;

namespace TechSupport.UI.ViewModels;

public sealed class DepartmentsViewModel : BaseItemsViewModel<Department>
{
    private readonly IDepartmentService _departmentService;
    private readonly IWindowDialogService _dialogService;

    public override string Title => "Управление отделами";

    public Department SelectedDepartment
    {
        get => GetValue<Department>(nameof(SelectedDepartment));
        set => SetValue(value, nameof(SelectedDepartment));
    }

    public ICommand LoadViewDataCommand { get; }
    public ICommand CreateDepartmentCommand { get; }
    public ICommand UpdateDepartmentCommand { get; }
    public ICommand RemoveDepartmentCommand { get; }

    public DepartmentsViewModel(IDepartmentService departmentService, IWindowDialogService dialogService)
    {
        _departmentService = departmentService;
        _dialogService = dialogService;

        LoadViewDataCommand = new AsyncCommand(LoadDepartments);
        CreateDepartmentCommand = new AsyncCommand(CreateDepartment, () => App.IsAdmin);
        UpdateDepartmentCommand = new AsyncCommand(EditDepartment, () => SelectedDepartment is not null && App.IsAdmin);
        RemoveDepartmentCommand = new AsyncCommand(RemoveDepartment, () => SelectedDepartment is not null && App.IsAdmin);

        ItemsView.Filter += CanFilterDepartment;
    }

    private bool CanFilterDepartment(object obj)
    {
        if (SearchText is { } && obj is Department department)
        {
            var predicates = new List<string>
            {
                department.Title,
                department.Place,
                department.Room
            };

            return predicates.Any(x => x.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        return true;
    }

    private async Task CreateDepartment()
    {
        await Execute(async () =>
        {
            var departmentViewModel = new EditDepartmentViewModel();

            var result = _dialogService.ShowDialog(
                "Создание нового отдела",
                typeof(EditDepartmentPage),
                departmentViewModel);

            if (result == Models.DialogResult.OK)
            {
                await _departmentService.Create(departmentViewModel.Department);
                await LoadDepartments();
            }
        });
    }

    private async Task EditDepartment()
    {
        await Execute(async () =>
        {
            var department = await _departmentService.GetDepartmentById(SelectedDepartment.Id);
            var departmentViewModel = new EditDepartmentViewModel(department);

            var result = _dialogService.ShowDialog(
                "Редактирование отдела",
                typeof(EditDepartmentPage),
                departmentViewModel);

            if (result == Models.DialogResult.OK)
            {
                await _departmentService.Update(department);
                await LoadDepartments();
            }
        });
    }

    private async Task RemoveDepartment()
    {
        await Execute(async () =>
        {
            await _departmentService.Remove(SelectedDepartment.Id);
        });
        await LoadDepartments();
    }

    // Метод загрузки предварительных данных после появления окна на экране
    private async Task LoadDepartments()
    {
        await Execute(async () =>
        {
            _items.Clear();
            var departments = await _departmentService.GetDepartments();
            _items.AddRange(departments);
        });
    }
}
