using HandyControl.Controls;
using System.Windows.Automation;
using System.Windows.Controls;
using TechSupport.UI.Models;

namespace TechSupport.UI.Views.EditableViews;

public partial class EditView : Window, IDialogWindow
{
    private bool _isAccept;
    public DialogResult DialogResult { get; private set; }
    public ContentControl ContextItem { get; }

    public EditView(string title, ContentControl page)
    {
        InitializeComponent();
        Title = title;
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
        _isAccept = true;
        this.Close();
    }

    private void Window_Closed(object sender, System.EventArgs e)
    {
        if (!_isAccept)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
