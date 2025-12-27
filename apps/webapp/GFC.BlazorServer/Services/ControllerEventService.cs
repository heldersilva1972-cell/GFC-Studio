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

    public ControllerEventService(IDbContextFactory<GfcDbContext> contextFactory, ILogger<ControllerEventService> logger)
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
        var latest = await dbContext.ControllerEvents
            .AsNoTracking()
            .Where(e => idList.Contains(e.ControllerId))
            .GroupBy(e => e.ControllerId)
            .Select(g => g.OrderByDescending(e => e.TimestampUtc).ThenByDescending(e => e.RawIndex).First())
            .ToListAsync(cancellationToken);

        return latest.ToDictionary(e => e.ControllerId);
    }
}

