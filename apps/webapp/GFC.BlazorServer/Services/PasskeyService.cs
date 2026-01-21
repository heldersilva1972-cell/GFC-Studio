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
    Task<bool> RevokeAllUserPasskeysAsync(int userId);
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
        // Generate a challenge for WebAuthn
        var challenge = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        
        // Get existing credentials to prevent double registration
        await using var context = await _contextFactory.CreateDbContextAsync();
        var existingCredentials = await context.UserPasskeys
            .Where(p => p.UserId == userId)
            .Select(p => new { id = p.CredentialId, type = "public-key" })
            .ToListAsync();

        var rpName = _configuration["Fido2:ServerName"] ?? "GFC System";
        
        // [FIX] Robust RP ID Detection
        // Prioritize the configured domain if available, as it's the most stable for WebAuthn
        string rpId = _configuration["Fido2:ServerDomain"];
        
        if (string.IsNullOrEmpty(rpId))
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                rpId = httpContext.Request.Host.Host;
                _logger.LogInformation("Using rpId from request: {RpId}", rpId);
            }
            else
            {
                rpId = "localhost";
                _logger.LogWarning("No HTTP context and no Fido2:ServerDomain config. Using fallback: {RpId}", rpId);
            }
        }
        else
        {
            _logger.LogInformation("Using rpId from config: {RpId}", rpId);
        }

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
                UserHandle = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(userId.ToString())),
                SignatureCounter = 0,
                AttestationFormat = "none",
                CreatedAtUtc = DateTime.UtcNow,
                AAGUID = null  // Set to null instead of Guid.Empty for nullable column
            };

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
}
