using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TechSupport.BusinessLogic.Models;
using TechSupport.UI.Helpers;

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
}