using GFC.BlazorServer.Data;
using GFC.Core.Models.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Collections.Concurrent;

using GFC.Core.Interfaces;
using GFC.Core.DTOs;

namespace GFC.BlazorServer.Services;

// Interface moved to GFC.Core.Interfaces

// DTO moved to GFC.Core.DTOs

public class DeviceTrustService : IDeviceTrustService
{
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;
    private readonly ILogger<DeviceTrustService> _logger;
    private static readonly ConcurrentDictionary<string, (string Token, DateTime Expiry)> _pendingSetupCodes = new();

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
                await ExtendTokenLifeAsync(device, context);
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
            
            // [FIXED] Smart Token Rotation: Revoke existing active tokens of the same type (e.g. Android)
            // This prevents cluttering the "Cast Activity" Feed with multiple entries for the same phone.
            var existingTokens = await context.TrustedDevices
                .Where(d => d.UserId == userId && !d.IsRevoked && d.ExpiresAtUtc > DateTime.UtcNow)
                .ToListAsync();

            if (existingTokens.Any())
            {
                var platform = userAgent?.Contains("Android") == true ? "Android" : 
                             userAgent?.Contains("iPhone") == true ? "iPhone" : "Browser";

                var matches = existingTokens.Where(t => 
                    (platform == "Android" && t.UserAgent.Contains("Android")) ||
                    (platform == "iPhone" && t.UserAgent.Contains("iPhone")) ||
                    (platform == "Browser" && !t.UserAgent.Contains("Android") && !t.UserAgent.Contains("iPhone"))
                ).ToList();

                foreach (var oldToken in matches)
                {
                    oldToken.IsRevoked = true;
                }
                
                if (matches.Any()) {
                    _logger.LogInformation("Auto-cleanup: Revoked {Count} old {Type} sessions for user {UserId}.", matches.Count, platform, userId);
                }
            }


            
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

    public async Task<string> CreateStationTokenAsync(int authorizedByUserId, string userAgent, string ipAddress, int durationDays, string? stationName = null, string? loginMode = null, string? authorizedUserIdsCsv = null)
    {
        try
        {
            var token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");

            await using var context = await _contextFactory.CreateDbContextAsync();
        
        // [MODIFIED] Do NOT auto-revoke existing stations from this IP.
        // This allows multiple physical computers at the same club location (shared public IP) to coexist.

        var device = new TrustedDevice
        {
            UserId = authorizedByUserId, // Record who authorized this station
            DeviceToken = token,
            UserAgent = userAgent?.Length > 256 ? userAgent.Substring(0, 256) : userAgent,
            IpAddress = ipAddress?.Length > 45 ? ipAddress.Substring(0, 45) : ipAddress,
            LastUsedUtc = DateTime.UtcNow,
            ExpiresAtUtc = DateTime.UtcNow.AddYears(50), // [FIX] Stations have permanent trust (50 years)
            IsRevoked = false,
            IsStation = true,
            StationName = stationName,
            LoginMode = loginMode ?? "Standard",
            AuthorizedUserIdsCsv = authorizedUserIdsCsv
        };

        context.TrustedDevices.Add(device);
        await context.SaveChangesAsync();

            _logger.LogInformation("Created STATION trust token '{StationName}' authorized by {UserId}, expires {ExpiresAt}", 
                stationName ?? "Unnamed", authorizedByUserId, device.ExpiresAtUtc);

            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating station token");
            throw;
        }
    }

