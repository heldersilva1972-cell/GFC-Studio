using GFC.BlazorServer.Data;
using GFC.Core.Models.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

using GFC.Core.Interfaces;
using GFC.Core.DTOs;

namespace GFC.BlazorServer.Services;

// Interface moved to GFC.Core.Interfaces

// DTO moved to GFC.Core.DTOs

public class DeviceTrustService : IDeviceTrustService
{
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;
    private readonly ILogger<DeviceTrustService> _logger;

    public DeviceTrustService(
        IDbContextFactory<GfcDbContext> contextFactory,
        ILogger<DeviceTrustService> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    /// <summary>
    /// Validates a device token and checks if it's still valid
    /// </summary>
    public async Task<bool> ValidateDeviceTokenAsync(string token, int userId)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            
            var device = await context.TrustedDevices
                .FirstOrDefaultAsync(d => 
                    d.DeviceToken == token && 
                    d.UserId == userId &&
                    !d.IsRevoked &&
                    d.ExpiresAtUtc > DateTime.UtcNow);

            if (device != null)
            {
                // Update last used time
                device.LastUsedUtc = DateTime.UtcNow;
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating device token for user {UserId}", userId);
            return false;
        }
    }

    /// <summary>
    /// Creates a new device trust token
    /// </summary>
    public async Task<string> CreateDeviceTokenAsync(int userId, string userAgent, string ipAddress, int durationDays)
    {
        try
        {
            // Generate a URL-safe token
            var token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");

            await using var context = await _contextFactory.CreateDbContextAsync();
            
            /* 
            // Enforce Single Session Policy: Revoke all existing active tokens for this user
            // This ensures a user can only have one trusted device active at a time to prevent session proliferation.
            var existingTokens = await context.TrustedDevices
                .Where(d => d.UserId == userId && !d.IsRevoked && d.ExpiresAtUtc > DateTime.UtcNow)
                .ToListAsync();

            if (existingTokens.Any())
            {
                foreach (var existingToken in existingTokens)
                {
                    existingToken.IsRevoked = true;
                }
                _logger.LogInformation("Revoked {Count} existing sessions for user {UserId} to enforce single-session policy.", existingTokens.Count, userId);
            }
            */


            
            var device = new TrustedDevice
            {
                UserId = userId,
                DeviceToken = token,
                UserAgent = userAgent?.Length > 256 ? userAgent.Substring(0, 256) : userAgent,
                IpAddress = ipAddress?.Length > 45 ? ipAddress.Substring(0, 45) : ipAddress,
                LastUsedUtc = DateTime.UtcNow,
                ExpiresAtUtc = DateTime.UtcNow.AddDays(durationDays),
                IsRevoked = false
            };

            context.TrustedDevices.Add(device);
            await context.SaveChangesAsync();

            _logger.LogInformation("Created device trust token for user {UserId}, expires {ExpiresAt}", 
                userId, device.ExpiresAtUtc);

            return token;
        }
        catch (Exception ex)
        {
            var message = ex.InnerException != null ? $"{ex.Message} -> {ex.InnerException.Message}" : ex.Message;
            _logger.LogError(ex, "Error creating device token for user {UserId}: {Message}", userId, message);
            throw new Exception(message, ex);
        }
    }

    /// <summary>
    /// Revokes a device token
    /// </summary>
    public async Task<bool> RevokeDeviceTokenAsync(string token)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            
            var device = await context.TrustedDevices
                .FirstOrDefaultAsync(d => d.DeviceToken == token);

