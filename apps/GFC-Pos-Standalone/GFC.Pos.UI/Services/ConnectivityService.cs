using Microsoft.JSInterop;

namespace GFC.Pos.UI.Services;

/// <summary>
/// Detects real-time online/offline state via browser events.
/// Register as Singleton so all components share one instance.
/// </summary>
public class ConnectivityService : IAsyncDisposable
{
    private readonly IJSRuntime _js;
    private readonly HttpClient _http;
    private DotNetObjectReference<ConnectivityService>? _selfRef;

    private bool _isOnline = true;
    private bool _isHardwareOnline = true;
    private bool _isServerReachable = true;

    public bool IsOnline => _isOnline;
    public bool IsHardwareOnline => _isHardwareOnline;
    public bool IsServerReachable => _isServerReachable;

    public event Action<bool>? ConnectivityChanged;

    public ConnectivityService(IJSRuntime js, HttpClient http)
    {
        _js = js;
        _http = http;
    }

    public async Task InitializeAsync()
    {
        try
        {
            _selfRef = DotNetObjectReference.Create(this);
            await _js.InvokeVoidAsync("GfcConnectivity.initialize", _selfRef);
            
            // [MOBILE PARITY] Pulse every 5 seconds exactly like mobile hub
            _ = Task.Run(async () => {
                while (true) {
                    var prevOnline = _isOnline;
                    var prevHardware = _isHardwareOnline;
                    var prevServer = _isServerReachable;

                    await CheckServerReachableAsync();

                    // Fire if ANY state changed (Diagnostics dots need this)
                    if (prevOnline != _isOnline || prevHardware != _isHardwareOnline || prevServer != _isServerReachable)
                    {
                        ConnectivityChanged?.Invoke(_isOnline);
                    }

                    await Task.Delay(5000);
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Connectivity] Init failed: {ex.Message}");
        }
    }

    public async Task<bool> CheckServerReachableAsync()
    {
        try
        {
            // 1. Instant Hardware Check
            _isHardwareOnline = await _js.InvokeAsync<bool>("GfcConnectivity.isOnline");
            if (!_isHardwareOnline)
            {
                _isServerReachable = false;
                _isOnline = false;
                return false;
            }

            // 2. Real API Heartbeat (with Mobile-spec Cache Buster)
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var timestamp = DateTime.Now.Ticks;
            var response = await _http.GetAsync($"api/Health?t={timestamp}", cts.Token);
            
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

    public async Task<bool> GateAsync(string actionName)
    {
        // Zero 'Failed to fetch' strategy - Mobile Spec
        var reachable = await CheckServerReachableAsync();
        if (!reachable)
        {
            Console.WriteLine($"[CONNECTIVITY GUARD] Blocking '{actionName}' - Offline state detected.");
            return false;
        }
        return true;
    }

    [JSInvokable]
    public void OnConnectivityChanged(bool isOnline)
    {
        // Immediate hardware update from JS, but heartbeat will confirm IsOnline
        _isHardwareOnline = isOnline;
    }

    public async ValueTask DisposeAsync()
    {
        try { await _js.InvokeVoidAsync("GfcConnectivity.dispose"); } catch { }
        _selfRef?.Dispose();
    }
}
