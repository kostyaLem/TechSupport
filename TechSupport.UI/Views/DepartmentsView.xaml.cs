using HandyControl.Controls;
using TechSupport.UI.ViewModels;

namespace TechSupport.UI.Views;

public partial class DepartmentsView : Window
{
    public DepartmentsView(DepartmentsViewModel context)
    {
        InitializeComponent();
        DataContext = context;
    }
}
