namespace TechSupport.BusinessLogic.Exceptions;

/// <summary>
/// Ошибка создания пользователя с повторяющимися данными
/// </summary>
internal class DuplicateDataException : Exception
{
	public DuplicateDataException(string message) : base(message)
	{

	}
}
