using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using GFC.BlazorServer.Connectors.Mengqi.Configuration;

namespace GFC.BlazorServer.Connectors.Mengqi.Transport;

internal sealed class UdpControllerTransport : IControllerTransport, IDisposable
{
    private readonly Socket _socket;

    public UdpControllerTransport()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
        {
            Blocking = false,
            EnableBroadcast = true,
            ReceiveBufferSize = 16777216 // 16MB - matches vendor software (wgUdpComm.cs line 23, 57, 81, 293)
        };
        
        // Disable ICMP port unreachable errors (matches vendor SockSpecialSet - wgUdpComm.cs lines 269-282)
        // This prevents the socket from throwing exceptions when the controller is offline
        try
        {
            uint IOC_IN = 0x80000000;
            uint IOC_VENDOR = 0x18000000;
            uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
            _socket.IOControl((int)SIO_UDP_CONNRESET, new byte[] { 0 }, null);
        }
        catch
        {
            // Ignore if IOControl fails - not critical
        }
        
        _socket.Bind(new IPEndPoint(IPAddress.Any, 0));
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
        EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        
        try
        {
            var receiveResult = await _socket.ReceiveFromAsync(
                new Memory<byte>(buffer),
                SocketFlags.None,
                remoteEndPoint,
                cts.Token).ConfigureAwait(false);

            return buffer.AsSpan(0, receiveResult.ReceivedBytes).ToArray();
        }
        catch (OperationCanceledException)
        {
            throw new TimeoutException($"No response from {target} within {timeout.TotalSeconds}s");
        }
    }

    public async IAsyncEnumerable<byte[]> BroadcastAsync(
        ControllerEndpoint endpoint,
        ReadOnlyMemory<byte> request,
        TimeSpan timeout,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var target = new IPEndPoint(endpoint.Address, endpoint.UdpPort);
        await _socket.SendToAsync(request, SocketFlags.None, target, cancellationToken).ConfigureAwait(false);

        var stopAt = DateTime.UtcNow.Add(timeout);
        var buffer = new byte[4096];

        while (DateTime.UtcNow < stopAt && !cancellationToken.IsCancellationRequested)
        {
            if (_socket.Available == 0)
            {
                await Task.Delay(50, cancellationToken).ConfigureAwait(false);
                continue;
            }

            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            var result = await _socket.ReceiveFromAsync(new ArraySegment<byte>(buffer), SocketFlags.None, remoteEndPoint).ConfigureAwait(false);
            if (result.ReceivedBytes > 0)
            {
                yield return buffer.AsSpan(0, result.ReceivedBytes).ToArray();
            }
        }
    }

    public void Dispose()
    {
        _socket.Dispose();
    }
}


