using System.Threading.Tasks;
using GFC.Core.Models;

namespace GFC.BlazorServer.Services.Vpn;

public interface IVpnConfigurationService
{
    /// <summary>
    /// Generates the content of a WireGuard .conf file for the specified user and device.
    /// This includes the user's private key and peer server config.
    /// </summary>
    Task<string> GenerateConfigForUserAsync(int userId, string? deviceName = null, string? deviceType = null);
    
    /// <summary>
    /// Generates a new onboarding token for a user.
    /// </summary>
    Task<string> CreateOnboardingTokenAsync(int userId, int durationHours = 48);
    
    /// <summary>
    /// Validates an onboarding token. Returns the UserId if valid, or null.
    /// </summary>
    Task<int?> ValidateOnboardingTokenAsync(string token);
    
    /// <summary>
    /// Checks if VPN testing endpoint is reachable.
    /// </summary>
    Task<bool> TestVpnConnectionAsync();

    /// <summary>
    /// Gets or creates an active VPN profile for a user.
    /// </summary>
    Task<VpnProfile> GetOrCreateProfileAsync(int userId, string? deviceName = null, string? deviceType = null);

    /// <summary>
    /// Revokes a specific VPN profile.
    /// </summary>
    Task RevokeProfileAsync(int profileId, int revokedBy, string reason);
    
    /// <summary>
    /// Revokes all VPN access for a user.
    /// </summary>
    Task RevokeUserAccessAsync(int userId, int revokedBy, string reason);

    /// <summary>
    /// Revokes all VPN access for a user (Legacy/System).
    /// </summary>
    Task RevokeUserAccessAsync(int userId);

    /// <summary>
    /// Rotates the keys for a specific profile (invalidates old, generates new).
    /// </summary>
    Task<string> RotateKeysAsync(int profileId);

    /// <summary>
    /// Marks an onboarding token as used.
    /// </summary>
    Task SetTokenUsedAsync(string token);
}
