using HandyControl.Controls;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using TechSupport.UI.Models;

namespace TechSupport.UI.Converters;

/// <summary>
/// Класс для сбора выбранных параметров фильтра по поиску заявок
/// </summary>
public class SearchDataToFilterRequestConverter : MarkupExtension, IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var users = values[0] as CheckComboBox;
        var categories = values[1] as CheckComboBox;
        var departments = values[2] as CheckComboBox;
        var statuses = values[3] as CheckComboBox;

        return new RequestFilter(statuses, categories, departments, users);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
        => this;
}
