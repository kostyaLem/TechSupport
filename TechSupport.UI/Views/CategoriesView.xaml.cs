using HandyControl.Controls;
using TechSupport.UI.ViewModels;

namespace TechSupport.UI.Views;

public partial class CategoriesView : Window
{
    public CategoriesView(CategoriesViewModel context)
    {
        InitializeComponent();
        DataContext = context;
    }
}
