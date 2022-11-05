using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Markup;

namespace TechSupport.UI.Converters;

/// <summary>
/// Класс для преобразования перечисления в строку для пользователя
/// </summary>
public class EnumToDescriptionConverter : MarkupExtension, IValueConverter
{
    private string GetEnumDescription(Enum enumObj)
    {
        FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

        object[] attribArray = fieldInfo.GetCustomAttributes(false);

        if (attribArray.Length == 0)
        {
            return enumObj.ToString();
        }
        else
        {
            DescriptionAttribute attrib = attribArray[0] as DescriptionAttribute;
            return attrib.Description;
        }
    }

    object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        Enum myEnum = (Enum)value;
        string description = GetEnumDescription(myEnum);
        return description;
    }

    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
        => this;
}
