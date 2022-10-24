using HandyControl.Controls;
using TechSupport.UI.ViewModels;

namespace TechSupport.UI.Views;

public partial class AdministrationView : Window
{
    public AdministrationView(AdministrationViewModel context)
    {
        InitializeComponent();
        DataContext = context;
    }
}
