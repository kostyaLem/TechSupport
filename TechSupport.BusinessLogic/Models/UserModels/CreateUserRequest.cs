namespace TechSupport.BusinessLogic.Models.UserModels;

public record CreateUserRequest
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public DateTime Birthday { get; init; }
    public UserType UserType { get; init; }
    public string Login { get; init; }
    public string PasswordHash { get; init; }
}