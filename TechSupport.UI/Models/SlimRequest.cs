using TechSupport.BusinessLogic.Models;
using TechSupport.UI.Views;

namespace TechSupport.UI.Models;

/// <summary>
/// Легковесное отображене заявки в таблице
/// </summary>
public class SlimRequest
{
    public string Title { get; set; }
    public string Computer { get; set; }
    public string Description { get; set; }
    public Department Department { get; set; }
    public Category Category { get; set; }
}
