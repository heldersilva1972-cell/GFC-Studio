using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Provides the current controller mode based on system settings.
/// Loads settings per scope to ensure fresh values.
/// </summary>
public class ControllerModeProvider : IControllerModeProvider
{
    private readonly ISystemSettingsService _systemSettingsService;
    private readonly ILogger<ControllerModeProvider> _logger;
    private bool? _cachedValue;
    private DateTime _cacheTime = DateTime.MinValue;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(5);

    public ControllerModeProvider(ISystemSettingsService systemSettingsService, ILogger<ControllerModeProvider> logger)
    {
        _systemSettingsService = systemSettingsService ?? throw new ArgumentNullException(nameof(systemSettingsService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public bool UseRealControllers
    {
        get
        {
            // Use a short cache to avoid hitting the DB on every property access
            if (_cachedValue.HasValue && DateTime.UtcNow - _cacheTime < CacheDuration)
            {
                return _cachedValue.Value;
            }

            try
            {
                // Load synchronously for property access (settings should be fast)
                // Use ConfigureAwait(false) to avoid deadlocks
                // Add timeout to prevent hanging
                var settingsTask = _systemSettingsService.GetAsync();
                var completedTask = Task.WhenAny(settingsTask, Task.Delay(TimeSpan.FromSeconds(3))).GetAwaiter().GetResult();
                
                if (completedTask == settingsTask && settingsTask.IsCompletedSuccessfully)
                {
                    var settings = settingsTask.GetAwaiter().GetResult();
                    _cachedValue = settings.UseRealControllers;
                    _cacheTime = DateTime.UtcNow;
                    return _cachedValue.Value;
                }
                else
                {
                    _logger.LogWarning("SystemSettings load timed out, defaulting to simulation mode");
                    _cachedValue = false;
                    _cacheTime = DateTime.UtcNow;
                    return false;
                }
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                _logger.LogWarning(ex, "Database update error loading system settings, defaulting to simulation mode");
                _cachedValue = false;
                _cacheTime = DateTime.UtcNow;
                return false;
            }
            catch (System.TimeoutException)
            {
                _logger.LogWarning("SystemSettings load timed out, defaulting to simulation mode");
                _cachedValue = false;
                _cacheTime = DateTime.UtcNow;
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load system settings, defaulting to simulation mode");
                // Default to simulation mode (false) on error
                _cachedValue = false;
                _cacheTime = DateTime.UtcNow;
                return false;
            }
        }
    }
}

