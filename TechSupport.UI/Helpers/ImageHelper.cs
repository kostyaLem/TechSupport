using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TechSupport.UI.Helpers;

internal static class ImageHelper
{
    public static BitmapImage LoadImage(byte[] imageData)
    {
        var image = new BitmapImage();
        using (var mem = new MemoryStream(imageData))
        {
            mem.Position = 0;
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = null;
            image.StreamSource = mem;
            image.EndInit();
        }
        image.Freeze();
        return image;
    }

    public static byte[] ImageToByteArray(BitmapImage image)
    {
        return File.ReadAllBytes(image.UriSource.AbsolutePath);
    }
}
