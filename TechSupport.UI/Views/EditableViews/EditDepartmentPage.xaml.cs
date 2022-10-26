using System.Windows.Controls;
using TechSupport.UI.ViewModels.EditViewModels;

namespace TechSupport.UI.Views.EditableViews;

public partial class EditDepartmentPage : UserControl
{
    public EditDepartmentPage(EditDepartmentViewModel context)
    {
        InitializeComponent();
        DataContext = context;
    }
}
