using Microsoft.JSInterop;

namespace GFC.Mobile.Services;

public interface IConnectivityService
{
    bool IsOnline { get; }
    bool IsHardwareOnline { get; }
    bool IsServerReachable { get; }
    event Action<bool>? ConnectivityChanged;
    Task InitializeAsync();
    Task DisposeAsync();
    Task<bool> CanReachableServerAsync();
    Task<bool> GateAsync(string actionName);
}

public class MobileConnectivityService : IConnectivityService
{
    private readonly IJSRuntime _js;
    private readonly HttpClient _http;
    private bool _isOnline = true;
    private bool _isHardwareOnline = true;
    private bool _isServerReachable = true;

    public MobileConnectivityService(IJSRuntime js, HttpClient http)
    {
        _js = js;
        _http = http;
    }

    public bool IsOnline => _isOnline;
    public bool IsHardwareOnline => _isHardwareOnline;
    public bool IsServerReachable => _isServerReachable;

    public async Task<bool> CanReachableServerAsync()
    {
        try
        {
            // 1. [HONEST PROBE] Check actual internet access via JS (bypasses WASM CORS blocks)
            _isHardwareOnline = await _js.InvokeAsync<bool>("GfcConnectivity.checkHonestInternet");

            if (!_isHardwareOnline) 
            {
                _isServerReachable = false;
                _isOnline = false;
                return false;
            }

            // 2. Real API Heartbeat (Server level)
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var timestamp = DateTime.Now.Ticks;
            var response = await _http.GetAsync($"api/health?t={timestamp}", cts.Token);
            
            _isServerReachable = response.IsSuccessStatusCode;
            _isOnline = _isServerReachable;
            return _isOnline;
        }
        catch
        {
            _isServerReachable = false;
            _isOnline = false;
            return false;
        }
    }

    public event Action<bool>? ConnectivityChanged;

    public async Task InitializeAsync()
    {
        _ = Task.Run(async () => {
            while (true) {
                var prevOnline = _isOnline;
                var prevHardware = _isHardwareOnline;
                var prevServer = _isServerReachable;

                await CanReachableServerAsync();

                // Fire event if ANY of the three states changed
                if (prevOnline != _isOnline || prevHardware != _isHardwareOnline || prevServer != _isServerReachable)
                {
                    ConnectivityChanged?.Invoke(_isOnline);
                }

                await Task.Delay(5000); // 5-second pulse
            }
        });
    }

    public Task DisposeAsync() => Task.CompletedTask;

    public async Task<bool> GateAsync(string actionName)
    {
        // Zero 'Failed to fetch' strategy: Never even start the request if offline
        var reachable = await CanReachableServerAsync();
        if (!reachable)
        {
            Console.WriteLine($"[CONNECTIVITY GUARD] Blocking '{actionName}' - Offline state detected.");
            return false;
        }
        return true;
    }
}
