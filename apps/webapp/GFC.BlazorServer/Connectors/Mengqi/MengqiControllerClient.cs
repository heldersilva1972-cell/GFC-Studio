using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GFC.BlazorServer.Connectors.Mengqi.Abstractions;
using GFC.BlazorServer.Connectors.Mengqi.Configuration;
using GFC.BlazorServer.Connectors.Mengqi.Models;
using GFC.BlazorServer.Connectors.Mengqi.Protocol;
using GFC.BlazorServer.Connectors.Mengqi.Services;
using GFC.BlazorServer.Connectors.Mengqi.Transport;
using GFC.BlazorServer.Connectors.Mengqi.Packets;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Connectors.Mengqi;

public sealed class MengqiControllerClient : IMengqiControllerClient, IDisposable
{
    private readonly ControllerCommandProfiles _commands;
    private readonly WgCommandDispatcher _dispatcher;
    private readonly ILogger<MengqiControllerClient>? _logger;
    private readonly IControllerTransport _transport;
    private bool _disposed;

    public MengqiControllerClient(
        ControllerClientOptions options,
        IControllerEndpointResolver endpointResolver,
        IControllerTransport? transport = null,
        ILogger<MengqiControllerClient>? logger = null)
    {
        _ = options ?? throw new ArgumentNullException(nameof(options));
        endpointResolver = endpointResolver ?? throw new ArgumentNullException(nameof(endpointResolver));

        _commands = options.CommandProfiles ?? throw new ArgumentNullException(nameof(options.CommandProfiles));
        _transport = transport ?? new UdpControllerTransport();
        _dispatcher = new WgCommandDispatcher(options, endpointResolver, _transport);
        _logger = logger;
    }

    public async Task OpenDoorAsync(uint controllerSn, int doorNo, int? durationSeconds = null, CancellationToken cancellationToken = default)
    {
        var payload = WgPayloadFactory.BuildOpenDoorPayload(_commands.OpenDoor, doorNo, durationSeconds);
        await SendAndExpectAck(controllerSn, _commands.OpenDoor, payload, cancellationToken).ConfigureAwait(false);
        _logger?.LogInformation("Opened door {Door} on controller {ControllerSn}", doorNo, controllerSn);
    }

    public async Task SyncTimeAsync(uint controllerSn, DateTime serverTime, CancellationToken cancellationToken = default)
    {
        var payload = WgPayloadFactory.BuildSyncTimePayload(_commands.SyncTime, serverTime);
        await SendAndExpectAck(controllerSn, _commands.SyncTime, payload, cancellationToken).ConfigureAwait(false);
    }

