namespace TechSupport.BusinessLogic.Models;

public record Department
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Place { get; init; }
    public string Room { get; init; }

    public override string ToString() => Title;
}
