namespace TechSupport.BusinessLogic.Exceptions;

/// <summary>
/// Ошибка поиска сущности в базе данных
/// </summary>
internal class NotFoundException : Exception
{
	public NotFoundException(string message) : base(message)
	{

	}
}
