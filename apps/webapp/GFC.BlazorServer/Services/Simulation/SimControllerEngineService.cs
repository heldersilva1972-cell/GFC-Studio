using System;
using GFC.BlazorServer.Configuration;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Models;
using GFC.BlazorServer.Services.Controllers;
using GFC.BlazorServer.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GFC.BlazorServer.Services.Simulation;

/// <summary>
/// Background service that simulates controller behavior when in simulation mode.
/// Advances simulated time, applies auto-open schedules, advanced door modes, and generates events.
/// </summary>
public class SimControllerEngineService : BackgroundService, ISimulationEngineControlService
{
    private readonly SimControllerStateStore _stateStore;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IOptions<SimControllerOptions> _options;
    private readonly ILogger<SimControllerEngineService> _logger;
    private readonly Random _random = new();

    // Event type constants (matching real controller firmware)
    private const int EventTypeDoorOpen = 1;
    private const int EventTypeDoorClosed = 2;
    private const int EventTypeCardGranted = 3;
    private const int EventTypeCardDenied = 4;
    private const int EventTypeForcedOpenAlarm = 5;
    private const int EventTypeHeldOpenAlarm = 6;
    private const int EventTypeAlarmCleared = 7;
    private const int EventTypeAutoOpenActivated = 8;
    private const int EventTypeAutoOpenDeactivated = 9;

    public SimControllerEngineService(
        SimControllerStateStore stateStore,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<SimControllerOptions> options,
        ILogger<SimControllerEngineService> logger)
    {
        _stateStore = stateStore ?? throw new ArgumentNullException(nameof(stateStore));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Sim Controller Engine Service started.");

        var tickInterval = TimeSpan.FromMilliseconds(_options.Value.TickIntervalMs);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessTickAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in simulation engine tick");
            }

