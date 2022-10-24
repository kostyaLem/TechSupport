using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using TechSupport.BusinessLogic.Models.UserModels;

namespace TechSupport.UI.ViewModels.EditViewModels;

public class EditCustomerViewModel : BindableBase
{
    public string Password { get; set; }
    public User User { get; set; }

    public IEnumerable<UserType> UserTypes =>
        Enum.GetValues<UserType>();

    public EditCustomerViewModel()
    {
        User = new User();
    }

    public EditCustomerViewModel(User user)
    {
        User = user;
    }
}