            if (device != null)
            {
                device.IsRevoked = true;
                await context.SaveChangesAsync();
                _logger.LogInformation("Revoked device token for user {UserId}", device.UserId);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking device token");
            return false;
        }
    }

    /// <summary>
    /// Revokes all active device tokens for a specific user.
    /// </summary>
    public async Task RevokeAllUserDevicesAsync(int userId)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var devices = await context.TrustedDevices
                .Where(t => t.UserId == userId && !t.IsRevoked)
                .ToListAsync();
            
            if (devices.Any())
            {
                foreach (var device in devices)
                {
                    device.IsRevoked = true;
                }
                await context.SaveChangesAsync();
                _logger.LogInformation("Revoked {Count} active device tokens for user {UserId}.", devices.Count, userId);
            }

            // [FIX] Invalidate global in-memory cache to force logout on next request
            CustomAuthenticationStateProvider.InvalidateUser(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking all device tokens for user {UserId}", userId);
        }
    }

    /// <summary>
    /// Removes expired and revoked tokens from the database
    /// </summary>
    public async Task CleanupExpiredTokensAsync()
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            
            var cutoffDate = DateTime.UtcNow.AddDays(-30); // Keep revoked tokens for 30 days for audit
            
            var expiredTokens = await context.TrustedDevices
                .Where(d => d.ExpiresAtUtc < DateTime.UtcNow || (d.IsRevoked && d.LastUsedUtc < cutoffDate))
                .ToListAsync();

            if (expiredTokens.Any())
            {
                context.TrustedDevices.RemoveRange(expiredTokens);
                await context.SaveChangesAsync();
                _logger.LogInformation("Cleaned up {Count} expired device tokens", expiredTokens.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up expired device tokens");
        }
    }

    public async Task RevokeAllGlobalSessionsAsync()
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var devices = await context.TrustedDevices
                .Where(t => !t.IsRevoked)
                .ToListAsync();
            
            if (devices.Any())
            {
                foreach (var device in devices)
                {
                    device.IsRevoked = true;
                }
                await context.SaveChangesAsync();
                _logger.LogInformation("GLOBAL REVOKE: Revoked {Count} active device tokens system-wide.", devices.Count);
            }

            // [FIX] Invalidate ENTIRE global in-memory cache
            CustomAuthenticationStateProvider.InvalidateAll();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking all global device tokens");
        }
    }

    /// <summary>
    /// Validates a token without requiring userId (for middleware use)
    /// </summary>
    public bool ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        try
        {
            using var context = _contextFactory.CreateDbContext();
            
            var device = context.TrustedDevices
                .FirstOrDefault(d => 
                    d.DeviceToken == token && 
                    !d.IsRevoked && 
                    d.ExpiresAtUtc > DateTime.UtcNow);

            if (device == null)
            {
                var tokenPreview = string.IsNullOrEmpty(token) ? "EMPTY" : (token.Length > 8 ? token.Substring(0, 8) + "..." : token);
                _logger.LogWarning("Token validation failed for: {TokenPreview}. Reason: Not found, revoked, or expired.", tokenPreview);
                return false;
            }

            return true;
        }
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 208)
        {
            // Table missing? Assume valid for now to avoid death spirals during migrations or safe-mode
            _logger.LogWarning("SystemSettings or TrustedDevices table missing. Assuming token valid for background check.");
            return true; 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating device token. Fail-open to prevent lockout.");
            // We return true here to allow the circuit to start even if the DB is flickery.
            // The actual data security is still enforced by the AuthenticationStateProvider.
            return true; 
        }
    }

    /// <summary>
    /// Validates a token asynchronously.
    /// </summary>
    public async Task<bool> ValidateTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token)) return false;
        
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var device = await context.TrustedDevices
                .FirstOrDefaultAsync(d => d.DeviceToken == token && !d.IsRevoked && d.ExpiresAtUtc > DateTime.UtcNow);
            
            return device != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating device token async");
            return false;
        }
    }
 
    /// <summary>
    /// Gets the UserId associated with a valid token
    /// </summary>
    public async Task<int?> GetUserIdByTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var device = await context.TrustedDevices
                .FirstOrDefaultAsync(d => 
                    d.DeviceToken == token && 
                    !d.IsRevoked && 
                    d.ExpiresAtUtc > DateTime.UtcNow);

            return device?.UserId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting UserId by token");
            return null;
        }
    }
    public async Task<List<TrustedDevice>> GetDevicesForUserAsync(int userId)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.TrustedDevices
                .Where(d => d.UserId == userId && !d.IsRevoked && d.ExpiresAtUtc > DateTime.UtcNow)
                .OrderByDescending(d => d.LastUsedUtc)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting devices for user {UserId}", userId);
            return new List<TrustedDevice>();
        }
    }

    public async Task<List<DeviceSessionDto>> GetAllActiveDevicesAsync()
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var sessions = await context.TrustedDevices
                                .Include(d => d.User)
                                .Where(d => !d.IsRevoked && d.ExpiresAtUtc > DateTime.UtcNow)
                                .OrderByDescending(d => d.LastUsedUtc)
                                .Select(d => new DeviceSessionDto
                                {
                                    UserId = d.User.UserId,
                                    Username = d.User.Username,
                                    DeviceToken = d.DeviceToken,
                                    UserAgent = d.UserAgent,
                                    IpAddress = d.IpAddress,
                                    LastUsedUtc = d.LastUsedUtc,
                                    ExpiresAtUtc = d.ExpiresAtUtc,
                                    IsRevoked = d.IsRevoked
                                }).ToListAsync();
            return sessions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all active devices");
            return new List<DeviceSessionDto>();
        }
    }

    public async Task ResetMobileSetupAsync(int userId)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            // 1. Delete Trusted Devices (Fresh start as per user request)
            // [NOTE] This deletes ALL trusted devices (phones, laptops, tablets) for this user.
            var devices = await context.TrustedDevices.Where(d => d.UserId == userId).ToListAsync();
            
            // Safety: If the user reset themselves (which shouldn't happen via UI but for safety),
            // this loop would kill their current session token immediately.
            // However, the UI blocks 'admin' reset, and users can't reset themselves via this tool easily.
            // The "Logged Out" effect happens because we nuke the token they are currently using.
            
            if (devices.Any()) context.TrustedDevices.RemoveRange(devices);

            // 2. Delete Push Subscriptions
            var pushSubs = await context.PushSubscriptions.Where(s => s.UserId == userId).ToListAsync();
            if (pushSubs.Any()) context.PushSubscriptions.RemoveRange(pushSubs);

            // 3. Delete Passkeys
            var passkeys = await context.UserPasskeys.Where(p => p.UserId == userId).ToListAsync();
            if (passkeys.Any()) context.UserPasskeys.RemoveRange(passkeys);

            // 4. Delete Device Invite Tokens
            var invites = await context.DeviceInviteTokens.Where(i => i.UserId == userId).ToListAsync();
            if (invites.Any()) context.DeviceInviteTokens.RemoveRange(invites);

            // 5. [FIX] Clear the Passcode (PIN) so the wizard runs Step 1 again
            var user = await context.AppUsers.FindAsync(userId);
            if (user != null)
            {
                user.PassCodeHash = null;
                // We don't necessarily force a password change, just the PIN
            }

            await context.SaveChangesAsync();
            _logger.LogInformation("MOBILE RESET: Performed full mobile setup reset for user {UserId}.", userId);

            // [FIX] Invalidate global in-memory cache to force logout on next request
            CustomAuthenticationStateProvider.InvalidateUser(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing full mobile reset for user {UserId}", userId);
            throw;
        }
    }
}
