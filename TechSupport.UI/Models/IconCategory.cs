using System.Windows.Media;
using TechSupport.BusinessLogic.Models.CategoriesModels;

namespace TechSupport.UI.Models;

public class IconCategory
{
    public Category Category { get; init; }
    public ImageSource Image { get; init; }
}
