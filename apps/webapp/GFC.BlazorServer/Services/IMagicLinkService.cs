using System.Threading.Tasks;

namespace GFC.BlazorServer.Services;

public interface IMagicLinkService
{
    /// <summary>
    /// Generates a magic link token for the given email (if user exists) and sends it.
    /// Returns true if sent, false if user not found or rate limited.
    /// </summary>
    Task<bool> SendMagicLinkAsync(string email);

    /// <summary>
    /// Validates the token. If valid, returns the UserId and marks token used.
    /// If invalid/expired/used, returns null.
    /// </summary>
    Task<int?> ValidateTokenAsync(string token);
}
