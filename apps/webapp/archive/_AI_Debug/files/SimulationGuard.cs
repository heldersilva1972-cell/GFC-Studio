using System;
using GFC.BlazorServer.Services;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services.Controllers;

/// <summary>
/// Provides a single place to enforce that dangerous controller operations are never executed while Simulation Mode is enabled.
/// </summary>
public interface ISimulationGuard
{
    /// <summary>
    /// Throws a <see cref="SimulationModeBlockedException"/> when Simulation Mode is enabled.
    /// </summary>
    /// <param name="operationName">Human-friendly operation name for logging/UI.</param>
    /// <param name="controllerId">Optional controller identifier for logging.</param>
    /// <param name="controllerSerialNumber">Optional controller serial number for logging.</param>
    Task EnsureNotSimulationAsync(string operationName, int? controllerId = null, uint? controllerSerialNumber = null);
}

internal sealed class SimulationGuard : ISimulationGuard
{
    private readonly ISystemSettingsService _systemSettingsService;
    private readonly ILogger<SimulationGuard> _logger;

    public SimulationGuard(
        ISystemSettingsService systemSettingsService,
        ILogger<SimulationGuard> logger)
    {
        _systemSettingsService = systemSettingsService ?? throw new ArgumentNullException(nameof(systemSettingsService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task EnsureNotSimulationAsync(string operationName, int? controllerId = null, uint? controllerSerialNumber = null)
    {
        var settings = await _systemSettingsService.GetAsync();
        if (settings.UseRealControllers)
        {
            return;
        }

        var message = $"This operation ({operationName}) is blocked while Simulation Mode is enabled. " +
                      "Switch to Real Controller Mode in System Settings to perform this action.";

        _logger.LogWarning(
            "Blocked dangerous operation {Operation} in Simulation Mode. ControllerId={ControllerId}, Serial={ControllerSerial}",
            operationName,
            controllerId,
            controllerSerialNumber);

        throw new SimulationModeBlockedException(message);
    }
}

