using System;
using System.Collections.Generic;
using Gfc.Agent.Api.Models;
using Gfc.Agent.Api.Services;
using Gfc.ControllerClient.Abstractions;
using Gfc.ControllerClient.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Gfc.Agent.Api.Endpoints;

internal static class ControllerEndpoints
{
    public static RouteGroupBuilder MapControllerEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/controllers");

        group.MapPost("/{sn:long}/door/{doorNo:int}/open", HandleOpenDoor);
        group.MapPost("/{sn:long}/sync-time", HandleSyncTime);
        group.MapPost("/{sn:long}/cards", HandleAddOrUpdateCard);
        group.MapDelete("/{sn:long}/cards/{cardNo:long}", HandleDeleteCard);
        group.MapPost("/{sn:long}/cards/bulk-upload", HandleBulkUploadCards);
        group.MapPost("/{sn:long}/cards/clear-all", HandleClearCards);
        group.MapGet("/{sn:long}/events", HandleGetEvents);
        group.MapGet("/{sn:long}/run-status", HandleGetRunStatus);
        group.MapGet("/{sn:long}/time-schedules", HandleGetTimeSchedules);
        group.MapPost("/{sn:long}/time-schedules/sync", HandleSyncTimeSchedules);
        group.MapGet("/{sn:long}/door-config", HandleGetDoorConfig);
        group.MapPost("/{sn:long}/door-config/sync", HandleSyncDoorConfig);
        group.MapGet("/{sn:long}/auto-open", HandleGetAutoOpen);
        group.MapPost("/{sn:long}/auto-open/sync", HandleSyncAutoOpen);
        group.MapGet("/{sn:long}/advanced-door-modes", HandleGetAdvancedDoorModes);
        group.MapPost("/{sn:long}/advanced-door-modes/sync", HandleSyncAdvancedDoorModes);
        group.MapGet("/{sn:long}/network-config", HandleGetNetworkConfig);
        group.MapPost("/{sn:long}/network-config", HandleSetNetworkConfig);
        group.MapPost("/{sn:long}/reboot", HandleReboot);

