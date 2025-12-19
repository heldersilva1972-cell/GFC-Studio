using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GFC.BlazorServer.Models;
using GFC.BlazorServer.Services.Controllers;
using GFC.BlazorServer.Services.Models;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services.Simulation;

/// <summary>
/// Scoped simulation engine that centralizes all controller state mutations for simulation mode.
/// </summary>
internal sealed class SimulationStateEngine : ISimulationStateEngine
{
    private const int EventTypeDoorOpen = 1;
    private const int EventTypePrivilegeGranted = 1001;
    private const int EventTypePrivilegeRevoked = 1002;
    private const int EventTypePrivilegeDeleted = 1003;
    private const int EventTypePrivilegesCleared = 1004;
    private const int EventTypeBulkPrivileges = 1005;
    private const int EventTypeTimeSync = 2001;
    private const int EventTypeConfigChange = 3001;

    private readonly SimControllerStateStore _stateStore;
    private readonly ILogger<SimulationStateEngine> _logger;

    public SimulationStateEngine(SimControllerStateStore stateStore, ILogger<SimulationStateEngine> logger)
    {
        _stateStore = stateStore ?? throw new ArgumentNullException(nameof(stateStore));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<AgentRunStatusDto?> GetRunStatusAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var state = _stateStore.Get(controllerSn);
        if (state == null)
        {
            _logger.LogWarning("Simulation engine: controller {Sn} not found for run status.", controllerSn);
            return Task.FromResult<AgentRunStatusDto?>(null);
        }

        AgentRunStatusDto result;
        lock (state)
        {
            var doors = state.Doors.Values
                .OrderBy(d => d.DoorIndex)
                .Select(d => new AgentDoorStatusDto
                {
                    DoorNumber = d.DoorIndex,
                    IsDoorOpen = d.IsDoorOpen,
                    IsRelayOn = d.IsRelayOn,
                    IsSensorActive = d.IsSensorActive
                })
                .ToList();

            result = new AgentRunStatusDto
            {
                Doors = doors,
                RelayStates = state.RelayStates.ToList(),
                IsFireAlarmActive = state.IsFireAlarmActive,
                IsTamperActive = state.IsTamperActive
            };
        }

        return Task.FromResult<AgentRunStatusDto?>(result);
    }

    public Task SyncTimeAsync(string controllerSn, DateTime serverTimeUtc, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var state = _stateStore.GetOrCreate(controllerSn);

        lock (state)
        {
            state.SimNowUtc = serverTimeUtc;
            state.LastCommUtc = DateTime.UtcNow;
            state.IsOnline = true;
            state.AddEvent(new ControllerEventDto
            {
                EventType = EventTypeTimeSync,
                RawData = $"Controller clock synchronized to {serverTimeUtc:O}"
            });
        }

        _logger.LogInformation("Simulation engine: Synced controller {Sn} clock to {Time}.", controllerSn, serverTimeUtc);
        return Task.CompletedTask;
    }

    public Task OpenDoorAsync(string controllerSn, int doorNo, int? durationSec = null, string? reason = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var state = _stateStore.GetOrCreate(controllerSn);

        lock (state)
        {
            var now = TouchClock(state);
            var door = EnsureDoorState(state, doorNo);

            door.IsLocked = false;
            door.IsRelayOn = true;
            door.IsDoorOpen = true;
            door.LastOpenCommandUtc = now;
            door.LastDoorOpenUtc = now;
            door.LastOpenCommandSource = reason ?? "Manual simulation command";

            if (durationSec.HasValue && durationSec.Value > 0)
            {
                door.UnlockDurationSeconds = durationSec;
                door.UnlockUntilUtc = now.AddSeconds(durationSec.Value);
            }
            else
            {
                door.UnlockDurationSeconds = null;
                door.UnlockUntilUtc = null;
            }

            state.AddEvent(new ControllerEventDto
            {
                DoorNumber = doorNo,
                EventType = EventTypeDoorOpen,
                RawData = $"Door {doorNo} opened ({door.LastOpenCommandSource})",
                IsByButton = false,
                IsByCard = false
            });
        }

        _logger.LogInformation("Simulation engine: Opened door {Door} on controller {Sn}.", doorNo, controllerSn);
        return Task.CompletedTask;
    }

