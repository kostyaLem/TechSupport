using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using TechSupport.BusinessLogic.Models;
using TechSupport.UI.Helpers;
using TechSupport.UI.Models;

namespace TechSupport.UI.Mapping;

public static class CategoryMapping
{
    public static Category Recreate(this Category category, BitmapImage bitmapSource)
    {
        byte[] imageData = null;

        if (bitmapSource is not null)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            using var stream = new MemoryStream();

            encoder.Save(stream);
            imageData = stream.ToArray();
        }

        return category with
        {
            ImageData = imageData
        };
    }

    public static IReadOnlyList<IconCategory> MapToIcons(this IReadOnlyList<Category> categories)
    {
        return categories.Select(x =>
        {
            return new IconCategory
            {
                Category = x,
                Image = x.ImageData is not null ? ImageHelper.LoadImage(x.ImageData) : null
            };
        }).ToList();
    }
}