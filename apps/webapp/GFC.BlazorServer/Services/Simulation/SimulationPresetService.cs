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

    private CancellationTokenSource? _scenarioCancellation;
    private Task? _scenarioTask;

    /// <summary>
    /// Applies a preset scenario to a controller's simulation state.
    /// </summary>
    public async Task ApplyPresetAsync(int controllerId, string presetKey, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Applying preset {PresetKey} to controller {ControllerId}", presetKey, controllerId);

        // Stop any running scenario first
        await StopCurrentScenarioAsync();

        switch (presetKey)
        {
            case "quiet-weekday":
                await ApplyQuietWeekdayAsync(controllerId);
                break;
            
            case "busy-friday-night":
                await StartBusyFridayNightAsync(controllerId);
                break;
            
            case "door-held-open":
                await ApplyDoorHeldOpenAsync(controllerId);
                break;
            
            default:
                _logger.LogWarning("Unknown preset key: {PresetKey}", presetKey);
                break;
        }
    }

    /// <summary>
    /// Stops any currently running scenario.
    /// </summary>
    public async Task StopCurrentScenarioAsync()
    {
        if (_scenarioCancellation != null)
        {
            _scenarioCancellation.Cancel();
            _scenarioCancellation.Dispose();
            _scenarioCancellation = null;
        }

        if (_scenarioTask != null)
        {
            try
            {
                await _scenarioTask;
            }
            catch (OperationCanceledException)
            {
                // Expected when cancelling
            }
            _scenarioTask = null;
        }
    }

    private Task ApplyQuietWeekdayAsync(int controllerId)
    {
        // Set all doors to "Card Only" mode, silence alarms
        var state = _stateStore.GetOrCreate(controllerId.ToString());
        foreach (var door in state.Doors.Values)
        {
            door.IsLocked = true;
            door.IsDoorOpen = false;
        }
        
        _logger.LogInformation("Quiet Weekday preset applied - all doors locked and closed");
        return Task.CompletedTask;
    }

    private Task StartBusyFridayNightAsync(int controllerId)
    {
        _scenarioCancellation = new CancellationTokenSource();
        var token = _scenarioCancellation.Token;

        _scenarioTask = Task.Run(async () =>
        {
            _logger.LogInformation("Starting Busy Friday Night scenario for controller {ControllerId}", controllerId);
            
            var random = new Random();
            var state = _stateStore.GetOrCreate(controllerId.ToString());

            while (!token.IsCancellationRequested)
            {
                try
                {
                    // Random delay between 2-5 seconds
                    await Task.Delay(random.Next(2000, 5000), token);

                    // Pick a random door
                    var doorKeys = state.Doors.Keys.ToList();
                    if (doorKeys.Any())
                    {
                        var randomDoorIndex = doorKeys[random.Next(doorKeys.Count)];
                        var door = state.Doors[randomDoorIndex];

                        // Simulate card swipe -> unlock -> open -> close -> lock sequence
                        _logger.LogDebug("Busy Friday: Simulating activity on door {DoorIndex}", randomDoorIndex);
                        
                        door.IsLocked = false;
                        await Task.Delay(500, token);
                        
                        door.IsDoorOpen = true;
                        await Task.Delay(random.Next(1000, 3000), token);
                        
                        door.IsDoorOpen = false;
                        await Task.Delay(300, token);
                        
                        door.IsLocked = true;
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Busy Friday Night scenario");
                }
            }

            _logger.LogInformation("Busy Friday Night scenario stopped for controller {ControllerId}", controllerId);
        }, token);

        return Task.CompletedTask;
    }

    private async Task ApplyDoorHeldOpenAsync(int controllerId)
    {
        var state = _stateStore.GetOrCreate(controllerId.ToString());
        var firstDoor = state.Doors.Values.FirstOrDefault();
        
        if (firstDoor != null)
        {
            _logger.LogInformation("Door Held Open preset - opening door {DoorIndex}", firstDoor.DoorIndex);
            
            firstDoor.IsLocked = false;
            firstDoor.IsDoorOpen = true;
            
            // In a real scenario, this would trigger an alarm after a timeout
            // For now, just leave the door open
        }
        
        await Task.CompletedTask;
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
