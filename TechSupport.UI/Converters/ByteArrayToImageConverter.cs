using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using TechSupport.UI.Helpers;

namespace TechSupport.UI.Converters;

/// <summary>
/// Класс для преобразования массива байтов в картинку
/// </summary>
public class ByteArrayToImageConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is byte[] data)
        {
            return ImageHelper.LoadImage(data);
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
        => this;
}
