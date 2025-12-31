using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

public class ControllerTestService
{
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;
    private readonly ILogger<ControllerTestService> _logger;

    public ControllerTestService(
        IDbContextFactory<GfcDbContext> contextFactory,
        ILogger<ControllerTestService> logger)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task LogCommandAsync(int controllerId, string action, ApiResult result, int? latencyMs, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var log = new ControllerCommandLog
            {
                ControllerId = controllerId,
                Action = action,
                Success = result.Success,
                Error = result.Success ? null : result.Message,
                LatencyMs = latencyMs,
                TimestampUtc = DateTime.UtcNow
            };

            dbContext.ControllerCommandLogs.Add(log);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to log command for controller {ControllerId}", controllerId);
            // Don't throw - logging failure shouldn't break the operation
        }
    }

    public async Task<(ApiResult<T> Result, int LatencyMs)> ExecuteWithLoggingAsync<T>(
        int controllerId,
        string action,
        Func<Task<ApiResult<T>>> operation,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = await operation();
        stopwatch.Stop();

        await LogCommandAsync(controllerId, action, new ApiResult { Success = result.Success, Message = result.Message }, (int)stopwatch.ElapsedMilliseconds, cancellationToken);

        return (result, (int)stopwatch.ElapsedMilliseconds);
    }

    public async Task<(ApiResult Result, int LatencyMs)> ExecuteWithLoggingAsync(
        int controllerId,
        string action,
        Func<Task<ApiResult>> operation,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = await operation();
        stopwatch.Stop();

        await LogCommandAsync(controllerId, action, result, (int)stopwatch.ElapsedMilliseconds, cancellationToken);

        return (result, (int)stopwatch.ElapsedMilliseconds);
    }
}

