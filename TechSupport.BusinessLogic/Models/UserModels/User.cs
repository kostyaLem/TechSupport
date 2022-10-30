namespace TechSupport.BusinessLogic.Models.UserModels;

public record User
{
    public int Id { get; init; }
    public string Login { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public DateTime Birthday { get; init; }
    public DateTime CreatedOn { get; init; }
    public DateTime UpdatedOn { get; init; }
    public UserType UserType { get; init; }

    public override string ToString() => $"{LastName} {FirstName}";
}
