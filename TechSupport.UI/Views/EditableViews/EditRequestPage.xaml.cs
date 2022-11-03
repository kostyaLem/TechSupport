using System.Windows.Controls;
using TechSupport.UI.ViewModels.EditViewModels;

namespace TechSupport.UI.Views.EditableViews;

public partial class EditRequestPage : UserControl
{
    public EditRequestPage(EditRequestViewModel context)
    {
        InitializeComponent();
        DataContext = context;
    }
}
