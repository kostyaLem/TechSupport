using System.ComponentModel;

namespace TechSupport.BusinessLogic.Models.UserModels;

public enum UserType
{
    [Description("Администратор")]
    Admin,
    [Description("Поддержка")]
    Employee
}