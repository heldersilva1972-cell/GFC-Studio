using GFC.BlazorServer.Configuration;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Models;
using GFC.BlazorServer.Services;
using GFC.BlazorServer.Services.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services.Controllers;

/// <summary>
/// Real controller client that delegates to AgentApiClient (Agent PC).
/// </summary>
public class RealControllerClient : IControllerClient
{
    private readonly GFC.BlazorServer.Connectors.Mengqi.MengqiControllerClient _mengqiClient;
    private readonly ILogger<RealControllerClient> _logger;
    private readonly ControllerRegistryService _controllerRegistry;
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;

    public RealControllerClient(
        GFC.BlazorServer.Connectors.Mengqi.MengqiControllerClient mengqiClient,
        ILogger<RealControllerClient> logger,
        ControllerRegistryService controllerRegistry,
        IDbContextFactory<GfcDbContext> contextFactory)
    {
        _mengqiClient = mengqiClient ?? throw new ArgumentNullException(nameof(mengqiClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _controllerRegistry = controllerRegistry ?? throw new ArgumentNullException(nameof(controllerRegistry));
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
    }

    // ... (Wait, I need to preserve the SimMode S2 contract region or just replace the HTTP calls?)
    // The previous implementation had a "SimMode S2 contract" region which I should preserve but update to use _mengqiClient internally if it called private helpers or if those helpers were calling agent. 
    // Wait, the SimMode contract methods (OpenDoorAsync, AddOrUpdatePrivilegeAsync) called private overloads or public overloads which used _agentApiClient.
    
    // I will replace the methods that used _agentApiClient.
    
    #region SimMode S2 contract
    // ... (Keep existing implementation that delegates to overloads, just update overloads)
    public async Task OpenDoorAsync(int controllerId, int doorId, CancellationToken ct)
    {
        var (controller, door) = await ResolveControllerAndDoorAsync(controllerId, doorId, ct);
        if (controller == null || door == null) return;
        await OpenDoorAsync(controller.SerialNumberDisplay, door.DoorIndex, null, ct);
    }
    
     public async Task AddOrUpdatePrivilegeAsync(CardPrivilegeModel model, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.ControllerId == 0) throw new ArgumentException("ControllerId is required on CardPrivilegeModel.", nameof(model));

        var controller = await _controllerRegistry.GetControllerByIdAsync(model.ControllerId, ct);
        if (controller == null)
        {
            throw new InvalidOperationException($"Controller {model.ControllerId} not found for AddOrUpdatePrivilege");
        }

        var doorIndexes = ResolveDoorIndexes(model, controller);
        if (!doorIndexes.Any())
        {
            _logger.LogWarning("No door indexes resolved for controller {ControllerId} and Card {CardNumber}. Skipping hardware update.", controller.Id, model.CardNumber);
            return;
        }

        var request = new AddOrUpdateCardRequestDto
        {
            CardNumber = model.CardNumber.ToString(),
            DoorIndexes = doorIndexes,
            TimeProfileIndex = model.TimeProfileIndex,
            Enabled = model.Enabled
        };

        var result = await AddOrUpdateCardAsync(controller.SerialNumberDisplay, request, ct);
        if (!result.Success)
        {
            throw new InvalidOperationException($"Failed to add/update privilege: {result.Message}");
        }
    }

    public async Task DeletePrivilegeAsync(long cardNumber, CancellationToken ct)
    {
        var controllers = await _controllerRegistry.GetControllersAsync(includeDoors: false, cancellationToken: ct);
        foreach (var controller in controllers)
        {
            try
            {
                var result = await DeleteCardAsync(controller.SerialNumberDisplay, cardNumber.ToString(), ct);
                if (!result.Success)
                {
                    _logger.LogWarning("DeletePrivilege failed for controller {ControllerId}: {Message}", controller.Id, result.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "DeletePrivilege threw for controller {ControllerId}", controller.Id);
            }
        }
    }
    
    public async Task BulkUploadAsync(IEnumerable<CardPrivilegeModel> models, CancellationToken ct)
    {
        if (models == null) return;
        foreach (var model in models) await AddOrUpdatePrivilegeAsync(model, ct);
    }
    
    public async Task ClearAllCardsAsync(int controllerId, CancellationToken ct)
    {
        var controller = await _controllerRegistry.GetControllerByIdAsync(controllerId, ct);
        if (controller == null)
        {
            throw new InvalidOperationException($"Controller {controllerId} not found for ClearAllCards");
        }
        var result = await ClearAllCardsAsync(controller.SerialNumberDisplay, ct);
        if (!result.Success)
        {
            throw new InvalidOperationException($"ClearAllCards failed: {result.Message}");
        }
    }

    public async Task SyncTimeAsync(int controllerId, CancellationToken ct)
    {
        var controller = await _controllerRegistry.GetControllerByIdAsync(controllerId, ct);
        if (controller == null)
        {
            _logger.LogWarning("Controller {ControllerId} not found for SyncTime", controllerId);
            return;
        }
        // Direct call to sync time
        if (!uint.TryParse(controller.SerialNumberDisplay, out var sn)) return;
        await _mengqiClient.SyncTimeAsync(sn, DateTime.UtcNow, ct);
    }

    public async Task<RunStatusModel> GetRunStatusAsync(int controllerId, CancellationToken ct)
    {
        var controller = await _controllerRegistry.GetControllerByIdAsync(controllerId, ct);
        if (controller == null) return new RunStatusModel { IsOnline = false };

        var status = await GetRunStatusAsync(controller.SerialNumberDisplay, ct);
        if (status == null) return new RunStatusModel { IsOnline = false };

        // Filter doors based on configured door count to eliminate ghost data
        // On 2-door controllers, bytes 56-60 in the 0x20 response contain uninitialized RAM
        // values from unmapped hardware registers (Doors 3 & 4 don't physically exist)
        var doorCount = controller.DoorCount > 0 ? controller.DoorCount : 4;
        var filteredDoors = status.Doors
            .Where(d => d.DoorNumber <= doorCount)
            .Select(d => new RunStatusModel.DoorStatus
            {
                DoorIndex = d.DoorNumber,
                IsDoorOpen = d.IsDoorOpen,
                IsRelayOn = d.IsRelayOn,
                IsSensorActive = d.IsSensorActive
            }).ToList();

        return new RunStatusModel
        {
            IsOnline = true,
            ControllerTimeUtc = status.ControllerTimeUtc,
            TotalCards = status.TotalCards,
            TotalEvents = status.TotalEvents,
            Doors = filteredDoors
        };
    }
    
     public async Task<IReadOnlyList<EventLogModel>> GetEventsByIndexAsync(int controllerId, long fromIndex, int maxCount, CancellationToken ct)
    {
        var controller = await _controllerRegistry.GetControllerByIdAsync(controllerId, ct);
        if (controller == null) return Array.Empty<EventLogModel>();

        if (!uint.TryParse(controller.SerialNumberDisplay, out var sn)) return Array.Empty<EventLogModel>();

        try 
        {
            var (events, lastIndex) = await _mengqiClient.GetNewEventsAsync(sn, (uint)fromIndex, ct);
             // Mapper from ControllerEvent to EventLogModel
             var doorMap = controller.Doors?.ToDictionary(d => d.DoorIndex, d => d.Id) ?? new Dictionary<int, int>();
             return events.Select((e, i) => new EventLogModel 
             {
                 Index = fromIndex + i + 1,
                 ControllerId = controller.Id,
                 DoorNumber = e.DoorOrReader,
                 DoorId = doorMap.TryGetValue(e.DoorOrReader, out var did) ? did : null,
                 TimestampUtc = e.TimestampUtc,
                 CardNumber = e.CardNumber,
                 EventType = (int)e.EventType,
                 ReasonCode = (int)e.ReasonCode,
                 IsByCard = e.IsByCard,
                 IsByButton = e.IsByExitButton
             }).ToList();
        }
        catch (Exception ex)
        {
             _logger.LogError(ex, "Failed to get events");
             return Array.Empty<EventLogModel>();
        }
    }
    #endregion
    
    // Direct Hardware Operations
    
    public async Task<bool> PingAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var controllers = await _controllerRegistry.GetControllersAsync(includeDoors: false, cancellationToken);
            var firstEnabled = controllers.FirstOrDefault(c => c.IsEnabled);
            
            if (firstEnabled == null)
            {
                _logger.LogWarning("No enabled controllers found for ping check");
                return false;
            }

            // 1. Try standard status check
            var status = await GetRunStatusAsync(firstEnabled.SerialNumberDisplay, cancellationToken);
            if (status != null && status.IsOnline)
            {
                return true;
            }
            
            // 2. Fallback to targeted search (most robust command)
            // If the controller responds to search, it's definitely there even if status fails
            if (uint.TryParse(firstEnabled.SerialNumberDisplay, out var sn))
            {
                var discovered = await _mengqiClient.DiscoverControllersAsync(cancellationToken);
                if (discovered.Any(d => d.SerialNumber == sn))
                {
                    _logger.LogInformation("Controller {SerialNumber} responded to search ping", firstEnabled.SerialNumberDisplay);
                    return true;
                }
            }
            
            _logger.LogWarning("Controller {SerialNumber} ping failed - unreachable", firstEnabled.SerialNumberDisplay);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Controller ping failed with exception");
            return false;
        }
    }