        return group;
    }

    private static Task<IResult> HandleOpenDoor(
        HttpContext context,
        long sn,
        int doorNo,
        OpenDoorRequest request,
        IMengqiControllerClient client,
        ApiRequestExecutor executor) =>
        executor.RunAsync(
            context,
            (uint)sn,
            $"open-door-{doorNo}",
            ct => client.OpenDoorAsync((uint)sn, doorNo, request.DurationSec, ct));

    private static Task<IResult> HandleSyncTime(
        HttpContext context,
        long sn,
        SyncTimeRequest request,
        IMengqiControllerClient client,
        ApiRequestExecutor executor)
    {
        var time = request.ServerTimeUtc ?? DateTime.UtcNow;
        return executor.RunAsync(
            context,
            (uint)sn,
            "sync-time",
            ct => client.SyncTimeAsync((uint)sn, time, ct));
    }

    private static Task<IResult> HandleAddOrUpdateCard(
        HttpContext context,
        long sn,
        CardPrivilegeModel card,
        IMengqiControllerClient client,
        ApiRequestExecutor executor) =>
        executor.RunAsync(
            context,
            (uint)sn,
            "add-card",
            ct => client.AddOrUpdateCardAsync((uint)sn, card, ct));

    private static Task<IResult> HandleDeleteCard(
        HttpContext context,
        long sn,
        long cardNo,
        IMengqiControllerClient client,
        ApiRequestExecutor executor) =>
        executor.RunAsync(
            context,
            (uint)sn,
            "delete-card",
            ct => client.DeleteCardAsync((uint)sn, cardNo, ct));

    private static Task<IResult> HandleBulkUploadCards(
        HttpContext context,
        long sn,
        BulkCardUploadRequest request,
        IMengqiControllerClient client,
        ApiRequestExecutor executor)
    {
        var cards = request.Cards ?? new List<CardPrivilegeModel>();
        return executor.RunAsync(
            context,
            (uint)sn,
            "bulk-upload-cards",
            ct => client.BulkUploadCardsAsync((uint)sn, cards, ct));
    }

    private static Task<IResult> HandleClearCards(
        HttpContext context,
        long sn,
        IMengqiControllerClient client,
        ApiRequestExecutor executor) =>
        executor.RunAsync(
            context,
            (uint)sn,
            "clear-cards",
            ct => client.ClearAllCardsAsync((uint)sn, ct));

    private static Task<IResult> HandleGetEvents(
        HttpContext context,
        long sn,
        uint lastIndex,
        IMengqiControllerClient client,
        ApiRequestExecutor executor) =>
        executor.RunAsync(
            context,
            (uint)sn,
            "get-events",
            ct => client.GetNewEventsAsync((uint)sn, lastIndex, ct));

    private static Task<IResult> HandleGetRunStatus(
        HttpContext context,
        long sn,
        IMengqiControllerClient client,
        ApiRequestExecutor executor) =>
        executor.RunAsync(
            context,
            (uint)sn,
            "get-run-status",
            ct => client.GetRunStatusAsync((uint)sn, ct));

    private static Task<IResult> HandleGetTimeSchedules(
        HttpContext context,
        long sn,
        IMengqiControllerClient client,
        ApiRequestExecutor executor) =>
        executor.RunAsync(
            context,
            (uint)sn,
            "get-time-schedules",
            ct => client.ReadTimeSchedulesAsync((uint)sn, ct));

    private static Task<IResult> HandleSyncTimeSchedules(
        HttpContext context,
        long sn,
        TimeScheduleSyncRequest request,
        IMengqiControllerClient client,
        ApiRequestExecutor executor)
    {
        if (request.Schedule is null)
        {
            return Task.FromResult(Results.Json(ApiResponse.Failure("Schedule payload is required.")));
        }

        return executor.RunAsync(
            context,
            (uint)sn,
            "sync-time-schedules",
            ct => client.WriteTimeSchedulesAsync((uint)sn, request.Schedule, ct));
    }

    private static Task<IResult> HandleGetDoorConfig(
        HttpContext context,
        long sn,
        IMengqiControllerClient client,
        ApiRequestExecutor executor) =>
        executor.RunAsync(
            context,
            (uint)sn,
            "get-door-config",
            ct => client.ReadExtendedConfigAsync((uint)sn, ct));

    private static Task<IResult> HandleSyncDoorConfig(
        HttpContext context,
        long sn,
        DoorConfigSyncRequest request,
        IMengqiControllerClient client,
        ApiRequestExecutor executor)
    {
        if (request.Config is null)
        {
            return Task.FromResult(Results.Json(ApiResponse.Failure("Door configuration payload is required.")));
        }

        return executor.RunAsync(
            context,
            (uint)sn,
            "sync-door-config",
            ct => client.WriteExtendedDoorConfigAsync((uint)sn, request.Config, ct));
    }

    private static Task<IResult> HandleGetAutoOpen(
        HttpContext context,
        long sn,
        IMengqiControllerClient client,
        ApiRequestExecutor executor) =>
        executor.RunAsync(
            context,
            (uint)sn,
            "get-auto-open",
            ct => client.ReadTimeSchedulesAsync((uint)sn, ct));

    private static Task<IResult> HandleSyncAutoOpen(
        HttpContext context,
        long sn,
        AutoOpenSyncRequest request,
        IMengqiControllerClient client,
        ApiRequestExecutor executor)
    {
        if (request.Schedule is null)
        {
            return Task.FromResult(Results.Json(ApiResponse.Failure("Auto-open schedule payload is required.")));
        }

        return executor.RunAsync(
            context,
            (uint)sn,
            "sync-auto-open",
            ct => client.WriteTimeSchedulesAsync((uint)sn, request.Schedule, ct));
    }

    private static Task<IResult> HandleGetAdvancedDoorModes(
        HttpContext context,
        long sn,
        IMengqiControllerClient client,
        ApiRequestExecutor executor) =>
        executor.RunAsync(
            context,
            (uint)sn,
            "get-advanced-door-modes",
            ct => client.ReadExtendedConfigAsync((uint)sn, ct));

    private static Task<IResult> HandleSyncAdvancedDoorModes(
        HttpContext context,
        long sn,
        AdvancedDoorModesSyncRequest request,
        IMengqiControllerClient client,
        ApiRequestExecutor executor)
    {
        if (request.Config is null)
        {
            return Task.FromResult(Results.Json(ApiResponse.Failure("Advanced door modes payload is required.")));
        }

        return executor.RunAsync(
            context,
            (uint)sn,
            "sync-advanced-door-modes",
            ct => client.WriteExtendedDoorConfigAsync((uint)sn, request.Config, ct));
    }

    private static Task<IResult> HandleGetNetworkConfig(
        HttpContext context,
        long sn,
        IMengqiControllerClient client,
        ApiRequestExecutor executor) =>
        executor.RunAsync(
            context,
            (uint)sn,
            "get-network-config",
            ct => client.GetNetworkConfigAsync((uint)sn, ct));

    private static Task<IResult> HandleSetNetworkConfig(
        HttpContext context,
        long sn,
        ControllerNetworkConfig config,
        IMengqiControllerClient client,
        ApiRequestExecutor executor) =>
        executor.RunAsync(
            context,
            (uint)sn,
            "set-network-config",
            ct => client.SetNetworkConfigAsync((uint)sn, config, ct));

    private static Task<IResult> HandleReboot(
        HttpContext context,
        long sn,
        IMengqiControllerClient client,
        ApiRequestExecutor executor) =>
        executor.RunAsync(
            context,
            (uint)sn,
            "reboot",
            ct => client.RebootControllerAsync((uint)sn, ct));
}