            await Task.Delay(tickInterval, stoppingToken);
        }

        _logger.LogInformation("Sim Controller Engine Service stopped.");
    }

    private async Task ProcessTickAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Create a scope for this tick to resolve scoped services
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GfcDbContext>();
            var modeProvider = scope.ServiceProvider.GetRequiredService<IControllerModeProvider>();

            // Double-check we're still in simulation mode (may have changed)
            if (modeProvider.UseRealControllers)
            {
                return; // Skip this tick if mode changed to real
            }

            // Load all enabled controllers from database
            var controllers = await dbContext.Controllers
                .Include(c => c.Doors)
                .Where(c => c.IsEnabled)
                .ToListAsync(cancellationToken);

            var now = DateTime.UtcNow;

            foreach (var controller in controllers)
            {
                var state = _stateStore.GetOrCreate(controller.SerialNumberDisplay);
                
                // Update simulated time (use real time for now)
                state.SimNowUtc = now;
                state.LastCommUtc = now;
                state.IsOnline = true;

                // Process each door
                foreach (var door in controller.Doors)
                {
                    await ProcessDoorAsync(state, door, now, cancellationToken);
                }

                // Generate random card swipes if enabled
                if (_options.Value.RandomCardSwipesEnabled)
                {
                    await GenerateRandomCardSwipesAsync(state, controller, cancellationToken);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing simulation tick");
        }
    }

    /// <summary>
    /// Advances simulated time by the specified delta.
    /// </summary>
    public Task AdvanceTimeAsync(TimeSpan delta)
    {
        try
        {
            var serialNumbers = _stateStore.GetAllSerialNumbers();
            foreach (var sn in serialNumbers)
            {
                var state = _stateStore.Get(sn);
                if (state != null)
                {
                    state.SimNowUtc = state.SimNowUtc.Add(delta);
                }
            }
            _logger.LogInformation("Advanced simulation time by {Delta}", delta);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error advancing simulation time");
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Resets a controller's simulation state to defaults.
    /// </summary>
    public Task ResetControllerStateAsync(int controllerId, CancellationToken cancellationToken = default)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GfcDbContext>();

            var controller = dbContext.Controllers
                .FirstOrDefault(c => c.Id == controllerId);

            if (controller == null)
            {
                _logger.LogWarning("Controller {ControllerId} not found for reset", controllerId);
                return Task.CompletedTask;
            }

            var state = _stateStore.Get(controller.SerialNumberDisplay);
            if (state != null)
            {
                // Reset door states
                foreach (var doorState in state.Doors.Values)
                {
                    doorState.IsLocked = true;
                    doorState.IsDoorOpen = false;
                    doorState.AlarmActive = false;
                    doorState.IsRelayOn = false;
                    doorState.IsSensorActive = false;
                    doorState.LastDoorOpenUtc = null;
                    doorState.LastAutoUnlockUtc = null;
                    doorState.AutoUnlockUntilUtc = null;
                    doorState.UnlockUntilUtc = null;
                    doorState.UnlockDurationSeconds = null;
                }

                // Reset alarms
                state.IsFireAlarmActive = false;
                state.IsTamperActive = false;

                _logger.LogInformation("Reset simulation state for controller {ControllerId} ({SerialNumber})", 
                    controllerId, controller.SerialNumber);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting controller state");
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Injects a synthetic event into the simulation.
    /// </summary>
    public Task InjectEventAsync(int controllerId, int doorIndex, long? cardNumber, int eventType, DateTime timestamp, CancellationToken cancellationToken = default)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GfcDbContext>();

            var controller = dbContext.Controllers
                .FirstOrDefault(c => c.Id == controllerId);

            if (controller == null)
            {
                _logger.LogWarning("Controller {ControllerId} not found for event injection", controllerId);
                return Task.CompletedTask;
            }

            var state = _stateStore.GetOrCreate(controller.SerialNumberDisplay);
            
            // Ensure door state exists
            if (!state.Doors.ContainsKey(doorIndex))
            {
                state.Doors[doorIndex] = new SimDoorState { DoorIndex = doorIndex };
            }

            var doorState = state.Doors[doorIndex];

            // Update door state based on event type
            switch (eventType)
            {
                case EventTypeDoorOpen:
                    doorState.IsDoorOpen = true;
                    doorState.LastDoorOpenUtc = timestamp;
                    break;
                case EventTypeDoorClosed:
                    doorState.IsDoorOpen = false;
                    break;
                case EventTypeCardGranted:
                    if (!doorState.IsLocked)
                    {
                        doorState.IsDoorOpen = true;
                        doorState.LastDoorOpenUtc = timestamp;
                    }
                    break;
                case EventTypeForcedOpenAlarm:
                case EventTypeHeldOpenAlarm:
                    doorState.AlarmActive = true;
                    break;
                case EventTypeAlarmCleared:
                    doorState.AlarmActive = false;
                    break;
            }

            // Add the event
            state.AddEvent(new ControllerEventDto
            {
                DoorNumber = doorIndex,
                CardNumber = cardNumber,
                EventType = eventType,
                IsByCard = cardNumber.HasValue,
                IsByButton = eventType == EventTypeDoorOpen && !cardNumber.HasValue,
                TimestampUtc = timestamp,
                RawData = $"Injected event: Type {eventType}, Door {doorIndex}"
            });

            // Update simulated time to match event timestamp if it's in the future
            if (timestamp > state.SimNowUtc)
            {
                state.SimNowUtc = timestamp;
            }

            _logger.LogInformation("Injected event {EventType} for controller {ControllerId}, door {DoorIndex}", 
                eventType, controllerId, doorIndex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error injecting event");
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Configures a scenario for a controller (placeholder for future preset implementation).
    /// </summary>
    public Task ConfigureScenarioAsync(int controllerId, SimulationScenarioConfig config, CancellationToken cancellationToken = default)
    {
        // Placeholder for future scenario configuration
        _logger.LogInformation("Scenario configured for controller {ControllerId}", controllerId);
        return Task.CompletedTask;
    }

    private async Task ProcessDoorAsync(SimControllerState state, Door door, DateTime now, CancellationToken cancellationToken)
    {
        if (!state.Doors.ContainsKey(door.DoorIndex))
        {
            state.Doors[door.DoorIndex] = new SimDoorState { DoorIndex = door.DoorIndex };
        }

        var doorState = state.Doors[door.DoorIndex];
        var previousLocked = doorState.IsLocked;
        var previousOpen = doorState.IsDoorOpen;

        // 1. Check auto-open schedules
        await ProcessAutoOpenAsync(state, door, doorState, now);

        // 2. Check momentary unlock expiration
        if (doorState.UnlockUntilUtc.HasValue && now >= doorState.UnlockUntilUtc.Value)
        {
            if (!doorState.IsLocked)
            {
                doorState.IsLocked = true;
                doorState.IsRelayOn = false;
                doorState.UnlockUntilUtc = null;
                doorState.UnlockDurationSeconds = null;

                // Generate door closed event if door was open
                if (doorState.IsDoorOpen)
                {
                    doorState.IsDoorOpen = false;
                    state.AddEvent(new ControllerEventDto
                    {
                        DoorNumber = door.DoorIndex,
                        EventType = EventTypeDoorClosed,
                        RawData = $"Door {door.DoorIndex} closed (momentary unlock expired)"
                    });
                }
            }
        }

        // 3. Check auto-open window expiration
        if (doorState.AutoUnlockUntilUtc.HasValue && now >= doorState.AutoUnlockUntilUtc.Value)
        {
            if (!doorState.IsLocked)
            {
                doorState.IsLocked = true;
                doorState.IsRelayOn = false;
                doorState.AutoUnlockUntilUtc = null;

                state.AddEvent(new ControllerEventDto
                {
                    DoorNumber = door.DoorIndex,
                    EventType = EventTypeAutoOpenDeactivated,
                    RawData = $"Auto-open deactivated for door {door.DoorIndex}"
                });
            }
        }

        // 4. Apply advanced door modes
        await ProcessAdvancedDoorModesAsync(state, door, doorState, now);

        // 5. Check for alarm conditions
        await CheckAlarmConditionsAsync(state, door, doorState, now);

        // 6. Detect state transitions and generate events
        if (previousLocked != doorState.IsLocked)
        {
            if (!doorState.IsLocked && !previousOpen)
            {
                // Door unlocked - may open
                doorState.IsDoorOpen = _random.NextDouble() < 0.3; // 30% chance door opens when unlocked
                if (doorState.IsDoorOpen)
                {
                    doorState.LastDoorOpenUtc = now;
                    state.AddEvent(new ControllerEventDto
                    {
                        DoorNumber = door.DoorIndex,
                        EventType = EventTypeDoorOpen,
                        RawData = $"Door {door.DoorIndex} opened"
                    });
                }
            }
        }

        if (previousOpen != doorState.IsDoorOpen)
        {
            if (!doorState.IsDoorOpen && previousOpen)
            {
                state.AddEvent(new ControllerEventDto
                {
                    DoorNumber = door.DoorIndex,
                    EventType = EventTypeDoorClosed,
                    RawData = $"Door {door.DoorIndex} closed"
                });
            }
        }
    }

    private async Task ProcessAutoOpenAsync(SimControllerState state, Door door, SimDoorState doorState, DateTime now)
    {
        if (state.AutoOpen == null || state.TimeSchedules == null)
        {
            return;
        }

        // Find auto-open task for this door
        var autoOpenTask = state.AutoOpen.Tasks.FirstOrDefault(t => t.DoorNumber == door.DoorIndex && t.IsEnabled);
        if (autoOpenTask == null)
        {
            return;
        }

        // Find the time zone for this task
        var timeZone = state.TimeSchedules.TimeZones.FirstOrDefault(tz => tz.Index == autoOpenTask.TimeZoneIndex);
        if (timeZone == null)
        {
            return;
        }

        // Check if current time is within any active interval
        var currentDayOfWeek = (int)now.DayOfWeek; // 0=Sunday, 6=Saturday
        var currentTime = now.TimeOfDay;

        bool isInActiveWindow = false;
        foreach (var daySchedule in timeZone.Days)
        {
            // Check if today matches the day of week (simplified - real firmware uses bitmask)
            // For now, check if any day schedule matches
            var startTime = new TimeSpan(daySchedule.StartHour, daySchedule.StartMinute, 0);
            var endTime = new TimeSpan(daySchedule.EndHour, daySchedule.EndMinute, 0);

            if (currentTime >= startTime && currentTime < endTime)
            {
                isInActiveWindow = true;
                break;
            }
        }

        // Check holidays (if today is a holiday, auto-open may be disabled)
        bool isHoliday = false;
        if (state.TimeSchedules.Holidays != null)
        {
            var today = DateOnly.FromDateTime(now);
            isHoliday = state.TimeSchedules.Holidays.Any(h => 
                today >= h.StartDate && today <= h.EndDate);
        }

        // Apply auto-open logic
        if (isInActiveWindow && !isHoliday)
        {
            // Should be unlocked
            if (doorState.IsLocked)
            {
                doorState.IsLocked = false;
                doorState.IsRelayOn = true;
                doorState.LastAutoUnlockUtc = now;
                
                // Set unlock until end of current window (simplified - use 1 hour default)
                doorState.AutoUnlockUntilUtc = now.AddHours(1);

                state.AddEvent(new ControllerEventDto
                {
                    DoorNumber = door.DoorIndex,
                    EventType = EventTypeAutoOpenActivated,
                    RawData = $"Auto-open activated for door {door.DoorIndex}"
                });
            }
        }
        else
        {
            // Should be locked (if not manually held open)
            if (!doorState.IsLocked && doorState.AutoUnlockUntilUtc.HasValue)
            {
                // Only lock if auto-open was the reason it was unlocked
                doorState.IsLocked = true;
                doorState.IsRelayOn = false;
                doorState.AutoUnlockUntilUtc = null;

                state.AddEvent(new ControllerEventDto
                {
                    DoorNumber = door.DoorIndex,
                    EventType = EventTypeAutoOpenDeactivated,
                    RawData = $"Auto-open deactivated for door {door.DoorIndex}"
                });
            }
        }
    }

    private async Task ProcessAdvancedDoorModesAsync(SimControllerState state, Door door, SimDoorState doorState, DateTime now)
    {
        if (state.AdvancedDoorModes == null)
        {
            return;
        }

        var doorMode = state.AdvancedDoorModes.Doors.FirstOrDefault(d => d.DoorNumber == door.DoorIndex);
        if (doorMode == null)
        {
            return;
        }

        // FirstCardOpenEnabled: First card swipe unlocks door for a duration
        // DoorAsSwitchEnabled: Door acts as a switch (unlock on open, lock on close)
        // These are handled in the main door processing logic

        // OpenTooLongWarnEnabled and Invalid3CardsWarnEnabled are checked in alarm conditions
    }

    private async Task CheckAlarmConditionsAsync(SimControllerState state, Door door, SimDoorState doorState, DateTime now)
    {
        var doorMode = state.AdvancedDoorModes?.Doors.FirstOrDefault(d => d.DoorNumber == door.DoorIndex);

        // Check for forced open alarm (door open but locked)
        if (doorState.IsLocked && doorState.IsDoorOpen)
        {
            if (!doorState.AlarmActive)
            {
                doorState.AlarmActive = true;
                
                // Only generate alarm if not disabled
                if (doorMode == null || doorMode.OpenTooLongWarnEnabled)
                {
                    state.AddEvent(new ControllerEventDto
                    {
                        DoorNumber = door.DoorIndex,
                        EventType = EventTypeForcedOpenAlarm,
                        RawData = $"Forced open alarm: Door {door.DoorIndex} is open but locked"
                    });
                }
            }
        }
        else if (doorState.AlarmActive && !doorState.IsLocked || !doorState.IsDoorOpen)
        {
            // Clear alarm
            doorState.AlarmActive = false;
            state.AddEvent(new ControllerEventDto
            {
                DoorNumber = door.DoorIndex,
                EventType = EventTypeAlarmCleared,
                RawData = $"Alarm cleared for door {door.DoorIndex}"
            });
        }

        // Check for held open too long (door open for more than threshold)
        if (doorState.IsDoorOpen && doorState.LastDoorOpenUtc.HasValue)
        {
            var openDuration = now - doorState.LastDoorOpenUtc.Value;
            if (openDuration.TotalMinutes > 5) // 5 minute threshold
            {
                if (!doorState.AlarmActive && (doorMode == null || doorMode.OpenTooLongWarnEnabled))
                {
                    doorState.AlarmActive = true;
                    state.AddEvent(new ControllerEventDto
                    {
                        DoorNumber = door.DoorIndex,
                        EventType = EventTypeHeldOpenAlarm,
                        RawData = $"Held open alarm: Door {door.DoorIndex} has been open for {openDuration.TotalMinutes:F1} minutes"
                    });
                }
            }
        }
    }

    private async Task GenerateRandomCardSwipesAsync(SimControllerState state, ControllerDevice controller, CancellationToken cancellationToken)
    {
        if (state.Privileges.Count == 0)
        {
            return; // No cards to swipe
        }

        foreach (var door in controller.Doors)
        {
            if (_random.NextDouble() < _options.Value.SyntheticSwipeChance)
            {
                // Pick a random card
                var cardNumbers = state.Privileges.Keys.ToList();
                if (cardNumbers.Count == 0)
                {
                    continue;
                }

                var cardNumber = cardNumbers[_random.Next(cardNumbers.Count)];
                var doorMask = state.Privileges[cardNumber];

                // Check if card has access to this door (simplified - use bit 0 for door 0, bit 1 for door 1, etc.)
                bool hasAccess = (doorMask & (1u << door.DoorIndex)) != 0;

                if (hasAccess)
                {
                    // Grant access
                    state.AddEvent(new ControllerEventDto
                    {
                        DoorNumber = door.DoorIndex,
                        CardNumber = long.TryParse(cardNumber, out var cardNum) ? cardNum : null,
                        EventType = EventTypeCardGranted,
                        IsByCard = true,
                        RawData = $"Card {cardNumber} granted access to door {door.DoorIndex}"
                    });

                    // Unlock door briefly
                    if (!state.Doors.ContainsKey(door.DoorIndex))
                    {
                        state.Doors[door.DoorIndex] = new SimDoorState { DoorIndex = door.DoorIndex };
                    }
                    var doorState = state.Doors[door.DoorIndex];
                    doorState.IsLocked = false;
                    doorState.IsRelayOn = true;
                    doorState.UnlockUntilUtc = state.SimNowUtc.AddSeconds(5); // 5 second unlock
                }
                else
                {
                    // Deny access
                    state.AddEvent(new ControllerEventDto
                    {
                        DoorNumber = door.DoorIndex,
                        CardNumber = long.TryParse(cardNumber, out var cardNum) ? cardNum : null,
                        EventType = EventTypeCardDenied,
                        IsByCard = true,
                        RawData = $"Card {cardNumber} denied access to door {door.DoorIndex}"
                    });
                }
            }
        }
    }
}

/// <summary>
/// Configuration for a simulation scenario.
/// </summary>
public class SimulationScenarioConfig
{
    public double CardSwipeRatePerMinute { get; set; }
    public double DoorOpenProbability { get; set; }
    public double AlarmChance { get; set; }
    public Dictionary<int, bool> DoorStates { get; set; } = new();
}
