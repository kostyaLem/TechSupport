using DevExpress.Mvvm;
using System.Collections.Generic;
using TechSupport.BusinessLogic.Models;
using TechSupport.BusinessLogic.Models.RequestModels;
using TechSupport.BusinessLogic.Models.UserModels;

namespace TechSupport.UI.Models;

public class RequestFilter : BindableBase
{
    public IEnumerable<RequestStatus> RequestStatuses { get; set; }
    public IEnumerable<IconCategory> Categories { get; set; }
    public IEnumerable<Department> Departments { get; set; }
    public IEnumerable<User> Users { get; set; }

    public RequestFilter(
        IEnumerable<RequestStatus> statuses,
        IEnumerable<IconCategory> categories,
        IEnumerable<Department> departments,
        IEnumerable<User> users)
    {
        RequestStatuses = statuses;
        Categories = categories;
        Departments = departments;
        Users = users;
    }
}
