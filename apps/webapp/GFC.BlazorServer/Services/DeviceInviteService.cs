using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace GFC.BlazorServer.Services;

public interface IDeviceInviteService
{
    Task<string> CreateInviteTokenAsync(int userId, string? deviceName, int expiryHours = 1, int? targetStationId = null);
    Task<DeviceInviteToken?> ValidateTokenAsync(string token);
    Task<bool> MarkTokenAsUsedAsync(string token);
    Task CleanupExpiredInvitesAsync();
    Task RevokeAllUserInvitesAsync(int userId);
}

public class DeviceInviteService : IDeviceInviteService
{
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;
    private readonly ILogger<DeviceInviteService> _logger;

    public DeviceInviteService(
        IDbContextFactory<GfcDbContext> contextFactory,
        ILogger<DeviceInviteService> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    public async Task<string> CreateInviteTokenAsync(int userId, string? deviceName, int expiryHours = 1, int? targetStationId = null)
    {
        try
        {
            var token = GenerateSecureToken();
            
            await using var context = await _contextFactory.CreateDbContextAsync();
            
            var invite = new DeviceInviteToken
            {
                Token = token,
                UserId = userId,
                CreatedAtUtc = DateTime.UtcNow,
                ExpiresAtUtc = DateTime.UtcNow.AddHours(expiryHours),
                TargetDeviceName = deviceName,
                TargetStationId = targetStationId,
                IsRevoked = false
            };

            context.DeviceInviteTokens.Add(invite);
            await context.SaveChangesAsync();

            _logger.LogInformation("Created device invite token for User {UserId} ({DeviceName})", userId, deviceName);
            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating device invite token for User {UserId}", userId);
            throw;
        }
    }

    public async Task<DeviceInviteToken?> ValidateTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token)) return null;

        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            
            // [NEW] Support both full tokens and 8-char short codes (prefix match)
            var query = context.DeviceInviteTokens
                .Include(i => i.User)
                .Where(i => !i.IsRevoked && i.UsedAtUtc == null);
            
            DeviceInviteToken? invite;
            if (token.Length == 8)
            {
                invite = await query.FirstOrDefaultAsync(i => i.Token.StartsWith(token.ToLower()));
            }
            else
            {
                invite = await query.FirstOrDefaultAsync(i => i.Token == token);
            }

            if (invite == null) return null;

            if (invite.ExpiresAtUtc < DateTime.UtcNow)
            {
                _logger.LogWarning("Attempted to use expired device invite token");
                return null;
            }

            return invite;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating device invite token");
            return null;
        }
    }

    public async Task<bool> MarkTokenAsUsedAsync(string token)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var invite = await context.DeviceInviteTokens.FirstOrDefaultAsync(i => i.Token == token);
            
            if (invite != null)
            {
                invite.UsedAtUtc = DateTime.UtcNow;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking device invite token as used");
            return false;
        }
    }

    public async Task CleanupExpiredInvitesAsync()
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var expired = await context.DeviceInviteTokens
                .Where(i => i.ExpiresAtUtc < DateTime.UtcNow || i.IsRevoked || i.UsedAtUtc != null)
                .ToListAsync();

            if (expired.Any())
            {
                context.DeviceInviteTokens.RemoveRange(expired);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up device invite tokens");
        }
    }

    public async Task RevokeAllUserInvitesAsync(int userId)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var invites = await context.DeviceInviteTokens
                .Where(i => i.UserId == userId && i.UsedAtUtc == null && !i.IsRevoked)
                .ToListAsync();

            foreach (var invite in invites)
            {
                invite.IsRevoked = true;
            }

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking device invite tokens for user {UserId}", userId);
        }
    }

    private string GenerateSecureToken()
    {
        var bytes = new byte[32];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToHexString(bytes).ToLower();
    }
}
