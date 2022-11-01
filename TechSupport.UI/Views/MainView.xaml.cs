using HandyControl.Controls;
using TechSupport.UI.ViewModels;

namespace TechSupport.UI.Views;

public partial class MainView : Window
{
    public MainView(MainViewModel context)
    {
        InitializeComponent();
        DataContext = context;
    }
}
