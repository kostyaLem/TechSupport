using HandyControl.Controls;
using System.Windows.Controls;
using TechSupport.UI.Models;

namespace TechSupport.UI.Views.EditableViews;

public partial class EditView : Window, IDialogWindow
{
    public DialogResult DialogResult { get; private set; }

    public ContentControl ContextItem { get; }

    public EditView(ContentControl page)
    {
        InitializeComponent();
        ContextItem = page;
        DataContext = this;
    }

    private void btnClose_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        this.Close();
    }

    private void btnOk_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        DialogResult = DialogResult.OK;
        this.Close();
    }
}
