using DevExpress.Mvvm;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

public sealed class RequestCreationViewModel : BaseViewModel
{
    public override string Title => "Создание заявки";

    
}


public sealed class DepartmentsViewModel : BaseViewModel
{
    private readonly IDepartmentService _departmentService;
    private readonly IWindowDialogService _dialogService;

    private readonly ObservableCollection<Department> _departments;

    public ICollectionView DepartmentsView { get; set; }

    public override string Title => "Управление отделами";

    public Department SelectedDepartment
    {
        get => GetValue<Department>(nameof(SelectedDepartment));
        set => SetValue(value, nameof(SelectedDepartment));
    }

    public string SearchText
    {
        get => GetValue<string>(nameof(SearchText));
        set => SetValue(value, () => DepartmentsView.Refresh(), nameof(SearchText));
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
        CreateDepartmentCommand = new AsyncCommand(CreateDepartment);
        UpdateDepartmentCommand = new AsyncCommand(EditDepartment, () => SelectedDepartment is not null);
        RemoveDepartmentCommand = new AsyncCommand(RemoveDepartment, () => SelectedDepartment is not null);

        _departments = new ObservableCollection<Department>();
        DepartmentsView = CollectionViewSource.GetDefaultView(_departments);
        DepartmentsView.Filter += CanFilterDepartment;
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
        var departmentViewModel = new EditDepartmentViewModel();

        var result = _dialogService.ShowDialog(
            "Создание нового отдела",
            typeof(EditDepartmentPage),
            departmentViewModel);

        if (result == Models.DialogResult.OK)
        {
            try
            {
                await _departmentService.Create(departmentViewModel.Department);
                await LoadDepartments();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }

    private async Task EditDepartment()
    {
        var department = await _departmentService.GetDepartmentById(SelectedDepartment.Id);
        var departmentViewModel = new EditDepartmentViewModel(department);

        var result = _dialogService.ShowDialog(
            "Редактирование отдела",
            typeof(EditDepartmentPage),
            departmentViewModel);

        if (result == Models.DialogResult.OK)
        {
            try
            {
                await _departmentService.Update(department);

                await LoadDepartments();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }

    private async Task RemoveDepartment()
    {
        await _departmentService.Remove(SelectedDepartment.Id);
        await LoadDepartments();
    }

    private async Task LoadDepartments()
    {
        _departments.Clear();
        var departments = await _departmentService.GetDepartments();
        _departments.AddRange(departments);
    }
}
