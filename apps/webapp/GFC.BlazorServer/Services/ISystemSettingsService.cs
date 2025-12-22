using GFC.BlazorServer.Data.Entities;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Service for managing system-wide settings.
/// </summary>
public interface ISystemSettingsService
{
    /// <summary>
    /// Gets the system settings. Creates default settings if none exist.
    /// </summary>
    Task<SystemSettings> GetAsync();

    /// <summary>
    /// Gets the system settings synchronously. Creates default settings if none exist.
    /// </summary>
    SystemSettings GetSettings();

    /// <summary>
    /// Updates NVR credentials for camera auto-discovery.
    /// </summary>
    Task UpdateNvrCredentialsAsync(string nvrIpAddress, int nvrPort, string username, string password);
}

