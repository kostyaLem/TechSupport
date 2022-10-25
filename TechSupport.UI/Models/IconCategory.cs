using System.Windows.Media.Imaging;
using TechSupport.BusinessLogic.Models.CategoriesModels;

namespace TechSupport.UI.Models;

public class IconCategory
{
    public Category Category { get; init; }
    public BitmapFrame Image { get; init; }
}
