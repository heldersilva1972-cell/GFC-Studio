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
    /// Updates the UseRealControllers setting.
    /// </summary>
    Task SetUseRealControllersAsync(bool value);
}