    public async Task<List<TrustedDevice>> GetAllActiveStationsAsync()
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.TrustedDevices
                .Where(d => d.IsStation && !d.IsRevoked && d.ExpiresAtUtc > DateTime.UtcNow)
                .OrderBy(d => d.StationName)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active stations");
            return new List<TrustedDevice>();
        }
    }

    public async Task<bool> IsStationTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token)) return false;
        
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.TrustedDevices
                .AnyAsync(d => d.DeviceToken == token && d.IsStation && !d.IsRevoked && d.ExpiresAtUtc > DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if token is station");
            return false;
        }
    }

    public async Task<TrustedDevice?> GetDeviceByTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token)) return null;
        
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.TrustedDevices
                .FirstOrDefaultAsync(d => d.DeviceToken == token && !d.IsRevoked && d.ExpiresAtUtc > DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting device by token");
            return null;
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
                
                // [FIX] Invalidate the memory cache for this specific token to force logout
                CustomAuthenticationStateProvider.InvalidateToken(token);
                
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
            
            if (device != null)
            {
                await ExtendTokenLifeAsync(device, context);
                return true;
            }

            return false;
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
            
            // BUG FIX: Fetch devices first, then project in memory
            // This avoids EF translation errors with complex string interpolation and null checks
            var devices = await context.TrustedDevices
                                .Include(d => d.User)
                                .Where(d => !d.IsRevoked && d.ExpiresAtUtc > DateTime.UtcNow)
                                .OrderByDescending(d => d.LastUsedUtc)
                                .ToListAsync();
            
            var sessions = devices.Select(d => new DeviceSessionDto
            {
                UserId = d.UserId,
                Username = d.IsStation 
                    ? $"Station Device (Auth by: {(d.User != null ? d.User.Username : "Unknown")})"
                    : (d.User != null ? d.User.Username : "Deleted User"),
                DeviceToken = d.DeviceToken,
                UserAgent = d.UserAgent,
                IpAddress = d.IpAddress,
                LastUsedUtc = d.LastUsedUtc,
                ExpiresAtUtc = d.ExpiresAtUtc,
                IsRevoked = d.IsRevoked,
                IsStation = d.IsStation,
                StationName = d.StationName
            }).ToList();
            
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

            // 1. Delete Personal Trusted Devices (Leave Stations)
            var devices = await context.TrustedDevices
                                .Where(d => d.UserId == userId && !d.IsStation)
                                .ToListAsync();
            
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
    private async Task ExtendTokenLifeAsync(TrustedDevice device, GfcDbContext context)
    {
        try
        {
            // Update last used time
            device.LastUsedUtc = DateTime.UtcNow;

            // Rolling Trust: Extend expiration based on system settings
            // [STATION MODE] Shared stations stay authorized for 1 year (365 days) from last use.
            // [USER MODE] Personal devices follow the system setting (default 30 days).
            var settings = await context.SystemSettings.FirstOrDefaultAsync(s => s.Id == 1);
            int durationDays = device.IsStation ? 365 : (settings?.TrustedDeviceDurationDays ?? 30);
 
            var newExpiration = DateTime.UtcNow.AddDays(durationDays);
            
            // Optimization: Only update the DB if the expiration has moved forward significantly (more than 1 day)
            // or if we are nearing the current expiration (less than 90% of duration left).
            // This prevents excessive DB writes on every single page navigation.
            if (device.ExpiresAtUtc < DateTime.UtcNow.AddDays(durationDays * 0.9))
            {
                device.ExpiresAtUtc = newExpiration;
                _logger.LogDebug("Rolling trust: Extended device token for user {UserId} to {ExpiresAt}", device.UserId, newExpiration);
            }

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to extend token life for device {DeviceId}", device.Id);
        }
    }

    public void InvalidateUserSession(int userId)
    {
        CustomAuthenticationStateProvider.InvalidateUser(userId);
    }

    public void InvalidateTokenSession(string token)
    {
        CustomAuthenticationStateProvider.InvalidateToken(token);
    }

    public void InvalidateAllUserSessions()
    {
        CustomAuthenticationStateProvider.InvalidateAll();
    }

    public async Task UpdateDeviceAsync(TrustedDevice device)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.TrustedDevices.Update(device);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update device {DeviceId}", device.Id);
            throw;
        }
    }

    public async Task<string?> GenerateSetupCodeAsync(string deviceToken)
    {
        // 1. Generate an 8-digit code (e.g. 1234 5678)
        var randomBytes = new byte[4];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        var code = (BitConverter.ToUInt32(randomBytes, 0) % 100000000).ToString("D8");
        var formattedCode = $"{code.Substring(0, 4)}-{code.Substring(4, 4)}";

        // 2. Clear stale codes
        var staleCodes = _pendingSetupCodes.Where(x => x.Value.Expiry < DateTime.UtcNow).Select(x => x.Key).ToList();
        foreach (var sc in staleCodes) _pendingSetupCodes.TryRemove(sc, out _);

        // 3. Store with 15 minute expiry
        _pendingSetupCodes[formattedCode] = (deviceToken, DateTime.UtcNow.AddMinutes(15));
        
        _logger.LogInformation("Generated Setup Recovery Code {Code} for token {TokenSnippet}", 
            formattedCode, deviceToken.Substring(0, 8));

        return await Task.FromResult(formattedCode);
    }

    public async Task<string?> ValidateSetupCodeAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code)) return null;
        
        var normalized = code.Trim().Replace(" ", "-");
        if (!normalized.Contains("-") && normalized.Length == 8)
        {
            normalized = $"{normalized.Substring(0, 4)}-{normalized.Substring(4, 4)}";
        }

        if (_pendingSetupCodes.TryRemove(normalized, out var data))
        {
            if (data.Expiry > DateTime.UtcNow)
            {
                return await Task.FromResult(data.Token);
            }
        }
        
        return await Task.FromResult<string?>(null);
    }
}
