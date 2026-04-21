using System.Security.Cryptography;
using System.Text;

namespace GFC.Core.Helpers;

/// <summary>
/// Helper class for password hashing and verification.
/// </summary>
public static class PasswordHelper
{
    /// <summary>
    /// Hashes a password using SHA256. In production, consider using bcrypt or Argon2.
    /// </summary>
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Verifies a password against a hash.
    /// </summary>
    public static bool VerifyPassword(string password, string hash)
    {
        var passwordHash = HashPassword(password);
        return passwordHash == hash;
    }
}

