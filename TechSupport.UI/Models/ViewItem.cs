using System;

namespace TechSupport.UI.Models;

/// <summary>
/// Класс для отображения пункта меню в окне пользователя
/// </summary>
public record ViewItem
{
    public string Title { get; init; }
    public string Description { get; init; }
    public string ImageName { get; init; }
    public Type ViewType { get; init; }
    public bool IsProtected { get; init; }
}
