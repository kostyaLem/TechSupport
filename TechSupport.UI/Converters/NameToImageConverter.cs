using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace TechSupport.UI.Converters;

/// <summary>
/// Поиск картинки по строке в ресурсах
/// </summary>
internal class NameToImageConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (string.IsNullOrWhiteSpace(value as string))
        {
            return null;
        }

        var imageName = value.ToString();
        return System.Windows.Application.Current.FindResource(imageName) as BitmapImage;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
        => this;
}
