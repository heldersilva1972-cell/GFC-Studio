using System;
using System.Threading;
using System.Threading.Tasks;
using Gfc.ControllerClient.Configuration;

namespace Gfc.ControllerClient.Transport;

internal interface IControllerTransport : IDisposable
{
    Task<byte[]> SendAsync(
        ControllerEndpoint endpoint,
        ReadOnlyMemory<byte> request,
        TimeSpan timeout,
        CancellationToken cancellationToken);
}
