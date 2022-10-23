using System.Windows;
using TechSupport.UI.ViewModels;

namespace TechSupport.UI.Views;

public partial class AdministrationView : Window
{
    public AdministrationView(AuthViewModel context)
    {
        InitializeComponent();
        DataContext = context;
    }
}
