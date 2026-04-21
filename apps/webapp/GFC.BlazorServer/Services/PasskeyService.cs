using GFC.BlazorServer.Data;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
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
    Task<bool> RevokeAllUserPasskeysAsync(int userId);
    Task<bool> HasPasskeysAsync(string username);
}


public class PasskeyService : IPasskeyService
{
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;
    private readonly ILogger<PasskeyService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PasskeyService(
        IDbContextFactory<GfcDbContext> contextFactory,
        ILogger<PasskeyService> logger,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
    {
        _contextFactory = contextFactory;
        _logger = logger;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<object> RequestRegistrationOptionsAsync(int userId, string username)
    {
        // Generate a 32-byte challenge for WebAuthn (more robust than 16)
        var challengeBytes = new byte[32];
        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            rng.GetBytes(challengeBytes);
        }
        var challenge = Convert.ToBase64String(challengeBytes);
        
        // Get existing credentials to prevent double registration
        await using var context = await _contextFactory.CreateDbContextAsync();
        var existingCredentials = await context.UserPasskeys
            .Where(p => p.UserId == userId)
            .Select(p => new { id = p.CredentialId, type = "public-key" })
            .ToListAsync();
 
        var rpName = _configuration["Fido2:ServerName"] ?? "GFC System";
        
        // [FIX] Dynamic RP ID Detection
        // If we are on localhost, we MUST use 'localhost' as the RP ID, otherwise the browser rejects it.
        // We only use the configured ServerDomain if it matches the current request or if the request is unknown.
        var httpContext = _httpContextAccessor.HttpContext;
        string currentHost = httpContext?.Request.Host.Host ?? "localhost";
        
        string rpId = _configuration["Fido2:ServerDomain"];
        
        // If hardcoded RP ID doesn't match current host, and current host is 'localhost', override it.
        // Also override if no RP ID is configured.
        if (string.IsNullOrEmpty(rpId) || (currentHost == "localhost" && rpId != "localhost"))
        {
            rpId = currentHost;
        }
        else if (currentHost.Contains(".") && !rpId.Contains("."))
        {
            // If we have a real domain but config is simple/missing, use the domain
            rpId = currentHost;
        }
 
        // Create a 16-byte user handle (more compatible with Android/Chrome)
        byte[] userHandle = new byte[16];
        byte[] idBytes = System.Text.Encoding.UTF8.GetBytes(userId.ToString());
        Array.Copy(idBytes, 0, userHandle, 0, Math.Min(idBytes.Length, 16));
        var userBase64 = Convert.ToBase64String(userHandle);
 
        return new
        {
            challenge = challenge,
            rp = new { name = rpName, id = rpId },
            user = new
            {
                id = userBase64,
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
                // Removing strict "platform" helps resolve NotReadableError on some devices
                // authenticatorAttachment = "platform", 
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
            _logger.LogInformation("Starting passkey registration for userId={UserId}, credentialId={CredentialId}", userId, credentialId);
            
            await using var context = await _contextFactory.CreateDbContextAsync();
            
            // Check if credential already exists
            var existingPasskey = await context.UserPasskeys
                .FirstOrDefaultAsync(p => p.CredentialId == credentialId);
                
            if (existingPasskey != null)
            {
                _logger.LogWarning("Credential {CredentialId} already exists for user {ExistingUserId}", credentialId, existingPasskey.UserId);
                return false;
            }

            var passkey = new UserPasskey
            {
                UserId = userId,
                CredentialId = credentialId,
                PublicKey = publicKey,
                FriendlyName = friendlyName,
                // Match the 16-byte user handle logic from RequestRegistrationOptionsAsync
                UserHandle = Convert.ToBase64String(new byte[16]), 
                SignatureCounter = 0,
                AttestationFormat = "none",
                CreatedAtUtc = DateTime.UtcNow,
                AAGUID = null
            };
            
            // Re-calculate the actual handle to match what was sent
            byte[] userHandle = new byte[16];
            byte[] idBytes = System.Text.Encoding.UTF8.GetBytes(userId.ToString());
            Array.Copy(idBytes, 0, userHandle, 0, Math.Min(idBytes.Length, 16));
            passkey.UserHandle = Convert.ToBase64String(userHandle);

            context.UserPasskeys.Add(passkey);
            var rowsAffected = await context.SaveChangesAsync();
            
            _logger.LogInformation("Passkey registered successfully for user {UserId}. Rows affected: {RowsAffected}", userId, rowsAffected);
            return rowsAffected > 0;
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database error completing passkey registration for user {UserId}", userId);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing passkey registration for user {UserId}", userId);
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
        
        // [FIX] Dynamic RP ID Detection (Login)
        var httpContext = _httpContextAccessor.HttpContext;
        string currentHost = httpContext?.Request.Host.Host ?? "localhost";
        string rpId = _configuration["Fido2:ServerDomain"];
        if (string.IsNullOrEmpty(rpId) || (currentHost == "localhost" && rpId != "localhost"))
        {
            rpId = currentHost;
        }

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

    public async Task<bool> RevokeAllUserPasskeysAsync(int userId)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var keys = await context.UserPasskeys.Where(p => p.UserId == userId).ToListAsync();
            if (keys.Any())
            {
                context.UserPasskeys.RemoveRange(keys);
                await context.SaveChangesAsync();
                _logger.LogInformation("All {Count} passkeys revoked for user {UserId}", keys.Count, userId);
            }
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking all passkeys for user {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> HasPasskeysAsync(string username)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            
            var user = await context.Set<AppUser>()
                .FromSqlRaw("SELECT * FROM AppUsers WHERE Username = {0}", username)
                .FirstOrDefaultAsync();
                
            if (user == null) return false;

            return await context.UserPasskeys.AnyAsync(p => p.UserId == user.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if user {Username} has passkeys", username);
            return false;
        }
    }
}


