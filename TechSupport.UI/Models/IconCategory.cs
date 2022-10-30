using System.Windows.Media.Imaging;
using TechSupport.BusinessLogic.Models;

namespace TechSupport.UI.Models;

public class IconCategory
{
    public Category Category { get; init; }
    public BitmapImage Image { get; init; }

    public override string ToString() => Category.Title;
}
