using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Persists controller event history mirrored from the Agent.
/// </summary>
public class ControllerEventService
{
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;
    private readonly ILogger<ControllerEventService> _logger;

    public ControllerEventService(
        IDbContextFactory<GfcDbContext> contextFactory, 
        ILogger<ControllerEventService> logger)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        
        _logger.LogInformation("=== SYNC DEBUG: Controller {Sn} reports TotalEvents = {CurrentIndex} ===", controllerSerialNumber, currentIndex);
        
        // 2. Get our last read index from tracking table (more reliable than querying events)
        var lastIndexRecord = await dbContext.ControllerLastIndexes
            .FirstOrDefaultAsync(li => li.ControllerId == controller.Id, cancellationToken);
            
        uint lastReadIndex = lastIndexRecord?.LastRecordIndex ?? 0;

        // Fallback: If tracking table is empty, check events table
        if (lastReadIndex == 0)
        {
            var lastRecord = await dbContext.ControllerEvents
                .Where(e => e.ControllerId == controller.Id)
                .OrderByDescending(e => e.RawIndex)
                .FirstOrDefaultAsync(cancellationToken);
            lastReadIndex = (uint)(lastRecord?.RawIndex ?? 0);
        }

        _logger.LogInformation("=== SYNC DEBUG: Controller {Sn}: CurrentIndex={Current}, LastReadIndex={Last}, Gap={Gap} ===", 
            controllerSerialNumber, currentIndex, lastReadIndex, (int)currentIndex - (int)lastReadIndex);

        // If we are exactly up to date, do nothing.
        if (currentIndex == lastReadIndex)
        {
            _logger.LogInformation("=== SYNC DEBUG: Controller {Sn} is up to date at index {Index} ===", controllerSerialNumber, currentIndex);
            return 0;
        }

        uint startSyncIndex = lastReadIndex + 1;
        
        // Safety: If index reset (rare), causing controller to report lower index than what we have saved
        if (currentIndex < lastReadIndex) 
        {
            _logger.LogWarning("Controller index reset detected for SN {Sn} (Controller: {Current}, DB: {Saved}). Resyncing from 1.", controllerSerialNumber, currentIndex, lastReadIndex);
            startSyncIndex = 1;
        }

        // HUGE GAP CAPPING: If this is a new DB or very old sync, only grab the last 1000 to avoid packet storms.
        const uint MaxGap = 1000;
        if (currentIndex > startSyncIndex + MaxGap)
        {
            _logger.LogInformation("Large event gap detected ({Gap} items). Skipping to last {MaxGap} for stability.", currentIndex - startSyncIndex + 1, MaxGap);
            startSyncIndex = currentIndex - MaxGap + 1;
        }

        // BATCHING: Only sync up to 250 items per pass to keep UI responsive
        const uint MaxBatch = 250;
        if (currentIndex > startSyncIndex + MaxBatch)
        {
            currentIndex = startSyncIndex + MaxBatch;
            _logger.LogInformation("Processing batch of {MaxBatch} events for {Sn}. Remaining gap will be synced in next cycle.", MaxBatch, controllerSerialNumber);
        }

        int totalSaved = 0;
        var batch = new List<ControllerEvent>();
        const int BatchSize = 25;
        int itemsToSync = (int)(currentIndex - startSyncIndex + 1);

        for (uint i = startSyncIndex; i <= currentIndex; i++)
        {
            if (cancellationToken.IsCancellationRequested) break;

            try 
            {
                // Fetch single event by index
                var result = await controllerClient.GetNewEventsAsync(controllerSerialNumber.ToString(), i, cancellationToken);
                if (result.Events != null && result.Events.Any())
                {
                    var evt = result.Events[0];
                    
                    // Map Door
                    int? doorId = null;
                    if (evt.DoorNumber > 0)
                    {
                        var door = await dbContext.Doors
                            .FirstOrDefaultAsync(d => d.ControllerId == controller.Id && d.DoorIndex == evt.DoorNumber, cancellationToken);
                        doorId = door?.Id;
                    }

                    batch.Add(new ControllerEvent
                    {
                        ControllerId = controller.Id,
                        DoorId = doorId,
                        TimestampUtc = evt.TimestampUtc,
                        CardNumber = evt.CardNumber,
                        EventType = (int)evt.EventType,
                        ReasonCode = evt.ReasonCode,
                        RawIndex = (int)i,
                        CreatedUtc = DateTime.UtcNow
                    });

                    if (batch.Count >= BatchSize)
                    {
                        await dbContext.ControllerEvents.AddRangeAsync(batch, cancellationToken);
                        await dbContext.SaveChangesAsync(cancellationToken);
                        totalSaved += batch.Count;
                        batch.Clear();
                        progressCallback?.Invoke(totalSaved, itemsToSync);
                    }
                }
                
                // 5ms delay to prevent overwhelming hardware
                await Task.Delay(5, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to fetch event at index {Index} for SN {SN}: {Msg}", i, controllerSerialNumber, ex.Message);
            }
        }

        if (batch.Count > 0)
        {
            await dbContext.ControllerEvents.AddRangeAsync(batch, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            totalSaved += batch.Count;
            progressCallback?.Invoke(totalSaved, itemsToSync);
        }

        // 3. Update the LastReadIndex tracker so we don't fetch these again
        if (totalSaved > 0 || currentIndex > lastReadIndex)
        {
            var tracker = await dbContext.ControllerLastIndexes
                .FirstOrDefaultAsync(li => li.ControllerId == controller.Id, cancellationToken);
            
            // Calculate the actual last index we successfully saved
            // If totalSaved is 0 (no new events), keep lastReadIndex unchanged
            uint newIndex = totalSaved > 0 
                ? startSyncIndex + (uint)totalSaved - 1 
                : lastReadIndex;

            if (tracker == null)
            {
                tracker = new ControllerLastIndex
                {
                    ControllerId = controller.Id,
                    LastRecordIndex = newIndex
                };
                dbContext.ControllerLastIndexes.Add(tracker);
            }
            else
            {
                tracker.LastRecordIndex = newIndex;
            }
            
            await dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Updated tracker for {Sn} to index {Index} (Sync complete)", controllerSerialNumber, newIndex);
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

