using System;
using System.Threading;
using System.Threading.Tasks;
using GFC.BlazorServer.Connectors.Mengqi.Abstractions;
using GFC.BlazorServer.Connectors.Mengqi.Configuration;
using GFC.BlazorServer.Connectors.Mengqi.Interop;
using GFC.BlazorServer.Connectors.Mengqi.Packets;
using GFC.BlazorServer.Connectors.Mengqi.Transport;
using GFC.BlazorServer.Connectors.Mengqi.Utilities;

namespace GFC.BlazorServer.Connectors.Mengqi.Services;

internal sealed class WgCommandDispatcher
{
    private readonly ControllerClientOptions _options;
    private readonly IControllerEndpointResolver _endpointResolver;
    private readonly IControllerTransport _transport;
    private readonly WgPacketBuilder _packetBuilder;
    private readonly AsyncLock _sendLock = new();
    private ushort _xid;

    public WgCommandDispatcher(
        ControllerClientOptions options,
        IControllerEndpointResolver endpointResolver,
        IControllerTransport transport)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _endpointResolver = endpointResolver ?? throw new ArgumentNullException(nameof(endpointResolver));
        _transport = transport ?? throw new ArgumentNullException(nameof(transport));
        _packetBuilder = new WgPacketBuilder(options);
    }

    public async Task<byte[]> SendAsync(uint controllerSn, WgCommandProfile profile, ReadOnlyMemory<byte> payload, CancellationToken cancellationToken)
    {
        var endpoint = _endpointResolver.Resolve(controllerSn);
        var request = _packetBuilder.Build(controllerSn, NextXid(), profile, payload.Span);
        var span = request.AsSpan();

        // In the N3000 64-byte protocol, bytes 0-3 are the core header (Type, Cmd, CRC)
        // Offset 4 onwards is the payload (SN + Data) that should be encrypted
        if (!string.IsNullOrWhiteSpace(endpoint.CommPassword))
        {
            var payloadSpan = span[4..]; 
            N3kCommCryptoProvider.Encrypt(payloadSpan, endpoint.CommPassword!);
            WgPacketBuilder.WriteCrc(span); // Re-calculate CRC over encrypted data
        }
        
        var attempt = 0;
        Exception? lastError = null;
        while (attempt <= _options.MaxRetries)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                using (await _sendLock.LockAsync(cancellationToken).ConfigureAwait(false))
                {
                    var response = await _transport
                        .SendAsync(endpoint, request, _options.ReceiveTimeout, cancellationToken)
                        .ConfigureAwait(false);

                    if (!string.IsNullOrWhiteSpace(endpoint.CommPassword))
                    {
                        var responseSpan = response.AsSpan();
                        if (responseSpan.Length > 4)
                        {
                            N3kCommCryptoProvider.Decrypt(responseSpan[4..], endpoint.CommPassword!);
                        }
                    }

                    return response;
                }
            }
            catch (Exception ex) when (attempt < _options.MaxRetries && !cancellationToken.IsCancellationRequested)
            {
                lastError = ex;
                attempt++;
            }
        }

        throw lastError ?? new TimeoutException($"Controller SN {controllerSn} did not respond after {_options.MaxRetries + 1} attempts.");
    }

    public async IAsyncEnumerable<byte[]> BroadcastAsync(WgCommandProfile profile, ReadOnlyMemory<byte> payload, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        // For broadcast, we use SN 0 and resolve IP broadcast
        var endpoint = _endpointResolver.Resolve(0);
        var request = _packetBuilder.Build(0, NextXid(), profile, payload.Span);
        
        await foreach (var response in _transport.BroadcastAsync(endpoint, request, _options.ReceiveTimeout, cancellationToken).ConfigureAwait(false))
        {
            yield return response;
        }
    }

    private ushort NextXid() => unchecked(++_xid);
}


