using System.Windows.Controls;
using TechSupport.UI.ViewModels.EditViewModels;

namespace TechSupport.UI.Views.EditableViews;

public partial class EditCustomerPage : UserControl
{
    public EditCustomerPage(EditCustomerViewModel context)
    {
        InitializeComponent();
        DataContext = context;
    }
}
