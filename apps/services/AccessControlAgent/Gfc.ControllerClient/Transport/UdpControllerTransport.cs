using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Gfc.ControllerClient.Configuration;

namespace Gfc.ControllerClient.Transport;

internal sealed class UdpControllerTransport : IControllerTransport, IDisposable
{
    private readonly Socket _socket;

    public UdpControllerTransport()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
        {
            Blocking = false
        };
    }

    public async Task<byte[]> SendAsync(
        ControllerEndpoint endpoint,
        ReadOnlyMemory<byte> request,
        TimeSpan timeout,
        CancellationToken cancellationToken)
    {
        var target = new IPEndPoint(endpoint.Address, endpoint.UdpPort);
        await _socket.SendToAsync(request, SocketFlags.None, target, cancellationToken).ConfigureAwait(false);

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(timeout);

        var buffer = new byte[4096];
        var receiveResult = await _socket.ReceiveFromAsync(
            new ArraySegment<byte>(buffer),
            SocketFlags.None,
            target,
            cts.Token).ConfigureAwait(false);

        return buffer.AsSpan(0, receiveResult.ReceivedBytes).ToArray();
    }

    public void Dispose()
    {
        _socket.Dispose();
    }
}

