using System;
using System.Threading;
using System.Threading.Tasks;
using Gfc.ControllerClient.Configuration;

namespace Gfc.ControllerClient.Transport;

public interface IControllerTransport : IDisposable
{
    Task<byte[]> SendAsync(
        ControllerEndpoint endpoint,
        ReadOnlyMemory<byte> request,
        TimeSpan timeout,
        CancellationToken cancellationToken);
}
