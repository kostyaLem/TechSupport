using HandyControl.Controls;
using TechSupport.UI.ViewModels;

namespace TechSupport.UI.Views;

public partial class RequestsView : Window
{
    public RequestsView(RequestsViewModel context)
    {
        InitializeComponent();
        DataContext = context;
    }
}
