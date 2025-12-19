using GFC.BlazorServer.Services.Controllers;

namespace GFC.BlazorServer.Services.Simulation;

/// <summary>
/// Service for managing simulation presets/scenarios.
/// </summary>
public class SimulationPresetService
{
    private readonly SimControllerStateStore _stateStore;
    private readonly ILogger<SimulationPresetService> _logger;

    public SimulationPresetService(
        SimControllerStateStore stateStore,
        ILogger<SimulationPresetService> logger)
    {
        _stateStore = stateStore ?? throw new ArgumentNullException(nameof(stateStore));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets all available simulation presets.
    /// </summary>
    public IReadOnlyList<SimulationPreset> GetPresets()
    {
        return new List<SimulationPreset>
        {
            new SimulationPreset
            {
                Key = "quiet-weekday",
                Name = "Quiet Weekday",
                Description = "Low activity: minimal card swipes, doors mostly closed, no alarms"
            },
            new SimulationPreset
            {
                Key = "busy-friday-night",
                Name = "Busy Friday Night",
                Description = "High activity: frequent card swipes, doors opening/closing often, occasional alarms"
            },
            new SimulationPreset
            {
                Key = "door-held-open",
                Name = "Door Held Open Problem",
                Description = "Simulates a door that's been held open too long, triggering alarms"
            }
        };
    }

    /// <summary>
    /// Applies a preset scenario to a controller's simulation state.
    /// </summary>
    public Task ApplyPresetAsync(int controllerId, string presetKey, CancellationToken cancellationToken = default)
    {
        // For now, presets are just metadata - actual behavior is controlled by the simulation engine
        // In future phases, this could configure event rates, door states, etc.
        _logger.LogInformation("Preset {PresetKey} applied to controller {ControllerId}", presetKey, controllerId);
        return Task.CompletedTask;
    }
}

/// <summary>
/// Represents a simulation preset/scenario.
/// </summary>
public class SimulationPreset
{
    public string Key { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}