    public async Task AddOrUpdateCardAsync(uint controllerSn, CardPrivilegeModel model, CancellationToken cancellationToken = default)
    {
        var payload = WgPayloadFactory.BuildPrivilegePayload(_commands.AddOrUpdateCard, model, markAsDeleted: false);
        await SendAndExpectAck(controllerSn, _commands.AddOrUpdateCard, payload, cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteCardAsync(uint controllerSn, long cardNumber, CancellationToken cancellationToken = default)
    {
        var model = new CardPrivilegeModel
        {
            CardNumber = cardNumber,
            DoorList = Array.Empty<int>()
        };
        var payload = WgPayloadFactory.BuildPrivilegePayload(_commands.DeleteCard, model, markAsDeleted: true);
        await SendAndExpectAck(controllerSn, _commands.DeleteCard, payload, cancellationToken).ConfigureAwait(false);
    }

    public async Task BulkUploadCardsAsync(uint controllerSn, IEnumerable<CardPrivilegeModel> cards, CancellationToken cancellationToken = default)
    {
        foreach (var card in cards)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await AddOrUpdateCardAsync(controllerSn, card, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task ClearAllCardsAsync(uint controllerSn, CancellationToken cancellationToken = default)
    {
        var payload = Array.Empty<byte>();
        await SendAndExpectAck(controllerSn, _commands.ClearAllCards, payload, cancellationToken).ConfigureAwait(false);
    }

    public async Task<(IReadOnlyList<ControllerEvent> Events, uint ControllerLastIndex)> GetNewEventsAsync(
        uint controllerSn,
        uint lastKnownIndex,
        CancellationToken cancellationToken = default)
    {
        var payload = new byte[4];
        BitConverter.GetBytes(lastKnownIndex).CopyTo(payload, 0);
        var response = await _dispatcher.SendAsync(controllerSn, _commands.GetEvents, payload, cancellationToken).ConfigureAwait(false);
        WgResponseParser.EnsureAck(response, _commands.GetEvents);
        return WgResponseParser.ParseEvents(response);
    }

    public async Task<RunStatusModel> GetRunStatusAsync(uint controllerSn, CancellationToken cancellationToken = default)
    {
        var response = await _dispatcher.SendAsync(controllerSn, _commands.GetRunStatus, Array.Empty<byte>(), cancellationToken).ConfigureAwait(false);
        WgResponseParser.EnsureAck(response, _commands.GetRunStatus);
        return WgResponseParser.ParseRunStatus(response);
    }

    public async Task<byte[]> ReadFlashBlockAsync(uint controllerSn, FlashArea area, int start, int length, CancellationToken cancellationToken = default)
    {
        var payload = WgPayloadFactory.BuildFlashReadPayload(_commands.ReadFlash, area, start, length);
        var response = await _dispatcher.SendAsync(controllerSn, _commands.ReadFlash, payload, cancellationToken).ConfigureAwait(false);
        WgResponseParser.EnsureAck(response, _commands.ReadFlash);
        return WgResponseParser.GetPayload(response).ToArray();
    }

    public async Task WriteFlashBlockAsync(uint controllerSn, FlashArea area, int start, ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
    {
        var payload = WgPayloadFactory.BuildFlashWritePayload(_commands.WriteFlash, area, start, data.Span);
        await SendAndExpectAck(controllerSn, _commands.WriteFlash, payload, cancellationToken).ConfigureAwait(false);
    }

    public Task<TimeScheduleDto> ReadTimeSchedulesAsync(uint controllerSn, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Time schedule parsing must be implemented once the WG3000 structures are defined.");
    }

    public Task WriteTimeSchedulesAsync(uint controllerSn, TimeScheduleWriteDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Time schedule serialization must be implemented from the WG3000 schedule layout.");
    }

    public Task<ExtendedConfigDto> ReadExtendedConfigAsync(uint controllerSn, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Extended config parsing requires WG3000 memory map.");
    }

    public Task WriteExtendedDoorConfigAsync(uint controllerSn, ExtendedDoorConfigWriteDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Door config serialization requires WG3000 memory map.");
    }

    public Task<ControllerNetworkConfig> GetNetworkConfigAsync(uint controllerSn, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Network config parsing requires WG3000 command map.");
    }

    public Task SetNetworkConfigAsync(uint controllerSn, ControllerNetworkConfig config, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Network config serialization requires WG3000 command map.");
    }

    public Task SetAllowedPcAndCommPasswordAsync(uint controllerSn, AllowedPcConfig config, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Allowed PC serialization requires WG3000 command map.");
    }

    public async Task RebootControllerAsync(uint controllerSn, CancellationToken cancellationToken = default)
    {
        var payload = Array.Empty<byte>();
        await SendAndExpectAck(controllerSn, _commands.Reboot, payload, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<DiscoveryResult>> DiscoverControllersAsync(CancellationToken cancellationToken = default)
    {
        var results = new List<DiscoveryResult>();
        var seenSns = new HashSet<uint>();

        try 
        {
             await foreach (var response in _dispatcher.BroadcastAsync(_commands.Search, Array.Empty<byte>(), cancellationToken).ConfigureAwait(false))
             {
                 if (response.Length >= 64) // N3000 standard frame
                 {
                     try 
                     {
                         var result = WgResponseParser.ParseDiscovery(response);
                         if (seenSns.Add(result.SerialNumber))
                         {
                             results.Add(result);
                         }
                     }
                     catch (Exception ex)
                     {
                        _logger?.LogDebug(ex, "Failed to parse discovery response");
                     }
                 }
             }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Discovery failed");
        }
        
        return results;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _transport.Dispose();
    }

    private async Task SendAndExpectAck(uint controllerSn, WgCommandProfile profile, ReadOnlyMemory<byte> payload, CancellationToken cancellationToken)
    {
        var response = await _dispatcher.SendAsync(controllerSn, profile, payload, cancellationToken).ConfigureAwait(false);
        WgResponseParser.EnsureAck(response, profile);
    }
}


