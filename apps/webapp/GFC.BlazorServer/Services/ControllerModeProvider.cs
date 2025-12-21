using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Provides the current controller mode.
/// After simulation mode purge, this always returns true (real controllers only).
/// </summary>
public class ControllerModeProvider : IControllerModeProvider
{
    private readonly ILogger<ControllerModeProvider> _logger;

    public ControllerModeProvider(ILogger<ControllerModeProvider> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Always returns true - application now operates in real controller mode only.
    /// Simulation mode has been purged.
    /// </summary>
    public bool UseRealControllers => true;
}
