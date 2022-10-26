using System;
using System.Windows.Media.Imaging;
using TechSupport.BusinessLogic.Models.CategoriesModels;
using TechSupport.UI.Helpers;

namespace TechSupport.UI.Mapping;

public static class CategoryMapping
{
    public static Category Recreate(this Category category, Uri imageUri)
    {
        byte[] imageData = null;

        if (imageUri is not null)
        {
            var image = new BitmapImage(imageUri);
            imageData = ImageHelper.ImageToByteArray(image);
        }

        return category with
        {
            ImageData = imageData
        };
    }
}