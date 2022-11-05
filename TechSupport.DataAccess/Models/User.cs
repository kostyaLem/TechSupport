namespace TechSupport.DataAccess.Models;

/// <summary>
/// Сущность "Пользователя системы"
/// </summary>
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public UserType Type { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime LastActivity { get; set; }
}
