using System.IO;
using System.Windows.Media.Imaging;

namespace TechSupport.UI.Helpers;

/// <summary>
/// Класс для работы с картинками
/// </summary>
internal static class ImageHelper
{
    // Преобразовать массив байтов в картинку
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

    // Преобразовать картинку в массив байтов
    public static byte[] ImageToByteArray(BitmapImage image)
    {
        return File.ReadAllBytes(image.UriSource.AbsolutePath);
    }
}
