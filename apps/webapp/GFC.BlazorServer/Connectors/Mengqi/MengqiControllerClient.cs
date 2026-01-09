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
using GFC.BlazorServer.Services.Controllers;
using MengqiModels = GFC.BlazorServer.Connectors.Mengqi.Models;

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
        ICommunicationLogService? logService = null,
        IControllerTransport? transport = null,
        ILogger<MengqiControllerClient>? logger = null,
        ILoggerFactory? loggerFactory = null)
    {
        _ = options ?? throw new ArgumentNullException(nameof(options));
        endpointResolver = endpointResolver ?? throw new ArgumentNullException(nameof(endpointResolver));

        _commands = options.CommandProfiles ?? throw new ArgumentNullException(nameof(options.CommandProfiles));
        _transport = transport ?? new UdpControllerTransport();
        _dispatcher = new WgCommandDispatcher(options, endpointResolver, _transport, logService, loggerFactory?.CreateLogger<WgCommandDispatcher>());
        _logger = logger ?? loggerFactory?.CreateLogger<MengqiControllerClient>();
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

    public async Task AddOrUpdateCardAsync(uint controllerSn, MengqiModels.CardPrivilegeModel model, CancellationToken cancellationToken = default)
    {
        var payload = WgPayloadFactory.BuildPrivilegePayload(_commands.AddOrUpdateCard, model, markAsDeleted: false);
        await SendAndExpectAck(controllerSn, _commands.AddOrUpdateCard, payload, cancellationToken).ConfigureAwait(false);
    }

    public async Task SetDoorConfigAsync(uint controllerSn, int doorIndex, byte controlMode, byte relayDelay, byte doorSensor, byte interlock, CancellationToken cancellationToken = default)
    {
        var payload = WgPayloadFactory.BuildDoorConfigPayload(_commands.SetDoorConfig, doorIndex, controlMode, relayDelay, doorSensor, interlock);
        await SendAndExpectAck(controllerSn, _commands.SetDoorConfig, payload, cancellationToken).ConfigureAwait(false);
    }

    public async Task<DiscoveryResult?> GetHardwareInfoAsync(uint controllerSn, CancellationToken cancellationToken = default)
    {
        // 1. Basic Discovery (0x94) - Gets generic Modes/Delays (may be summary data)
        var response = await _dispatcher.SendAsync(controllerSn, _commands.Search, Array.Empty<byte>(), cancellationToken).ConfigureAwait(false);
        if (response.Length < 64) return null;
        
        DiscoveryResult? info = null;
        try 
        {
            info = WgResponseParser.ParseDiscovery(response);
        }
        catch (Exception ex)
        {
            _logger?.LogDebug(ex, "Failed to parse discovery response for {Sn}", controllerSn);
            return null;
        }

            // 2. Fetch Detailed Door Params (0x5A) for each door to be precise
        // Request: 17 5A ... [DoorIndex] ...
        // Response: [ControlMode] [Delay] [Interlock] ...
        
        try
        {
            // Start with what we found in discovery (Search 0x94) as a baseline
            var collectedModes = info.DoorModes ?? new byte[4];
            var collectedDelays = info.DoorDelays ?? new byte[4];
            var collectedInterlocks = new byte[4];
            var collectedAjars = new byte[4];
            var collectedVerifications = new byte[4];
            
            for (int i = 1; i <= 4; i++)
            {
                try
                {
                    var paramsPayload = WgPayloadFactory.BuildGetDoorParamsPayload(_commands.GetDoorParams, i);
                    var paramsResp = await _dispatcher.SendAsync(controllerSn, _commands.GetDoorParams, paramsPayload, cancellationToken).ConfigureAwait(false);
                    
                    // If we get a valid ACK/Response
                    if (paramsResp.Length >= 64)
                    {
                        var hex = BitConverter.ToString(paramsResp.ToArray());
                        _logger?.LogInformation("Door {I} Raw 0x5A Response: {Hex}", i, hex);

                        var doorConfig = WgResponseParser.ParseDoorParams(paramsResp, i);
                        
                        // Unconditionally trust the 0x5A response as it is a specific hardware query.
                        collectedModes[i - 1] = (byte)doorConfig.ControlMode;
                        collectedDelays[i - 1] = doorConfig.UnlockDuration;
                        collectedInterlocks[i - 1] = (byte)doorConfig.Interlock;
                        collectedAjars[i - 1] = doorConfig.DoorAjarTimeout;
                        collectedVerifications[i - 1] = (byte)doorConfig.Verification;
                        
                        _logger?.LogDebug("Door {I} 0x5A Params: Mode={M}, Delay={D}", i, doorConfig.ControlMode, doorConfig.UnlockDuration);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning("Door {I} 0x5A fetch failed: {Message}", i, ex.Message);
                    // Continue to next door
                }
            }
            
            // Update info with merged data
            info = new DiscoveryResult
            {
                SerialNumber = info.SerialNumber,
                IpAddress = info.IpAddress,
                SubnetMask = info.SubnetMask,
                Gateway = info.Gateway,
                MacAddress = info.MacAddress,
                FirmwareVersion = info.FirmwareVersion,
                ControllerDate = info.ControllerDate,
                Port = info.Port,
                AllowedPcIp = info.AllowedPcIp,
                DoorModes = collectedModes,
                DoorDelays = collectedDelays,
                Interlocks = collectedInterlocks,
                DoorAjarTimeouts = collectedAjars,
                Verifications = collectedVerifications
            };
        }
        catch (Exception ex)
        {
             _logger?.LogWarning("Detailed door param fetch (0x5A) failed or partial: {Message}. Falling back to discovery summary.", ex.Message);
             // Return 'info' as-is from initial discovery if detailed fetch fails
        }
        
        return info;
    }

    public async Task DeleteCardAsync(uint controllerSn, long cardNumber, CancellationToken cancellationToken = default)
    {
        var model = new MengqiModels.CardPrivilegeModel
        {
            CardNumber = cardNumber,
            DoorList = Array.Empty<int>()
        };
        var payload = WgPayloadFactory.BuildPrivilegePayload(_commands.DeleteCard, model, markAsDeleted: true);
        await SendAndExpectAck(controllerSn, _commands.DeleteCard, payload, cancellationToken).ConfigureAwait(false);
    }

    public async Task BulkUploadCardsAsync(uint controllerSn, IEnumerable<MengqiModels.CardPrivilegeModel> cards, CancellationToken cancellationToken = default)
    {
        foreach (var card in cards)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await AddOrUpdateCardAsync(controllerSn, card, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task ClearAllCardsAsync(uint controllerSn, CancellationToken cancellationToken = default)
    {
        var payload = WgPayloadFactory.BuildClearAllCardsPayload(_commands.ClearAllCards);
        await SendAndExpectAck(controllerSn, _commands.ClearAllCards, payload, cancellationToken).ConfigureAwait(false);
    }

    public async Task<(IReadOnlyList<ControllerEvent> Events, uint ControllerLastIndex)> GetNewEventsAsync(
        uint controllerSn,
        uint eventIndex,
        CancellationToken cancellationToken = default)
    {
        // Controller expects index at packet offset 20.
        // Data starts at offset 8, so we need 12 bytes padding + 4 bytes index = 16 bytes total.
        var payload = new byte[16];
        BitConverter.GetBytes(eventIndex).CopyTo(payload, 12);
        
        var response = await _dispatcher.SendAsync(controllerSn, _commands.GetEvents, payload, cancellationToken).ConfigureAwait(false);
        WgResponseParser.EnsureAck(response, _commands.GetEvents);
        return WgResponseParser.ParseEvents(response, eventIndex);
    }
    
    public async Task AcknowledgeEventsAsync(uint controllerSn, uint eventsReadIndex, CancellationToken cancellationToken = default)
    {
        // User Requirement: "Occasionally send the 0xB2 (Acknowledgment) command with your LastReadIndex"
        var payload = new byte[4];
        BitConverter.GetBytes(eventsReadIndex).CopyTo(payload, 0);
        await SendAndExpectAck(controllerSn, _commands.AckEvents, payload, cancellationToken).ConfigureAwait(false);
    }

    public async Task<MengqiModels.RunStatusModel> GetRunStatusAsync(uint controllerSn, CancellationToken cancellationToken = default)
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
    
    public async Task ResetControllerAsync(uint controllerSn, int doorCount = 4, IEnumerable<DoorHardwareConfig>? doorConfigs = null, MengqiModels.CardPrivilegeModel? primaryCard = null, CancellationToken cancellationToken = default)
    {
        const uint WildcardSn = 0x00118000; // 00 80 11 00 (Little Endian)
        
        // 1. Triple-Handshake Sync (Broadcast Unlock Sequence)
        try
        {
            _logger?.LogInformation("Starting Triple-Handshake Sync for {Sn} using Wildcard ID...", controllerSn);

            // Step 0: Broadcast Search (0x24) - Wake up Maintenance Listener (Fire-and-Forget)
            // We iterate manually to fire one packet without waiting for all responses
            await _dispatcher.BroadcastAsync(_commands.Search, Array.Empty<byte>(), cancellationToken)
                             .GetAsyncEnumerator(cancellationToken)
                             .MoveNextAsync(); 
            _logger?.LogInformation("Sent Wake-up Broadcast (0x24)");
            await Task.Delay(100, cancellationToken); // Minimal gap

            // Step 1: Rescue Clear (0x54) via Wildcard SN (Fire-and-Forget)
            // Use BroadcastAsync logic with Wildcard SN payload to ensure "maintenance" mode
            // We use SendAsync with WildcardSn which Dispatcher routes to Broadcast IP
            var clearPayload = Array.Empty<byte>();
            // Using SendAsync but NOT awaiting the result in a blocking way if possible, 
            // but w/o modifying Dispatcher, we must await. 
            // However, Dispatcher using Broadcast IP for WildcardSn returns immediately or after timeout?
            // User requirement: "Switch to Wildcard SN... Broadcast Routing"
            // The dispatcher change I made routes WildcardSn to Broadcast IP.
            // To be fast, we use a short timeout or just assume success? 
            // Sending broadcast UDP is usually fast.
            try { await _dispatcher.SendAsync(WildcardSn, _commands.ClearAllCards, clearPayload, cancellationToken); } catch { /* Ignore missing ACK for broadcast */ }
            _logger?.LogInformation("Sent Unlock Clear (0x54) to Wildcard SN");
            
            await Task.Delay(100, cancellationToken); // Tight loop requirement

            // Step 2: Rescue Time Sync (0x30) via Wildcard SN (Fire-and-Forget)
            var timePayload = WgPayloadFactory.BuildSyncTimePayload(_commands.SyncTime, DateTime.Now);
            try { await _dispatcher.SendAsync(WildcardSn, _commands.SyncTime, timePayload, cancellationToken); } catch { /* Ignore missing ACK for broadcast */ }
            _logger?.LogInformation("Sent Unlock Time Sync (0x30) to Wildcard SN");
             
             await Task.Delay(500, cancellationToken); // Allow commit before verify
        }
        catch (Exception ex)
        {
             _logger?.LogWarning(ex, "Broadcast/Unlock sequence failed. Falling back to Unicast...");
             // Fallback to standard unicast logic if broadcast fails
             await ClearAllCardsAsync(controllerSn, cancellationToken).ConfigureAwait(false);
             await Task.Delay(300, cancellationToken);
             await SyncTimeAsync(controllerSn, DateTime.Now, cancellationToken).ConfigureAwait(false);
        }

        // 4. Verification & Re-Add (Standard Unicast)
        try
        {
            await Task.Delay(200, cancellationToken); // Settle buffer before unicast verify
            
            // Verify Clock (0x20) - Ensure year matches 2025 using REAL SN
            var status = await GetRunStatusAsync(controllerSn, cancellationToken).ConfigureAwait(false);
            if (!status.ControllerTime.HasValue || status.ControllerTime.Value.Year < 2025)
            {
                var currentYear = status.ControllerTime?.Year.ToString() ?? "Unknown";
                var errorMsg = $"[Hardware Clock Lock] Controller {controllerSn} rejected time sync. Year is still {currentYear}. Unlock sequence failed.";
                _logger?.LogError(errorMsg);
                throw new InvalidOperationException(errorMsg);
            }
            _logger?.LogInformation("Verification Successful: Year is {Year}", status.ControllerTime.Value.Year);

        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Time sync failed/verified for controller {Sn}. Reset process potentially compromised.", controllerSn);
            throw; // Critical for deep reset
        }
        
        // 3 & 4. Initialize Doors (0x8E) - Set Mode 3 for active doors, Mode 0 for unused doors to wipe Ghost Data
        var configList = doorConfigs?.ToList() ?? new List<DoorHardwareConfig>();

        for (int i = 1; i <= 4; i++)
        {
            if (i <= doorCount)
            {
                // Active Door (Step 3) - Enable physical doors
                var config = configList.FirstOrDefault(c => c.DoorIndex == i);
                
                byte mode = config != null ? (byte)config.ControlMode : (byte)0x03;
                byte delay = config?.UnlockDuration ?? 0x05;
                byte sensor = config?.SensorType ?? 0x00; 
                byte interlock = config != null ? (byte)config.Interlock : (byte)0x00;

                await SetDoorConfigAsync(controllerSn, i, mode, delay, sensor, interlock, cancellationToken);
                _logger?.LogInformation("Configured Door {DoorIndex} (Mode={Mode}, Delay={Delay}s) for controller {Sn}", i, mode, delay, controllerSn);
            }
            else
            {
                // Unused Door (Step 4) - Wipe Ghost Data (Force Mode 0)
                await SetDoorConfigAsync(controllerSn, i, 0x00, 0x00, 0, 0, cancellationToken);
                _logger?.LogInformation("Disabled Door {DoorIndex} (Mode 0 - Wipe Ghost Data) for controller {Sn}", i, controllerSn);
            }
            await Task.Delay(200, cancellationToken);
        }

        // 5. Re-Add Member Card (0x50) - Re-authorize the card with the corrected clock
        if (primaryCard != null)
        {
            try
            {
                await AddOrUpdateCardAsync(controllerSn, primaryCard, cancellationToken).ConfigureAwait(false);
                _logger?.LogInformation("Re-added primary member card {CardNumber} to controller {Sn}", primaryCard.CardNumber, controllerSn);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Failed to re-add primary card for {Sn}", controllerSn);
            }
        }
        
        _logger?.LogInformation("Deep Reset complete for controller {Sn}", controllerSn);
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


