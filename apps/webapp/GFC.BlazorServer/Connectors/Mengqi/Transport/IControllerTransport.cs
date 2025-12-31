using System;
using System.Threading;
using System.Threading.Tasks;
using GFC.BlazorServer.Connectors.Mengqi.Configuration;

namespace GFC.BlazorServer.Connectors.Mengqi.Transport;

public interface IControllerTransport : IDisposable
{
    Task<byte[]> SendAsync(
        ControllerEndpoint endpoint,
        ReadOnlyMemory<byte> request,
        TimeSpan timeout,
        CancellationToken cancellationToken);

    IAsyncEnumerable<byte[]> BroadcastAsync(
        ControllerEndpoint endpoint,
        ReadOnlyMemory<byte> request,
        TimeSpan timeout,
        CancellationToken cancellationToken);
}

