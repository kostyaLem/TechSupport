using DevExpress.Mvvm;

namespace TechSupport.UI.ViewModels;

public abstract class BaseViewModel : ViewModelBase
{
    public abstract string Title { get; }
}
