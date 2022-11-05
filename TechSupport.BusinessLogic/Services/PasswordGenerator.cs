using System.Security.Cryptography;
using System.Text;

namespace TechSupport.BusinessLogic.Services;

/// <summary>
/// Генератор паролей из строки
/// </summary>
internal static class PasswordGenerator
{
    // Сгенерировать хэш по строке
    public static string Generate(string value)
    {
        var bytes = new UTF8Encoding().GetBytes(value);

        byte[] hashBytes = SHA512.HashData(bytes);

        return Convert.ToBase64String(hashBytes);
    }
}
