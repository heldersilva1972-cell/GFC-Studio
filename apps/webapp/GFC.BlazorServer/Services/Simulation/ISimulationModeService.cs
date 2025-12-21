namespace GFC.BlazorServer.Services.Simulation;

/// <summary>
/// Service that provides information about the current simulation mode.
/// </summary>
public interface ISimulationModeService
{
    /// <summary>
    /// True if real controllers are being used, false if simulation mode is active.
    /// </summary>
    bool UseRealControllers { get; }

    /// <summary>
    /// True if simulation mode is active.
    /// </summary>
    bool IsSimulation => !UseRealControllers;

    /// <summary>
    /// Human-readable label for the current mode.
    /// </summary>
    string ModeLabel { get; }
}
