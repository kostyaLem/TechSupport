using HandyControl.Controls;
using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using TechSupport.BusinessLogic.Models;
using TechSupport.BusinessLogic.Models.RequestModels;
using TechSupport.BusinessLogic.Models.UserModels;
using TechSupport.UI.Models;

namespace TechSupport.UI.Helpers;

public class ClearFilterConverter : MarkupExtension, IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var boxes = values.Cast<IList>().ToArray();
        return boxes;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}

public class SearchDataToFilterRequestConverter : MarkupExtension, IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var users = ((IList)values[0]).Cast<User>();
        var categories = ((IList)values[1]).Cast<IconCategory>();
        var departments = ((IList)values[2]).Cast<Department>();
        var statuses = ((IList)values[3]).Cast<RequestStatus>();

        return new RequestFilter(statuses, categories, departments, users);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
