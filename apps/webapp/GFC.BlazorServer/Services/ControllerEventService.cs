using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using GFC.BlazorServer.Hubs;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Persists controller event history mirrored from the Agent.
/// </summary>
public class ControllerEventService
{
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;
    private readonly ILogger<ControllerEventService> _logger;
    private readonly IHubContext<ControllerEventHub> _hubContext;

    public ControllerEventService(
        IDbContextFactory<GfcDbContext> contextFactory, 
        ILogger<ControllerEventService> logger,
        IHubContext<ControllerEventHub> hubContext)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    }

    /// <summary>
    /// Saves events from ControllerEventDto (e.g., from simulation).
    /// </summary>
    public async Task SaveEventsFromDtoAsync(uint controllerSerialNumber, IEnumerable<ControllerEventDto> eventDtos, uint newLastIndex, CancellationToken cancellationToken = default)
    {
        if (eventDtos == null)
        {
            return;
        }

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var controller = await dbContext.Controllers
            .Include(c => c.Doors)
            .FirstOrDefaultAsync(c => c.SerialNumber == controllerSerialNumber, cancellationToken);

        if (controller == null)
        {
            throw new InvalidOperationException($"Controller SN {controllerSerialNumber} is not registered.");
        }

        var incomingList = eventDtos.ToList();
        if (incomingList.Count == 0 && newLastIndex == 0)
        {
            return;
        }

        var incomingIndexes = incomingList.Select(e => (int)e.RawIndex).ToArray();
        var existingIndexes = await dbContext.ControllerEvents
            .Where(e => e.ControllerId == controller.Id && incomingIndexes.Contains(e.RawIndex))
            .Select(e => e.RawIndex)
            .ToListAsync(cancellationToken);

        var now = DateTime.UtcNow;
        var newEvents = incomingList
            .Where(e => !existingIndexes.Contains((int)e.RawIndex))
            .Select(e =>
            {
                // Map DoorNumber to DoorId
                int? doorId = null;
                if (e.DoorNumber.HasValue)
                {
                    var door = controller.Doors.FirstOrDefault(d => d.DoorIndex == e.DoorNumber.Value);
                    doorId = door?.Id;
                }

                return new ControllerEvent
                {
                    ControllerId = controller.Id,
                    DoorId = doorId,
                    TimestampUtc = e.TimestampUtc,
                    CardNumber = e.CardNumber,
                    EventType = e.EventType,
                    ReasonCode = e.ReasonCode,
                    IsByCard = e.IsByCard,
                    IsByButton = e.IsByButton,
                    RawIndex = (int)e.RawIndex,
                    RawData = e.RawData,
                    IsSimulated = e.IsSimulated,
                    DoorOrReader = e.DoorNumber ?? 0,
                    CreatedUtc = now
                };
            })
            .ToList();

        if (newEvents.Count > 0)
        {
            await dbContext.ControllerEvents.AddRangeAsync(newEvents, cancellationToken);
        }

        var lastIndex = await dbContext.ControllerLastIndexes
            .FirstOrDefaultAsync(li => li.ControllerId == controller.Id, cancellationToken);
        if (lastIndex == null)
        {
            lastIndex = new ControllerLastIndex
            {
                ControllerId = controller.Id,
                LastRecordIndex = newLastIndex
            };
            dbContext.ControllerLastIndexes.Add(lastIndex);
        }
        else
        {
            lastIndex.LastRecordIndex = newLastIndex;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        // Notify UI of new events
        _ = _hubContext.Clients.All.SendAsync("ReceiveEventUpdate", controller.Id, cancellationToken);

        _logger.LogInformation(
            "Saved {Count} new simulated events for controller {ControllerName} (SN {Serial}). LastIndex={Index}",
            newEvents.Count,
            controller.Name,
            controllerSerialNumber,
            newLastIndex);
    }

    public async Task<int> SyncFromControllerAsync(
        uint controllerSerialNumber, 
        Controllers.IControllerClient controllerClient,
        Action<int, int>? progressCallback = null,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        
        var controller = await dbContext.Controllers
            .FirstOrDefaultAsync(c => c.SerialNumber == controllerSerialNumber, cancellationToken);
            
        if (controller == null)
            throw new InvalidOperationException($"Controller SN {controllerSerialNumber} is not registered.");

        // 1. Get current hardware status to see total events available
        var status = await controllerClient.GetRunStatusAsync(controllerSerialNumber.ToString(), cancellationToken);
        if (status == null) return 0;

        uint currentIndex = status.TotalEvents;
        
        // 2. Get our last read index from tracking table
        var lastIndexRecord = await dbContext.ControllerLastIndexes
            .FirstOrDefaultAsync(li => li.ControllerId == controller.Id, cancellationToken);
            
        uint lastReadIndex = lastIndexRecord?.LastRecordIndex ?? 0;

        _logger.LogInformation("Sync {Sn}: HardwareIndex={Current}, DBIndex={Last}", controllerSerialNumber, currentIndex, lastReadIndex);

        if (currentIndex == lastReadIndex) return 0;

        uint startSyncIndex = lastReadIndex + 1;
        
        // 4.3 Wraparound / Reset Logic
        if (currentIndex < lastReadIndex) 
        {
            _logger.LogWarning("SYNC RESET: Controller {Sn} index ({Current}) is less than DB ({Last}). Treating as reset/wrap.", 
                controllerSerialNumber, currentIndex, lastReadIndex);
            startSyncIndex = 1; 
        }

        // GAP CAPPING for stability - if we are thousands behind, just grab the latest 500
        const uint MaxGap = 2000;
        if (currentIndex > startSyncIndex + MaxGap)
        {
            _logger.LogInformation("Gap too large ({Gap}). Syncing last {MaxGap}.", currentIndex - startSyncIndex, MaxGap);
            startSyncIndex = currentIndex - MaxGap + 1;
        }

        // 4.5 Aggressive Batch Capping for UI Stability
        const uint MaxBatchPerPass = 1000;
        uint syncLimitIndex = Math.Min(currentIndex, startSyncIndex + MaxBatchPerPass - 1);

        int totalSaved = 0;
        uint lastSyncedIndex = lastReadIndex;
        var batch = new List<ControllerEvent>();
        const int BatchSize = 25;
        int itemsToSync = (int)(syncLimitIndex - startSyncIndex + 1);

        // 4.6 Optimized door entity mapping
        var doorMap = await dbContext.Doors
            .Where(d => d.ControllerId == controller.Id)
            .ToDictionaryAsync(d => d.DoorIndex, d => d, cancellationToken);

        // 4.4 Optimized Duplicate prevention
        var existingIndices = await dbContext.ControllerEvents
            .Where(e => e.ControllerId == controller.Id && e.RawIndex >= (int)startSyncIndex && e.RawIndex <= (int)syncLimitIndex)
            .Select(e => e.RawIndex)
            .ToListAsync(cancellationToken);
        var existingSet = new HashSet<int>(existingIndices);

        for (uint i = startSyncIndex; i <= syncLimitIndex; i++)
        {
            if (cancellationToken.IsCancellationRequested) break;

            try 
            {
                if (existingSet.Contains((int)i)) 
                {
                    lastSyncedIndex = i;
                    continue;
                }

                var result = await controllerClient.GetNewEventsAsync(controllerSerialNumber.ToString(), i, cancellationToken);
                if (result.Events != null && result.Events.Any())
                {
                    var evt = result.Events[0];
                    
                    int? doorId = null;
                    if (evt.DoorNumber > 0 && doorMap.TryGetValue(evt.DoorNumber.Value, out var door))
                    {
                        doorId = door.Id;
                    }

                    batch.Add(new ControllerEvent
                    {
                        ControllerId = controller.Id,
                        DoorId = doorId,
                        TimestampUtc = evt.TimestampUtc,
                        CardNumber = evt.CardNumber,
                        EventType = (int)evt.EventType,
                        IsByCard = evt.IsByCard,
                        IsByButton = evt.IsByButton,
                        RawIndex = (int)evt.RawIndex, // Use actual hardware index, not loop counter
                        DoorOrReader = evt.DoorNumber ?? 0,
                        RawData = evt.RawData, // Store raw packet for debugging
                        CreatedUtc = DateTime.UtcNow
                    });

                    // Update existing set immediately to prevent duplicates if sync restarts
                    existingSet.Add((int)i);

                    if (batch.Count >= BatchSize)
                    {
                        await dbContext.ControllerEvents.AddRangeAsync(batch, cancellationToken);
                        await dbContext.SaveChangesAsync(cancellationToken);
                        totalSaved += batch.Count;
                        batch.Clear();
                        progressCallback?.Invoke(totalSaved, itemsToSync);
                    }
                }
                
                lastSyncedIndex = i;
                await Task.Delay(1, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to fetch event {Index} for {SN}: {Msg}", i, controllerSerialNumber, ex.Message);
            }
        }

        if (batch.Count > 0)
        {
            await dbContext.ControllerEvents.AddRangeAsync(batch, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            totalSaved += batch.Count;
            progressCallback?.Invoke(totalSaved, itemsToSync);
        }

        // 3. Update the LastReadIndex tracker (only to where we actually got)
        if (totalSaved > 0 || lastSyncedIndex != lastReadIndex)
        {
            var tracker = await dbContext.ControllerLastIndexes
                .FirstOrDefaultAsync(li => li.ControllerId == controller.Id, cancellationToken);
            
            if (tracker == null)
            {
                tracker = new ControllerLastIndex { ControllerId = controller.Id, LastRecordIndex = lastSyncedIndex };
                dbContext.ControllerLastIndexes.Add(tracker);
            }
            else
            {
                tracker.LastRecordIndex = lastSyncedIndex;
            }
            
            await dbContext.SaveChangesAsync(cancellationToken);

            // Notify UI of new events
            _ = _hubContext.Clients.All.SendAsync("ReceiveEventUpdate", controller.Id, cancellationToken);

            // MANDATORY ACK (0xB2)
            try
            {
                await controllerClient.AcknowledgeEventsAsync(controllerSerialNumber.ToString(), lastSyncedIndex, cancellationToken);
                _logger.LogInformation("Acknowledged index {Index} for {Sn}", lastSyncedIndex, controllerSerialNumber);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to ACK index {Index} for {Sn}: {Msg}", lastSyncedIndex, controllerSerialNumber, ex.Message);
            }
        }

        return totalSaved;
    }

    public async Task SaveEventsAsync(uint controllerSerialNumber, IEnumerable<ControllerEvent> events, uint newLastIndex, CancellationToken cancellationToken = default)
    {
        if (events == null)
        {
            return;
        }

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var controller = await dbContext.Controllers
            .Include(c => c.Doors)
            .FirstOrDefaultAsync(c => c.SerialNumber == controllerSerialNumber, cancellationToken);

        if (controller == null)
        {
            throw new InvalidOperationException($"Controller SN {controllerSerialNumber} is not registered.");
        }

        var incomingList = events.ToList();
        if (incomingList.Count == 0 && newLastIndex == 0)
        {
            return;
        }

        var incomingIndexes = incomingList.Select(e => (int)e.RawIndex).ToArray();
        var existingIndexes = await dbContext.ControllerEvents
            .Where(e => e.ControllerId == controller.Id && incomingIndexes.Contains(e.RawIndex))
            .Select(e => e.RawIndex)
            .ToListAsync(cancellationToken);

        var now = DateTime.UtcNow;
        var newEvents = incomingList
            .Where(e => !existingIndexes.Contains((int)e.RawIndex))
            .Select(e =>
            {
                // DoorId is already set from the incoming ControllerEvent entity
                // (ControllerEventDto from simulation will need to be converted separately if needed)
                int? doorId = e.DoorId;

                var clone = new ControllerEvent
                {
                    ControllerId = controller.Id,
                    DoorId = doorId,
                    TimestampUtc = e.TimestampUtc,
                    CardNumber = e.CardNumber,
                    EventType = e.EventType,
                    ReasonCode = e.ReasonCode,
                    IsByCard = e.IsByCard,
                    IsByButton = e.IsByButton,
                    RawIndex = e.RawIndex,
                    RawData = e.RawData,
                    DoorOrReader = e.DoorOrReader,
                    IsSimulated = e.IsSimulated,
                    CreatedUtc = now
                };

                return clone;
            })
            .ToList();

        if (newEvents.Count > 0)
        {
            await dbContext.ControllerEvents.AddRangeAsync(newEvents, cancellationToken);
        }

        var lastIndex = await dbContext.ControllerLastIndexes
            .FirstOrDefaultAsync(li => li.ControllerId == controller.Id, cancellationToken);
        if (lastIndex == null)
        {
            lastIndex = new ControllerLastIndex
            {
                ControllerId = controller.Id,
                LastRecordIndex = newLastIndex
            };
            dbContext.ControllerLastIndexes.Add(lastIndex);
        }
        else
        {
            lastIndex.LastRecordIndex = newLastIndex;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Saved {Count} new events for controller {ControllerName} (SN {Serial}). LastIndex={Index}",
            newEvents.Count,
            controller.Name,
            controllerSerialNumber,
            newLastIndex);
    }

    public async Task<IReadOnlyList<ControllerEvent>> GetEventsAsync(
        DateTime? startUtc = null,
        DateTime? endUtc = null,
        int? controllerId = null,
        int? doorId = null,
        bool? isByCard = null,
        bool? isByButton = null,
        bool? isSimulated = null,
        int? limit = 500,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var query = dbContext.ControllerEvents
            .AsNoTracking()
            .Include(e => e.Controller)
            .Include(e => e.Door)
            .AsQueryable();

        if (startUtc.HasValue)
        {
            query = query.Where(e => e.TimestampUtc >= startUtc.Value);
        }

        if (endUtc.HasValue)
        {
            query = query.Where(e => e.TimestampUtc <= endUtc.Value);
        }

        if (controllerId.HasValue)
        {
            query = query.Where(e => e.ControllerId == controllerId.Value);
        }

        if (doorId.HasValue)
        {
            query = query.Where(e => e.DoorId == doorId.Value);
        }

        if (isByCard.HasValue)
        {
            query = query.Where(e => e.IsByCard == isByCard.Value);
        }

        if (isByButton.HasValue)
        {
            query = query.Where(e => e.IsByButton == isByButton.Value);
        }

        if (isSimulated.HasValue)
        {
            query = query.Where(e => e.IsSimulated == isSimulated.Value);
        }

        if (limit.HasValue && limit > 0)
        {
            query = query.Take(limit.Value);
        }

        return await query
            .OrderByDescending(e => e.TimestampUtc)
            .ThenByDescending(e => e.RawIndex)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<ControllerEvent> Events, int TotalCount)> GetEventsPageAsync(
        int page,
        int pageSize,
        DateTime? startUtc = null,
        DateTime? endUtc = null,
        int? controllerId = null,
        int? doorId = null,
        bool? isByCard = null,
        bool? isByButton = null,
        bool? isSimulated = null,
        CancellationToken cancellationToken = default)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 50;

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var query = dbContext.ControllerEvents.AsQueryable();

        if (startUtc.HasValue)
        {
            query = query.Where(e => e.TimestampUtc >= startUtc.Value);
        }

        if (endUtc.HasValue)
        {
            query = query.Where(e => e.TimestampUtc <= endUtc.Value);
        }

        if (controllerId.HasValue)
        {
            query = query.Where(e => e.ControllerId == controllerId.Value);
        }

        if (doorId.HasValue)
        {
            query = query.Where(e => e.DoorId == doorId.Value);
        }

        if (isByCard.HasValue)
        {
            query = query.Where(e => e.IsByCard == isByCard.Value);
        }

        if (isByButton.HasValue)
        {
            query = query.Where(e => e.IsByButton == isByButton.Value);
        }

        if (isSimulated.HasValue)
        {
            query = query.Where(e => e.IsSimulated == isSimulated.Value);
        }

        var total = await query.CountAsync(cancellationToken);

        var events = await query
            .Include(e => e.Controller)
            .Include(e => e.Door)
            .OrderByDescending(e => e.TimestampUtc)
            .ThenByDescending(e => e.RawIndex)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (events, total);
    }

    public async Task<IDictionary<int, ControllerEvent>> GetLatestEventsByControllerAsync(IEnumerable<int> controllerIds, CancellationToken cancellationToken = default)
    {
        var idList = controllerIds.Distinct().ToList();
        if (idList.Count == 0)
        {
            return new Dictionary<int, ControllerEvent>();
        }

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        // Optimized Raw SQL High Performance Query (avoids EF GroupBy timeout)
        var ids = string.Join(",", idList);
        // Protect against empty list causing SQL error
        if (string.IsNullOrEmpty(ids)) return new Dictionary<int, ControllerEvent>();

        var sql = $@"
            SELECT *
            FROM (
                SELECT *, ROW_NUMBER() OVER(PARTITION BY ControllerId ORDER BY TimestampUtc DESC, RawIndex DESC) as RowNum
                FROM ControllerEvents WITH (NOLOCK)
                WHERE ControllerId IN ({ids})
            ) t
            WHERE t.RowNum = 1";

        var latest = await dbContext.ControllerEvents
            .FromSqlRaw(sql)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return latest.ToDictionary(e => e.ControllerId);
    }
}

