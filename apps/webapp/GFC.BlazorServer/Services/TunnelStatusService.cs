using System.Diagnostics;

namespace GFC.BlazorServer.Services;

public interface ITunnelStatusService
{
    bool IsTunnelRunning();
    Task SetStatusAsync(bool isRunning);
}

public class TunnelStatusService : ITunnelStatusService
{
    private bool _cachedStatus = false;
    private DateTime _lastCheck = DateTime.MinValue;
    private readonly TimeSpan _cacheTime = TimeSpan.FromSeconds(30);

    public bool IsTunnelRunning()
    {
        // Use cached status if recent
        if (DateTime.UtcNow - _lastCheck < _cacheTime)
        {
            return _cachedStatus;
        }

        try
        {
            var processes = Process.GetProcesses();
            _cachedStatus = processes.Any(p => 
                p.ProcessName.Contains("cloudflared", StringComparison.OrdinalIgnoreCase));
            _lastCheck = DateTime.UtcNow;
            return _cachedStatus;
        }
        catch
        {
            return _cachedStatus; // Return last known status on error
        }
    }

    public Task SetStatusAsync(bool isRunning)
    {
        _cachedStatus = isRunning;
        _lastCheck = DateTime.UtcNow;
        return Task.CompletedTask;
    }
}
