using GFC.BlazorServer.Data.Entities;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Service for managing system-wide settings.
/// </summary>
public interface IBlazorSystemSettingsService
{
    /// <summary>
    /// Gets the system settings. Creates default settings if none exist.
    /// </summary>
    Task<SystemSettings> GetAsync();
    Task<SystemSettings> GetSystemSettingsAsync();

    /// <summary>
    /// Gets the system settings synchronously. Creates default settings if none exist.
    /// </summary>
    SystemSettings GetSettings();

    /// <summary>
    /// Updates NVR credentials for camera auto-discovery.
    /// </summary>
    Task UpdateNvrCredentialsAsync(string nvrIpAddress, int nvrPort, string username, string password);

    /// <summary>
    /// Updates the system settings.
    /// </summary>
    /// <param name="settings">The system settings object with updated values.</param>
    Task UpdateAsync(SystemSettings settings);
}

