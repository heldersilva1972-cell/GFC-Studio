using System;
using System.Threading;
using System.Threading.Tasks;
using Gfc.ControllerClient.Abstractions;
using Gfc.ControllerClient.Configuration;
using Gfc.ControllerClient.Interop;
using Gfc.ControllerClient.Packets;
using Gfc.ControllerClient.Transport;
using Gfc.ControllerClient.Utilities;

namespace Gfc.ControllerClient.Services;

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
        if (!string.IsNullOrWhiteSpace(endpoint.CommPassword))
        {
            var payloadSpan = span[WgPacketBuilder.HeaderLength..];
            N3kCommCryptoProvider.Encrypt(payloadSpan, endpoint.CommPassword!);
            WgPacketBuilder.WriteCrc(span);
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
                        if (responseSpan.Length > WgPacketBuilder.HeaderLength)
                        {
                            N3kCommCryptoProvider.Decrypt(responseSpan[WgPacketBuilder.HeaderLength..], endpoint.CommPassword!);
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

    private ushort NextXid() => unchecked(++_xid);
}

