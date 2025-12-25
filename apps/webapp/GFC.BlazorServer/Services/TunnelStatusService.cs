// [NEW]
using System;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class TunnelStatusService
    {
        public bool IsTunnelConnected { get; private set; }
        public DateTime LastCheckTime { get; private set; }
        public event Func<Task> OnStatusChanged;

        public async Task SetStatusAsync(bool isConnected)
        {
            IsTunnelConnected = isConnected;
            LastCheckTime = DateTime.UtcNow;
            if (OnStatusChanged != null)
            {
                await OnStatusChanged.Invoke();
            }
        }
    }
}
