using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services.Controllers;

public class SimulationTraceService : ISimulationTraceService
{
    private readonly GfcDbContext _dbContext;
    private readonly CustomAuthenticationStateProvider _authStateProvider;
    private readonly ILogger<SimulationTraceService> _logger;

    public SimulationTraceService(
        GfcDbContext dbContext,
        CustomAuthenticationStateProvider authStateProvider,
        ILogger<SimulationTraceService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _authStateProvider = authStateProvider ?? throw new ArgumentNullException(nameof(authStateProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<long> CreateTraceAsync(SimulationTraceCreateRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var currentUserId = request.UserId ?? _authStateProvider.GetCurrentUser()?.UserId;
        var trace = new SimulationControllerTrace
        {
            TimestampUtc = DateTime.UtcNow,
            UserId = currentUserId,
            Operation = string.IsNullOrWhiteSpace(request.Operation) ? "Unknown" : request.Operation,
            ControllerId = request.ControllerId,
            DoorId = request.DoorId,
            CardNumber = request.CardNumber,
            MemberId = request.MemberId,
            RequestSummary = request.RequestSummary,
            RequestPayloadJson = request.RequestPayloadJson,
            RequestPayloadRaw = request.RequestPayloadRaw,
            ExpectedResponseSummary = request.ExpectedResponseSummary,
            ExpectedResponsePayloadJson = request.ExpectedResponsePayloadJson,
            ResultStatus = "Pending",
            ResultDetails = null,
            TriggerPage = request.TriggerPage,
            IsSimulation = true
        };

        _dbContext.SimulationControllerTraces.Add(trace);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation("Simulation trace {TraceId} recorded for operation {Operation}", trace.Id, trace.Operation);

        return trace.Id;
    }

    public async Task UpdateResultAsync(long traceId, string resultStatus, string? resultDetails, CancellationToken ct = default)
    {
        if (traceId <= 0)
        {
            return;
        }

        var trace = await _dbContext.SimulationControllerTraces.FindAsync(new object?[] { traceId }, ct);
        if (trace == null)
        {
            _logger.LogWarning("Simulation trace {TraceId} not found when updating result", traceId);
            return;
        }

        trace.ResultStatus = string.IsNullOrWhiteSpace(resultStatus) ? "Pending" : resultStatus;
        trace.ResultDetails = resultDetails;

        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<(int ok, int partial, int failed, int pending)> GetStatusCountsAsync(
        string? operation = null,
        int? controllerId = null,
        int? doorId = null,
        long? cardNumber = null,
        int? memberId = null,
        CancellationToken ct = default)
    {
        var query = _dbContext.SimulationControllerTraces.AsQueryable();
        query = query.Where(t => t.IsSimulation);

        if (!string.IsNullOrWhiteSpace(operation))
        {
            query = query.Where(t => t.Operation == operation);
        }

        if (controllerId.HasValue)
        {
            query = query.Where(t => t.ControllerId == controllerId);
        }

        if (doorId.HasValue)
        {
            query = query.Where(t => t.DoorId == doorId);
        }

        if (cardNumber.HasValue)
        {
            query = query.Where(t => t.CardNumber == cardNumber);
        }

        if (memberId.HasValue)
        {
            query = query.Where(t => t.MemberId == memberId);
        }

        var grouped = await query
            .GroupBy(t => t.ResultStatus ?? "Pending")
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        var totals = (ok: 0, partial: 0, failed: 0, pending: 0);

        foreach (var item in grouped)
        {
            var status = string.IsNullOrWhiteSpace(item.Status) ? "Pending" : item.Status;
            switch (status.ToUpperInvariant())
            {
                case "OK":
                    totals.ok += item.Count;
                    break;
                case "PARTIAL":
                    totals.partial += item.Count;
                    break;
                case "FAILED":
                    totals.failed += item.Count;
                    break;
                default:
                    totals.pending += item.Count;
                    break;
            }
        }

        return totals;
    }

    public async Task<IReadOnlyList<SimulationControllerTrace>> GetTracesAsync(
        string? operation = null,
        string? resultStatus = null,
        int? controllerId = null,
        int? doorId = null,
        long? cardNumber = null,
        int? memberId = null,
        int page = 1,
        int pageSize = 50,
        CancellationToken ct = default)
    {
        if (page < 1)
        {
            page = 1;
        }

        if (pageSize <= 0)
        {
            pageSize = 50;
        }

        var query = _dbContext.SimulationControllerTraces.AsQueryable();
        query = query.Where(t => t.IsSimulation);

        if (!string.IsNullOrWhiteSpace(operation))
        {
            query = query.Where(t => t.Operation == operation);
        }

        if (!string.IsNullOrWhiteSpace(resultStatus))
        {
            query = query.Where(t => t.ResultStatus == resultStatus);
        }

        if (controllerId.HasValue)
        {
            query = query.Where(t => t.ControllerId == controllerId);
        }

        if (doorId.HasValue)
        {
            query = query.Where(t => t.DoorId == doorId);
        }

        if (cardNumber.HasValue)
        {
            query = query.Where(t => t.CardNumber == cardNumber);
        }

        if (memberId.HasValue)
        {
            query = query.Where(t => t.MemberId == memberId);
        }

        return await query
            .OrderByDescending(t => t.TimestampUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<SimulationControllerTrace>> GetTracesForExportAsync(
        string? operation = null,
        string? resultStatus = null,
        int? controllerId = null,
        int? doorId = null,
        long? cardNumber = null,
        int? memberId = null,
        CancellationToken ct = default)
    {
        const int maxExportRows = 5000;

        var query = _dbContext.SimulationControllerTraces.AsQueryable();
        query = query.Where(t => t.IsSimulation);

        if (!string.IsNullOrWhiteSpace(operation))
        {
            query = query.Where(t => t.Operation == operation);
        }

        if (!string.IsNullOrWhiteSpace(resultStatus))
        {
            query = query.Where(t => t.ResultStatus == resultStatus);
        }

        if (controllerId.HasValue)
        {
            query = query.Where(t => t.ControllerId == controllerId);
        }

        if (doorId.HasValue)
        {
            query = query.Where(t => t.DoorId == doorId);
        }

        if (cardNumber.HasValue)
        {
            query = query.Where(t => t.CardNumber == cardNumber);
        }

        if (memberId.HasValue)
        {
            query = query.Where(t => t.MemberId == memberId);
        }

        return await query
            .OrderByDescending(t => t.TimestampUtc)
            .Take(maxExportRows)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<SimulationControllerTrace>> GetTracesForReplayAsync(
        string sessionFilter,
        CancellationToken ct = default)
    {
        var query = _dbContext.SimulationControllerTraces.AsQueryable();
        query = query.Where(t => t.IsSimulation);

        if (!string.IsNullOrWhiteSpace(sessionFilter))
        {
            query = query.Where(t =>
                (t.RequestSummary != null && t.RequestSummary.Contains(sessionFilter)) ||
                (t.TriggerPage != null && t.TriggerPage.Contains(sessionFilter)) ||
                t.Operation.Contains(sessionFilter));
        }

        return await query
            .OrderBy(t => t.TimestampUtc)
            .ToListAsync(ct);
    }

    public async Task<SimulationControllerTrace?> GetTraceByIdAsync(long id, CancellationToken ct = default)
    {
        if (id <= 0)
        {
            return null;
        }

        return await _dbContext.SimulationControllerTraces.FindAsync(new object?[] { id }, ct);
    }
}
