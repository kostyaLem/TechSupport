using System.Windows;
using TechSupport.UI.ViewModels;

namespace TechSupport.UI.Views;

public partial class AuthView : Window
{
    public AuthView(AuthViewModel context)
    {
        InitializeComponent();
        DataContext = context;
    }
}
