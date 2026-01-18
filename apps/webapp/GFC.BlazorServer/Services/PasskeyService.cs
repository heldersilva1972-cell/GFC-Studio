using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GFC.BlazorServer.Services;

public interface IPasskeyService
{
    Task<object> RequestRegistrationOptionsAsync(int userId, string username);
    Task<bool> CompleteRegistrationAsync(int userId, string credentialId, string publicKey, string friendlyName);
    Task<object> RequestLoginOptionsAsync(string username);
    Task<bool> ValidateLoginAsync(string credentialId, string username);
    Task<List<UserPasskey>> GetUserPasskeysAsync(int userId);
    Task<bool> RevokePasskeyAsync(int id);
}

public class PasskeyService : IPasskeyService
{
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;
    private readonly ILogger<PasskeyService> _logger;
    private readonly IConfiguration _configuration;

    public PasskeyService(
        IDbContextFactory<GfcDbContext> contextFactory,
        ILogger<PasskeyService> logger,
        IConfiguration configuration)
    {
        _contextFactory = contextFactory;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<object> RequestRegistrationOptionsAsync(int userId, string username)
    {
        // Generate a challenge for WebAuthn
        var challenge = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        
        // Get existing credentials to prevent double registration
        await using var context = await _contextFactory.CreateDbContextAsync();
        var existingCredentials = await context.UserPasskeys
            .Where(p => p.UserId == userId)
            .Select(p => new { id = p.CredentialId, type = "public-key" })
            .ToListAsync();

        var rpName = _configuration["Fido2:ServerName"] ?? "GFC System";
        var rpId = _configuration["Fido2:ServerDomain"] ?? "localhost";

        return new
        {
            challenge = challenge,
            rp = new { name = rpName, id = rpId },
            user = new
            {
                id = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(userId.ToString())),
                name = username,
                displayName = username
            },
            pubKeyCredParams = new[]
            {
                new { type = "public-key", alg = -7 },  // ES256
                new { type = "public-key", alg = -257 } // RS256
            },
            timeout = 60000,
            excludeCredentials = existingCredentials,
            authenticatorSelection = new
            {
                authenticatorAttachment = "platform",
                requireResidentKey = false,
                userVerification = "preferred"
            },
            attestation = "none"
        };
    }

    public async Task<bool> CompleteRegistrationAsync(int userId, string credentialId, string publicKey, string friendlyName)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            
            // Check if credential already exists
            if (await context.UserPasskeys.AnyAsync(p => p.CredentialId == credentialId))
            {
                _logger.LogWarning("Credential {CredentialId} already exists", credentialId);
                return false;
            }

            var passkey = new UserPasskey
            {
                UserId = userId,
                CredentialId = credentialId,
                PublicKey = publicKey,
                FriendlyName = friendlyName,
                UserHandle = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(userId.ToString())),
                SignatureCounter = 0,
                AttestationFormat = "none",
                CreatedAtUtc = DateTime.UtcNow,
                AAGUID = Guid.Empty
            };

            context.UserPasskeys.Add(passkey);
            await context.SaveChangesAsync();
            
            _logger.LogInformation("Passkey registered for user {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing passkey registration");
            return false;
        }
    }

    public async Task<object> RequestLoginOptionsAsync(string username)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        // Query AppUsers table directly using FromSqlRaw
        var user = await context.Set<AppUser>()
            .FromSqlRaw("SELECT * FROM AppUsers WHERE Username = {0}", username)
            .FirstOrDefaultAsync();
        if (user == null)
        {
            _logger.LogWarning("User {Username} not found for passkey login", username);
            throw new Exception("User not found");
        }

        var existingCredentials = await context.UserPasskeys
            .Where(p => p.UserId == user.UserId)
            .Select(p => new { id = p.CredentialId, type = "public-key" })
            .ToListAsync();

        if (!existingCredentials.Any())
        {
            throw new Exception("No passkeys registered for this user");
        }

        var challenge = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        var rpId = _configuration["Fido2:ServerDomain"] ?? "localhost";

        return new
        {
            challenge = challenge,
            timeout = 60000,
            rpId = rpId,
            allowCredentials = existingCredentials,
            userVerification = "preferred"
        };
    }

    public async Task<bool> ValidateLoginAsync(string credentialId, string username)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var passkey = await context.UserPasskeys
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.CredentialId == credentialId);

            if (passkey == null)
            {
                _logger.LogWarning("Passkey {CredentialId} not found", credentialId);
                return false;
            }

            if (passkey.User.Username != username)
            {
                _logger.LogWarning("Username mismatch for passkey {CredentialId}", credentialId);
                return false;
            }

            // Update last used timestamp
            passkey.LastUsedUtc = DateTime.UtcNow;
            await context.SaveChangesAsync();

            _logger.LogInformation("Passkey login successful for user {Username}", username);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating passkey login");
            return false;
        }
    }

    public async Task<List<UserPasskey>> GetUserPasskeysAsync(int userId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.UserPasskeys
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<bool> RevokePasskeyAsync(int id)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var passkey = await context.UserPasskeys.FindAsync(id);
            if (passkey != null)
            {
                context.UserPasskeys.Remove(passkey);
                await context.SaveChangesAsync();
                _logger.LogInformation("Passkey {Id} revoked", id);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking passkey");
            return false;
        }
    }
}