    public Task<ApiResult> AddOrUpdateCardAsync(string controllerSn, AddOrUpdateCardRequestDto request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.CardNumber))
        {
            return Task.FromResult(new ApiResult { Success = false, Message = "Card number is required." });
        }

        var doorBit = GetDoorBit(request.DoorIndex);
        if (doorBit == 0)
        {
            return Task.FromResult(new ApiResult
            {
                Success = false,
                Message = $"Invalid door index {request.DoorIndex}."
            });
        }

        var state = _stateStore.GetOrCreate(controllerSn);
        bool enabled = request.Enabled;
        uint updatedMask;

        lock (state)
        {
            TouchClock(state);

            state.Privileges.TryGetValue(request.CardNumber, out var mask);
            if (enabled)
            {
                mask |= doorBit;
            }
            else
            {
                mask &= ~doorBit;
            }

            if (mask == 0)
            {
                state.Privileges.Remove(request.CardNumber);
            }
            else
            {
                state.Privileges[request.CardNumber] = mask;
            }

            updatedMask = mask;

            state.AddEvent(new ControllerEventDto
            {
                DoorNumber = request.DoorIndex,
                CardNumber = TryParseCard(request.CardNumber),
                EventType = enabled ? EventTypePrivilegeGranted : EventTypePrivilegeRevoked,
                RawData = $"Card {request.CardNumber} {(enabled ? "granted" : "revoked")} for door {request.DoorIndex}"
            });
        }

        _logger.LogInformation("Simulation engine: Card {Card} {Action} for door {Door} on controller {Sn}.",
            request.CardNumber,
            enabled ? "enabled" : "disabled",
            request.DoorIndex,
            controllerSn);

        return Task.FromResult(new ApiResult
        {
            Success = true,
            Message = enabled
                ? $"Card {request.CardNumber} updated with mask {updatedMask}."
                : $"Card {request.CardNumber} updated."
        });
    }

    public Task<ApiResult> DeleteCardAsync(string controllerSn, string cardNumber, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var state = _stateStore.GetOrCreate(controllerSn);
        bool removed;
        lock (state)
        {
            TouchClock(state);
            removed = state.Privileges.Remove(cardNumber);
            if (removed)
            {
                state.AddEvent(new ControllerEventDto
                {
                    CardNumber = TryParseCard(cardNumber),
                    EventType = EventTypePrivilegeDeleted,
                    RawData = $"Card {cardNumber} removed from controller"
                });
            }
        }

        _logger.LogInformation("Simulation engine: Card {Card} removal from controller {Sn}. Removed = {Removed}.",
            cardNumber, controllerSn, removed);

        return Task.FromResult(new ApiResult
        {
            Success = true,
            Message = removed ? "Card removed from simulation state." : "Card did not exist."
        });
    }

    public Task<ApiResult> BulkUploadCardsAsync(string controllerSn, IEnumerable<AddOrUpdateCardRequestDto> privileges, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var privilegeList = privileges?.ToList() ?? new List<AddOrUpdateCardRequestDto>();
        var state = _stateStore.GetOrCreate(controllerSn);

        lock (state)
        {
            TouchClock(state);
            state.Privileges.Clear();

            foreach (var entry in privilegeList)
            {
                if (entry == null || string.IsNullOrWhiteSpace(entry.CardNumber) || !entry.Enabled)
                {
                    continue;
                }

                var bit = GetDoorBit(entry.DoorIndex);
                if (bit == 0)
                {
                    continue;
                }

                state.Privileges.TryGetValue(entry.CardNumber, out var mask);
                mask |= bit;
                state.Privileges[entry.CardNumber] = mask;
            }

            state.AddEvent(new ControllerEventDto
            {
                EventType = EventTypeBulkPrivileges,
                RawData = $"Bulk privilege upload applied ({privilegeList.Count} entries)"
            });
        }

        _logger.LogInformation("Simulation engine: Applied bulk privilege upload with {Count} entries to controller {Sn}.",
            privilegeList.Count, controllerSn);

        return Task.FromResult(new ApiResult
        {
            Success = true,
            Message = $"Bulk privilege upload applied ({privilegeList.Count} entries)."
        });
    }

    public Task<ApiResult> ClearAllCardsAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var state = _stateStore.GetOrCreate(controllerSn);
        int clearedCount;

        lock (state)
        {
            TouchClock(state);
            clearedCount = state.Privileges.Count;
            state.Privileges.Clear();
            state.AddEvent(new ControllerEventDto
            {
                EventType = EventTypePrivilegesCleared,
                RawData = $"All privileges cleared ({clearedCount} card(s))"
            });
        }

        _logger.LogInformation("Simulation engine: Cleared {Count} cards from controller {Sn}.", clearedCount, controllerSn);

        return Task.FromResult(new ApiResult
        {
            Success = true,
            Message = $"Cleared {clearedCount} simulated card(s)."
        });
    }

    public Task<ControllerEventsResultDto> GetEventsAsync(string controllerSn, uint startIndex, int maxCount, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var state = _stateStore.Get(controllerSn);
        if (state == null)
        {
            return Task.FromResult(new ControllerEventsResultDto { LastIndex = startIndex });
        }

        List<ControllerEventDto> events;
        uint lastIndex;

        lock (state)
        {
            var limit = maxCount <= 0 ? int.MaxValue : maxCount;
            events = state.GetNewEvents(startIndex)
                .Take(limit)
                .ToList();
            lastIndex = state.LastEventIndex;
        }

        return Task.FromResult(new ControllerEventsResultDto
        {
            Events = events,
            LastIndex = lastIndex
        });
    }

    public uint GetCurrentEventIndex(string controllerSn)
    {
        var state = _stateStore.Get(controllerSn);
        return state?.LastEventIndex ?? 0;
    }

    public Task<TimeScheduleDto?> GetTimeSchedulesAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var state = _stateStore.Get(controllerSn);
        return Task.FromResult(state?.TimeSchedules);
    }

    public Task WriteTimeSchedulesAsync(string controllerSn, TimeScheduleWriteDto dto, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var state = _stateStore.GetOrCreate(controllerSn);

        lock (state)
        {
            TouchClock(state);
            state.TimeSchedules = new TimeScheduleDto
            {
                TimeZones = dto.TimeZones.Select(tz => new TimeScheduleDto.TimeZoneBlock(tz.Index, tz.Days)).ToList(),
                Holidays = dto.Holidays.Select(h => new TimeScheduleDto.HolidayBlock(h.Index, h.StartDate, h.EndDate)).ToList(),
                Tasks = dto.Tasks.Select(t => new TimeScheduleDto.TaskBlock(t.Index, t.Action, t.Door, t.ScheduleId)).ToList()
            };
            state.LastConfigChangeUtc = DateTime.UtcNow;
            RecordConfigEvent(state, "Time schedules updated.");
        }

        _logger.LogInformation("Simulation engine: Updated time schedules for controller {Sn}.", controllerSn);
        return Task.CompletedTask;
    }

    public Task<ExtendedConfigDto?> GetDoorConfigAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var state = _stateStore.Get(controllerSn);
        return Task.FromResult(state?.DoorConfig);
    }

    public Task WriteDoorConfigAsync(string controllerSn, ExtendedDoorConfigWriteDto dto, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var state = _stateStore.GetOrCreate(controllerSn);

        lock (state)
        {
            TouchClock(state);
            state.DoorConfig = new ExtendedConfigDto
            {
                FirmwareMajor = 1,
                FirmwareMinor = 0,
                BoardType = 0,
                AntiPassbackEnabled = false,
                InterlockEnabled = false,
                Doors = dto.Doors.Select(d => new ExtendedConfigDto.DoorExtendedConfig
                {
                    DoorNumber = d.DoorNumber,
                    LockDelaySeconds = d.LockDelaySeconds,
                    NormallyOpenMode = d.NormallyOpenMode,
                    DoubleLock = d.DoubleLock,
                    AllowButtonOpen = d.AllowButtonOpen
                }).ToList()
            };
            state.LastConfigChangeUtc = DateTime.UtcNow;
            RecordConfigEvent(state, "Door configuration updated.");
        }

        _logger.LogInformation("Simulation engine: Updated door config for controller {Sn}.", controllerSn);
        return Task.CompletedTask;
    }

    public Task<AutoOpenConfigDto?> GetAutoOpenAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var state = _stateStore.Get(controllerSn);
        return Task.FromResult(state?.AutoOpen);
    }

    public Task WriteAutoOpenAsync(string controllerSn, AutoOpenConfigDto dto, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var state = _stateStore.GetOrCreate(controllerSn);

        lock (state)
        {
            TouchClock(state);
            state.AutoOpen = dto;
            state.LastConfigChangeUtc = DateTime.UtcNow;
            RecordConfigEvent(state, "Auto-open configuration updated.");
        }

        _logger.LogInformation("Simulation engine: Updated auto-open config for controller {Sn}.", controllerSn);
        return Task.CompletedTask;
    }

    public Task<AdvancedDoorModesDto?> GetAdvancedDoorModesAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var state = _stateStore.Get(controllerSn);
        return Task.FromResult(state?.AdvancedDoorModes);
    }

    public Task WriteAdvancedDoorModesAsync(string controllerSn, AdvancedDoorModesDto dto, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var state = _stateStore.GetOrCreate(controllerSn);

        lock (state)
        {
            TouchClock(state);
            state.AdvancedDoorModes = dto;
            state.LastConfigChangeUtc = DateTime.UtcNow;
            RecordConfigEvent(state, "Advanced door modes updated.");
        }

        _logger.LogInformation("Simulation engine: Updated advanced door modes for controller {Sn}.", controllerSn);
        return Task.CompletedTask;
    }

    public Task<NetworkConfigDto?> GetNetworkConfigAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var state = _stateStore.Get(controllerSn);
        if (state?.NetworkConfig == null)
        {
            return Task.FromResult<NetworkConfigDto?>(new NetworkConfigDto
            {
                IpAddress = "192.168.1.100",
                Port = 60000,
                SubnetMask = "255.255.255.0",
                Gateway = "192.168.1.1"
            });
        }

        return Task.FromResult(state.NetworkConfig);
    }

    public Task SetNetworkConfigAsync(string controllerSn, NetworkConfigDto dto, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var state = _stateStore.GetOrCreate(controllerSn);

        lock (state)
        {
            TouchClock(state);
            if (state.NetworkConfig == null)
            {
                state.NetworkConfig = new NetworkConfigDto();
            }

            state.NetworkConfig.IpAddress = dto.IpAddress;
            state.NetworkConfig.Port = dto.Port;
            state.NetworkConfig.SubnetMask = dto.SubnetMask;
            state.NetworkConfig.Gateway = dto.Gateway;
            state.NetworkConfig.DhcpEnabled = dto.DhcpEnabled;

            state.LastConfigChangeUtc = DateTime.UtcNow;
            RecordConfigEvent(state, "Network configuration updated.");
        }

        _logger.LogInformation("Simulation engine: Updated network configuration for controller {Sn}.", controllerSn);
        return Task.CompletedTask;
    }

    public Task<AllowedPcAndPasswordRequestDto?> GetAllowedPcSettingsAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var state = _stateStore.Get(controllerSn);
        if (state?.AllowedPcSettings == null)
        {
            return Task.FromResult<AllowedPcAndPasswordRequestDto?>(new AllowedPcAndPasswordRequestDto
            {
                AllowedPcIp = "192.168.1.50",
                CommPassword = null
            });
        }

        return Task.FromResult<AllowedPcAndPasswordRequestDto?>(new AllowedPcAndPasswordRequestDto
        {
            AllowedPcIp = state.AllowedPcSettings.AllowedPcIp,
            CommPassword = state.AllowedPcSettings.CommPassword
        });
    }

    public Task<ApiResult> SetAllowedPcSettingsAsync(string controllerSn, AllowedPcAndPasswordRequestDto dto, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var state = _stateStore.GetOrCreate(controllerSn);

        lock (state)
        {
            TouchClock(state);
            state.AllowedPcSettings = new AllowedPcAndPasswordRequestDto
            {
                AllowedPcIp = dto.AllowedPcIp,
                CommPassword = dto.CommPassword
            };
            state.LastConfigChangeUtc = DateTime.UtcNow;
            RecordConfigEvent(state, "Allowed PC / password updated.");
        }

        _logger.LogInformation("Simulation engine: Updated allowed PC settings for controller {Sn}.", controllerSn);

        return Task.FromResult(new ApiResult
        {
            Success = true,
            Message = "Allowed PC / password updated in simulation state."
        });
    }

    public Task RebootAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var state = _stateStore.Get(controllerSn);
        if (state == null)
        {
            _logger.LogWarning("Simulation engine: Attempted to reboot controller {Sn} that has no state.", controllerSn);
            return Task.CompletedTask;
        }

        lock (state)
        {
            foreach (var door in state.Doors.Values)
            {
                door.IsLocked = true;
                door.IsDoorOpen = false;
                door.AlarmActive = false;
                door.IsRelayOn = false;
                door.IsSensorActive = false;
                door.LastDoorOpenUtc = null;
                door.LastAutoUnlockUtc = null;
                door.AutoUnlockUntilUtc = null;
                door.UnlockUntilUtc = null;
                door.UnlockDurationSeconds = null;
                door.LastOpenCommandUtc = null;
                door.LastOpenCommandSource = null;
            }

            state.IsFireAlarmActive = false;
            state.IsTamperActive = false;
            state.ResetEvents();
            state.LastCommUtc = DateTime.UtcNow;
        }

        _logger.LogInformation("Simulation engine: Rebooted controller {Sn} simulation state.", controllerSn);
        return Task.CompletedTask;
    }

    private static SimDoorState EnsureDoorState(SimControllerState state, int doorNo)
    {
        if (!state.Doors.ContainsKey(doorNo))
        {
            state.Doors[doorNo] = new SimDoorState { DoorIndex = doorNo };
        }

        return state.Doors[doorNo];
    }

    private static uint GetDoorBit(int doorIndex)
    {
        if (doorIndex < 0 || doorIndex >= 32)
        {
            return 0;
        }

        return 1u << doorIndex;
    }

    private static long? TryParseCard(string cardNumber)
    {
        return long.TryParse(cardNumber, out var card) ? card : null;
    }

    private static DateTime TouchClock(SimControllerState state)
    {
        var now = DateTime.UtcNow;
        state.SimNowUtc = now;
        state.LastCommUtc = now;
        state.IsOnline = true;
        return now;
    }

    private static void RecordConfigEvent(SimControllerState state, string description)
    {
        state.AddEvent(new ControllerEventDto
        {
            EventType = EventTypeConfigChange,
            RawData = description
        });
    }
}

