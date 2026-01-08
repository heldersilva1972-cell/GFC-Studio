using GFC.BlazorServer.Data;
using GFC.Core.Models.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Service for managing trusted device tokens
/// </summary>
public interface IDeviceTrustService
{
    Task<bool> ValidateDeviceTokenAsync(string token, int userId);
    Task<string> CreateDeviceTokenAsync(int userId, string userAgent, string ipAddress, int durationDays);
    Task RevokeDeviceTokenAsync(string token);
    Task CleanupExpiredTokensAsync();
    bool ValidateToken(string token); // For middleware - validates token exists and is not expired/revoked
    Task<int?> GetUserIdByTokenAsync(string token);
    Task<List<TrustedDevice>> GetDevicesForUserAsync(int userId);
}

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
    public async Task RevokeDeviceTokenAsync(string token)
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
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking device token");
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

            return device != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating device token");
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
}
