using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Vpn;

public interface IVpnConfigurationService
{
    /// <summary>
    /// Generates the content of a WireGuard .conf file for the specified user.
    /// This includes the user's private key (simulated/retrieved) and peer server config.
    /// </summary>
    Task<string> GenerateConfigForUserAsync(int userId);
    
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
    /// Revokes VPN access for a user (removes keys/access).
    /// </summary>
    Task RevokeUserAccessAsync(int userId);
}
