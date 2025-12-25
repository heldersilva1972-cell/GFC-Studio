// [NEW]
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class RemoteAccessHealthService : IRemoteAccessHealthService
    {
        public Task<RemoteAccessHealthStatus> GetHealthStatusAsync()
        {
            // In a real implementation, this service would check the status of the
            // Cloudflare Tunnel and the WireGuard VPN server.
            return Task.FromResult(new RemoteAccessHealthStatus
            {
                IsTunnelHealthy = true,
                IsVpnHealthy = true
            });
        }
    }
}
