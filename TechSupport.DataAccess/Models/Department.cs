namespace TechSupport.DataAccess.Models;

/// <summary>
/// Сущность "Отдел"
/// </summary>
public class Department
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Place { get; set; }
    public string Room { get; set; }
}