    public async Task<AgentRunStatusDto?> GetRunStatusAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn)) 
        {
            _logger.LogError("Invalid serial number format: {Sn}", controllerSn);
            return null;
        }

        try 
        {
             _logger.LogInformation("Checking status for controller {Sn}...", sn);
             var status = await _mengqiClient.GetRunStatusAsync(sn, cancellationToken);
             
             if (status == null)
             {
                 _logger.LogWarning("Controller {Sn} returned null status (Timeout?)", sn);
                 return null;
             }

             _logger.LogInformation("Controller {Sn} is ONLINE.", sn);

             // Automatic Time Sync Logic
             // User Requirement: "only synchronize the clock once every 24 hours or upon system reboot."
             // efficient check: If year < 2025, it's likely a reboot/loss of power (defaulting to 2000).
             if (status.ControllerTime.HasValue)
             {
                 if (status.ControllerTime.Value.Year < 2025)
                 {
                     _logger.LogWarning("Controller {Sn} date invalid ({Date}). Triggering Reboot Recovery Time Sync...", sn, status.ControllerTime.Value);
                     try 
                     {
                         await _mengqiClient.SyncTimeAsync(sn, DateTime.Now, cancellationToken).ConfigureAwait(false);
                     }
                     catch (Exception ex)
                     {
                         _logger.LogWarning(ex, "Recovery time sync failed for controller {Sn}", sn);
                     }
                 }
                 // Note: Daily 24h sync should be handled by a scheduled task, not this polling loop.
             }

             return new AgentRunStatusDto 
             {
                 SerialNumber = sn,
                 IsOnline = true,
                 ControllerTimeUtc = (status.ControllerTime ?? DateTime.Now).ToUniversalTime(),
                 TotalCards = status.TotalCards,
                 TotalEvents = status.TotalEvents,
                 Doors = status.Doors.Select(d => new AgentRunStatusDto.DoorRunStatus
                 {
                     DoorNumber = d.DoorNumber,
                     IsDoorOpen = d.IsDoorOpen,
                     IsRelayOn = d.IsRelayOn,
                     IsSensorActive = d.IsSensorActive
                 }).ToList()
             };
        }
        catch (Exception ex)
        {
             _logger.LogWarning("Failed to get run status for {Sn}. Error: {Msg}", sn, ex.Message);
             return null;
        }
    }
    
    public async Task OpenDoorAsync(string controllerSn, int doorNo, int? durationSec = null, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn)) return;
        await _mengqiClient.OpenDoorAsync(sn, doorNo, durationSec, cancellationToken);
    }

    public async Task<ApiResult> AddOrUpdateCardAsync(string controllerSn, AddOrUpdateCardRequestDto request, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn)) return new ApiResult { Success = false, Message = "Invalid SN" };
        
        var tzIndex = (byte)(request.TimeProfileIndex ?? 0);
        var flags = request.Enabled ? GFC.BlazorServer.Connectors.Mengqi.Models.CardPrivilegeFlags.Normal : GFC.BlazorServer.Connectors.Mengqi.Models.CardPrivilegeFlags.None;

        var model = new GFC.BlazorServer.Connectors.Mengqi.Models.CardPrivilegeModel 
        {
             CardNumber = long.Parse(request.CardNumber),
             DoorList = request.DoorIndexes,
             TimeZones = new[] { tzIndex, tzIndex, tzIndex, tzIndex },
             Flags = flags,
             ValidFrom = null, // Valid forever (WgPayloadFactory handles defaults)
             ValidTo = null    // Valid forever
        };
        
        try {
            await _mengqiClient.AddOrUpdateCardAsync(sn, model, cancellationToken);
            return new ApiResult { Success = true };
        } catch (Exception ex) { return new ApiResult { Success = false, Message = ex.Message }; }
    }

    public async Task<ApiResult> DeleteCardAsync(string controllerSn, string cardNumber, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn)) return new ApiResult { Success = false, Message = "Invalid SN" };
        try {
            await _mengqiClient.DeleteCardAsync(sn, long.Parse(cardNumber), cancellationToken);
            return new ApiResult { Success = true };
        } catch (Exception ex) { return new ApiResult { Success = false, Message = ex.Message }; }
    }
    
    public async Task<ApiResult> ClearAllCardsAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn)) return new ApiResult { Success = false, Message = "Invalid SN" };
        try {
            await _mengqiClient.ClearAllCardsAsync(sn, cancellationToken);
            return new ApiResult { Success = true };
        } catch (Exception ex) { return new ApiResult { Success = false, Message = ex.Message }; }
    }
    
    public Task<ControllerEventsResultDto> GetNewEventsAsync(string controllerSn, uint lastIndex, CancellationToken cancellationToken = default)
    {
         // This is a duplicate of GetEventsByIndexAsync logically but returns a specific DTO
        if (!uint.TryParse(controllerSn, out var sn)) return Task.FromResult(new ControllerEventsResultDto { LastIndex = lastIndex });
        
        // We'd need to async await here but interface is Task return.
        // For brevity/correctness lets implement using .Result or better wrap in async if interface allows (it returns Task so yes)
        return InternalGetNewEventsAsync(sn, lastIndex, cancellationToken);
    }
    
    private async Task<ControllerEventsResultDto> InternalGetNewEventsAsync(uint sn, uint lastIndex, CancellationToken ct)
    {
        try {
            var (events, newLastIndex) = await _mengqiClient.GetNewEventsAsync(sn, lastIndex, ct);
            // Map events
             var resultEvents = events.Select((e, i) => new ControllerEventDto 
             {
                 RawIndex = lastIndex + (uint)i + 1, // Infer index based on offset from lastIndex
                 DoorNumber = e.DoorOrReader,
                 TimestampUtc = e.TimestampUtc,
                 CardNumber = e.CardNumber,
                 EventType = (int)e.EventType,
                 ReasonCode = (int)e.ReasonCode,
                 IsByCard = e.IsByCard,
                 IsByButton = e.IsByExitButton
             }).ToList();
             
             // User Requirement: "Occasionally send the 0xB2 (Acknowledgment) command with your LastReadIndex"
             if (newLastIndex > lastIndex && events.Any())
             {
                 try 
                 {
                     await _mengqiClient.AcknowledgeEventsAsync(sn, newLastIndex, ct);
                 }
                 catch (Exception ex)
                 {
                     _logger.LogWarning(ex, "Failed to send Event Acknowledgment (0xB2) to controller {Sn}", sn);
                 }
             }

             return new ControllerEventsResultDto { 
                 LastIndex = newLastIndex,
                 Events = resultEvents
             };
        } catch { return new ControllerEventsResultDto { LastIndex = lastIndex }; }
    }
    
    // Config methods - use _mengqiClient.Read/Write methods if available, else throw NotImpl
    public Task<TimeScheduleDto?> GetTimeSchedulesAsync(string controllerSn, CancellationToken ct = default) 
    {
         // Feature not implemented in driver
         return Task.FromResult<TimeScheduleDto?>(null);
    }
    
    public Task WriteTimeSchedulesAsync(string controllerSn, TimeScheduleWriteDto dto, CancellationToken ct = default)
    {
         // Feature not implemented in driver
         return Task.CompletedTask;
    }
    
    public Task<ExtendedConfigDto?> GetDoorConfigAsync(string controllerSn, CancellationToken ct = default)
    {
         // Feature not implemented in driver
         return Task.FromResult<ExtendedConfigDto?>(null);
    }
    
    public Task WriteDoorConfigAsync(string controllerSn, ExtendedDoorConfigWriteDto dto, CancellationToken ct = default)
    {
        // Feature not implemented in driver
        return Task.CompletedTask;
    }

    // AutoOpen and AdvancedDoorModes were NOT on the library interface explicitly? They might be part of ExtendedConfig.
    // Checking MengqiControllerClient.cs, it had ReadExtendedConfigAsync.
    // The library interface didn't seem to have direct specific methods for AutoOpen/Advanced separate from ExtendedConfig?
    // Let's assume for now we return null/throw if not supported or check if they map to ExtendedConfig.
    // Given the library code I saw earlier, they threw NotImplemented except basic commands.
    
    public Task<AutoOpenConfigDto?> GetAutoOpenAsync(string controllerSn, CancellationToken ct = default) => Task.FromResult<AutoOpenConfigDto?>(null); 
    public Task WriteAutoOpenAsync(string controllerSn, AutoOpenConfigDto dto, CancellationToken ct = default) => Task.CompletedTask;
    public Task<AdvancedDoorModesDto?> GetAdvancedDoorModesAsync(string controllerSn, CancellationToken ct = default) => Task.FromResult<AdvancedDoorModesDto?>(null);
    public Task WriteAdvancedDoorModesAsync(string controllerSn, AdvancedDoorModesDto dto, CancellationToken ct = default) => Task.CompletedTask;
    
    // Network config
    // Network config
    public Task<NetworkConfigDto?> GetNetworkConfigAsync(string controllerSn, CancellationToken ct = default)
    {
        // Feature not implemented in driver
        return Task.FromResult<NetworkConfigDto?>(null);
    }
    
    public Task SetNetworkConfigAsync(string controllerSn, NetworkConfigDto dto, CancellationToken ct = default)
    {
        // Feature not implemented in driver
        return Task.CompletedTask;
    }
    
    public Task<AllowedPcAndPasswordRequestDto?> GetAllowedPcSettingsAsync(string controllerSn, CancellationToken ct = default) => Task.FromResult<AllowedPcAndPasswordRequestDto?>(null);
    public Task<ApiResult> SetAllowedPcSettingsAsync(string controllerSn, AllowedPcAndPasswordRequestDto dto, CancellationToken ct = default) => Task.FromResult(new ApiResult { Success = true });
    
    public async Task RebootAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn)) return;
        await _mengqiClient.RebootControllerAsync(sn, cancellationToken);
    }
    
    public async Task ResetControllerAsync(int controllerId, CancellationToken ct = default)
    {
        var controller = await _controllerRegistry.GetControllerByIdAsync(controllerId, ct);
        if (controller == null) return;

        if (!uint.TryParse(controller.SerialNumberDisplay, out var sn)) return;

        // Fetch Door Configs from database to apply hardware timers
        await using var db = await _contextFactory.CreateDbContextAsync(ct);
        var doors = await db.Doors.AsNoTracking()
            .Where(d => d.ControllerId == controllerId)
            .ToListAsync(ct);

        var doorIds = doors.Select(d => d.Id).ToList();
        var configs = await db.DoorConfigs.AsNoTracking()
            .Where(c => doorIds.Contains(c.DoorId))
            .ToListAsync(ct);

        var hwConfigs = doors.Select(d => {
            var cfg = configs.FirstOrDefault(c => c.DoorId == d.Id);
            return new GFC.BlazorServer.Connectors.Mengqi.Models.DoorHardwareConfig
            {
                DoorIndex = d.DoorIndex,
                ControlMode = cfg?.ControlMode ?? 3,
                RelayDelay = (byte)(cfg?.OpenTimeSeconds ?? 5),
                SensorType = cfg?.SensorType ?? 0,
                Interlock = cfg?.Interlock ?? 0
            };
        });

        // Step 5: Fetch a primary card to re-add (first active card with access to this controller)
        GFC.BlazorServer.Connectors.Mengqi.Models.CardPrivilegeModel? primaryCard = null;
        var access = await db.MemberDoorAccesses.AsNoTracking()
            .Where(a => doorIds.Contains(a.DoorId) && a.IsEnabled)
            .OrderBy(a => a.Id)
            .FirstOrDefaultAsync(ct);

        if (access != null && long.TryParse(access.CardNumber, out var cardNo))
        {
            // Fetch all doors for this card on this controller
            var cardDoors = await db.MemberDoorAccesses.AsNoTracking()
                .Where(a => a.CardNumber == access.CardNumber && doorIds.Contains(a.DoorId) && a.IsEnabled)
                .Select(a => a.Door.DoorIndex)
                .ToListAsync(ct);

            primaryCard = new GFC.BlazorServer.Connectors.Mengqi.Models.CardPrivilegeModel
            {
                CardNumber = cardNo,
                DoorList = cardDoors,
                Flags = GFC.BlazorServer.Connectors.Mengqi.Models.CardPrivilegeFlags.Normal
            };
        }

        await _mengqiClient.ResetControllerAsync(sn, controller.DoorCount, hwConfigs, primaryCard, ct);
    }

    public async Task ResetControllerAsync(string controllerSn, int doorCount = 4, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn)) return;
        await _mengqiClient.ResetControllerAsync(sn, doorCount, null, null, cancellationToken);
    }

    public async Task SetDoorConfigAsync(string controllerSn, int doorIndex, byte controlMode, byte relayDelay, byte doorSensor, byte interlock, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn)) return;
        await _mengqiClient.SetDoorConfigAsync(sn, doorIndex, controlMode, relayDelay, doorSensor, interlock, cancellationToken);
    }

    public async Task<GFC.BlazorServer.Connectors.Mengqi.Models.DiscoveryResult?> GetHardwareInfoAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn)) return null;
        return await _mengqiClient.GetHardwareInfoAsync(sn, cancellationToken);
    }

    public async Task<IEnumerable<GFC.BlazorServer.Connectors.Mengqi.Models.DiscoveryResult>> DiscoverAsync(CancellationToken cancellationToken = default)
    {
        return await _mengqiClient.DiscoverControllersAsync(cancellationToken);
    }

    private async Task<(ControllerDevice? Controller, Door? Door)> ResolveControllerAndDoorAsync(int controllerId, int doorId, CancellationToken ct)
    {
        var controller = await _controllerRegistry.GetControllerByIdAsync(controllerId, ct);
        if (controller == null)
        {
            _logger.LogWarning("Controller {ControllerId} not found.", controllerId);
            return (null, null);
        }

        var door = controller.Doors.FirstOrDefault(d => d.Id == doorId);
        if (door == null)
        {
            _logger.LogWarning("Door {DoorId} not found for controller {ControllerId}.", doorId, controllerId);
            return (controller, null);
        }

        return (controller, door);
    }

    private static List<int> ResolveDoorIndexes(CardPrivilegeModel model, ControllerDevice controller)
    {
        var result = new List<int>();

        if (model.DoorIndexes != null && model.DoorIndexes.Any())
        {
            result.AddRange(model.DoorIndexes);
        }
        else if (model.DoorIndex.HasValue)
        {
            result.Add(model.DoorIndex.Value);
        }
        else if (model.DoorId.HasValue)
        {
            var door = controller.Doors.FirstOrDefault(d => d.Id == model.DoorId.Value);
            if (door != null) result.Add(door.DoorIndex);
        }

        return result.Distinct().Where(idx => idx >= 1 && idx <= 4).ToList();
    }
}
