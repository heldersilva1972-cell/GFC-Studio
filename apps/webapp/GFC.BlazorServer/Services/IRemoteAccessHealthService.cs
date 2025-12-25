// [NEW]
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class RemoteAccessHealthStatus
    {
        public bool IsTunnelHealthy { get; set; }
        public bool IsVpnHealthy { get; set; }
    }

    public interface IRemoteAccessHealthService
    {
        Task<RemoteAccessHealthStatus> GetHealthStatusAsync();
    }
}
