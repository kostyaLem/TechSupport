using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using TechSupport.BusinessLogic.Models.UserModels;

namespace TechSupport.UI.ViewModels.EditViewModels;

public class EditCustomerViewModel : BindableBase
{
    public string Title { get; }

    public User User { get; set; }

    public IEnumerable<UserType> UserTypes =>
        Enum.GetValues<UserType>();

    public EditCustomerViewModel()
    {
        Title = "Создание нового пользователя";
        User = new User();
    }

    public EditCustomerViewModel(User user)
    {
        Title = "Редактирование пользователя";
        User = user;
    }
}
