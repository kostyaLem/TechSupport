using HandyControl.Controls;
using TechSupport.UI.ViewModels;

namespace TechSupport.UI.Views;

public partial class RequestCreationView : Window
{
    public RequestCreationView(RequestCreationViewModel context)
    {
        InitializeComponent();
        DataContext = context;
    }
}
