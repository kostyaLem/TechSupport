using DevExpress.Mvvm;
using HandyControl.Controls;

namespace TechSupport.UI.Models;

/// <summary>
/// Класс-сборщик всех фильтров по заявкам
/// </summary>
public class RequestFilter : BindableBase
{
    public CheckComboBox RequestStatuses { get; set; }
    public CheckComboBox Categories { get; set; }
    public CheckComboBox Departments { get; set; }
    public CheckComboBox Users { get; set; }

    public RequestFilter(
        CheckComboBox statuses,
        CheckComboBox categories,
        CheckComboBox departments,
        CheckComboBox users)
    {
        RequestStatuses = statuses;
        Categories = categories;
        Departments = departments;
        Users = users;
    }
}
