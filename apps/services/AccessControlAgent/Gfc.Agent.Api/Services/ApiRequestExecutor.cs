using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Gfc.Agent.Api.Configuration;
using Gfc.Agent.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gfc.Agent.Api.Services;

internal sealed class ApiRequestExecutor
{
    private readonly ILogger<ApiRequestExecutor> _logger;
    private readonly IOptionsMonitor<AgentApiOptions> _options;

    public ApiRequestExecutor(ILogger<ApiRequestExecutor> logger, IOptionsMonitor<AgentApiOptions> options)
    {
        _logger = logger;
        _options = options;
    }

    public Task<IResult> RunAsync(
        HttpContext context,
        uint controllerSn,
        string action,
        Func<CancellationToken, Task> handler) =>
        RunAsync<object?>(
            context,
            controllerSn,
            action,
            async ct =>
            {
                await handler(ct).ConfigureAwait(false);
                return null;
            });

    public async Task<IResult> RunAsync<T>(
        HttpContext context,
        uint controllerSn,
        string action,
        Func<CancellationToken, Task<T>> handler)
    {
        var stopwatch = Stopwatch.StartNew();
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(context.RequestAborted);
        cts.CancelAfter(_options.CurrentValue.RequestTimeout);

        try
        {
            var payload = await handler(cts.Token).ConfigureAwait(false);
            stopwatch.Stop();
            _logger.LogInformation(
                "Controller {ControllerSn} action {Action} succeeded in {Duration} ms",
                controllerSn,
                action,
                stopwatch.ElapsedMilliseconds);
            return Results.Json(ApiResponse<T>.Ok(payload));
        }
        catch (OperationCanceledException) when (!context.RequestAborted.IsCancellationRequested)
        {
            stopwatch.Stop();
            _logger.LogWarning(
                "Controller {ControllerSn} action {Action} timed out after {Duration} ms",
                controllerSn,
                action,
                stopwatch.ElapsedMilliseconds);
            return Results.Json(
                ApiResponse<T>.Failure("Operation timed out."),
                statusCode: StatusCodes.Status504GatewayTimeout);
        }
        catch (NotImplementedException ex)
        {
            stopwatch.Stop();
            _logger.LogWarning(
                ex,
                "Controller {ControllerSn} action {Action} is not implemented.",
                controllerSn,
                action);
            return Results.Json(
                ApiResponse<T>.Failure("Operation not yet implemented."),
                statusCode: StatusCodes.Status501NotImplemented);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(
                ex,
                "Controller {ControllerSn} action {Action} failed after {Duration} ms",
                controllerSn,
                action,
                stopwatch.ElapsedMilliseconds);
            return Results.Json(
                ApiResponse<T>.Failure("Controller request failed. See logs for details."),
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}

