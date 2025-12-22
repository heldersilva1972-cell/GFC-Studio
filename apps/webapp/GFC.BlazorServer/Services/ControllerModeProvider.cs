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

    public bool UseRealControllers => true; // Simulation mode removed - always use real controllers
}

