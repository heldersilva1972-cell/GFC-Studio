using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Provides the current controller mode based on system settings.
/// Loads settings per scope to ensure fresh values.
/// </summary>
public class ControllerModeProvider : IControllerModeProvider
{
    private readonly IBlazorSystemSettingsService _systemSettingsService;
    private readonly ILogger<ControllerModeProvider> _logger;

    public ControllerModeProvider(IBlazorSystemSettingsService systemSettingsService, ILogger<ControllerModeProvider> logger)
    {
        _systemSettingsService = systemSettingsService ?? throw new ArgumentNullException(nameof(systemSettingsService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public bool UseRealControllers => true; // Simulation mode removed - always use real controllers
}

