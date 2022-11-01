using System.Windows.Data;
using TechSupport.UI.Models;

namespace TechSupport.UI.ViewModels;

public class MainViewModel : BaseViewModel
{
    public override string Title => "Меню администратора";

    public MainViewModel(ViewItem[] viewItems)
    {
        ItemsView = CollectionViewSource.GetDefaultView(viewItems);
    }
}
