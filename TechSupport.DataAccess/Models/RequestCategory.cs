namespace TechSupport.DataAccess.Models;

public class RequestCategory
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public byte[] ImageData { get; set; }
}
