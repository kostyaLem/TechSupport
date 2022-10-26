using DevExpress.Mvvm;
using TechSupport.BusinessLogic.Models;

namespace TechSupport.UI.ViewModels.EditViewModels;

public class EditDepartmentViewModel : BindableBase
{
    public Department Department { get; set; }

    public EditDepartmentViewModel()
    {
        Department = new Department();
    }

    public EditDepartmentViewModel(Department department)
    {
        Department = department;
    }
}
