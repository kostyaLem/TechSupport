namespace TechSupport.DataAccess.Models;

/// <summary>
/// Сущность "Категория заявки"
/// </summary>
public class RequestCategory
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public byte[] ImageData { get; set; }
}
