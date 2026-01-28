using GFC.Core.Models.Security;

namespace GFC.Core.Interfaces;

/// <summary>
/// Service for managing trusted device tokens
/// </summary>
public interface IDeviceTrustService
{
    Task<bool> ValidateDeviceTokenAsync(string token, int userId);
    Task<string> CreateDeviceTokenAsync(int userId, string userAgent, string ipAddress, int durationDays);
    Task<bool> RevokeDeviceTokenAsync(string token);
    Task RevokeAllUserDevicesAsync(int userId);
    Task CleanupExpiredTokensAsync();
    bool ValidateToken(string token);
    Task<bool> ValidateTokenAsync(string token);
    Task<int?> GetUserIdByTokenAsync(string token);
    Task<List<TrustedDevice>> GetDevicesForUserAsync(int userId);
    // Note: DeviceSessionDto will need to be moved to Core as well or represented differently if used here.
    // However, DeviceTrustService implementation is in BlazorServer. 
    // If we move the interface to Core, we must ensure all return types are available in Core.
    // TrustedDevice is in GFC.Core.Models.Security.
    // DeviceSessionDto is currently defined in DeviceTrustService.cs (BlazorServer).
    // We should move DeviceSessionDto to Core DTOs.
    
    Task<List<GFC.Core.DTOs.DeviceSessionDto>> GetAllActiveDevicesAsync(); 
    Task RevokeAllGlobalSessionsAsync();
    Task ResetMobileSetupAsync(int userId);
}
