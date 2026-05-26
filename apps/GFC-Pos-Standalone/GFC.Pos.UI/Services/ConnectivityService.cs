using Microsoft.JSInterop;

namespace GFC.Pos.UI.Services;

public enum CircuitState
{
    Closed,      // Normal operational state
    Open,        // Network is broken, short-circuit immediately
    HalfOpen     // Testing the link
}

/// <summary>
/// Detects real-time online/offline state via browser events.
/// Register as Singleton so all components share one instance.
/// </summary>
public class ConnectivityService : IAsyncDisposable
{
    private readonly IJSRuntime _js;
    private readonly HttpClient _http;
    private DotNetObjectReference<ConnectivityService>? _selfRef;
    private readonly CancellationTokenSource _cts = new();

    private bool _isOnline = true;
    private bool _isHardwareOnline = true;
    private bool _isServerReachable = true;

    private CircuitState _circuitState = CircuitState.Closed;
    private DateTime _lastStateChange = DateTime.MinValue;
    private readonly TimeSpan _openCooldown = TimeSpan.FromSeconds(30);

    public bool IsOnline => _isOnline;
    public bool IsHardwareOnline => _isHardwareOnline;
    public bool IsServerReachable => _isServerReachable;
    public CircuitState CurrentCircuitState => _circuitState;

    public string EnvironmentName => _http.BaseAddress?.ToString().Contains("localhost") == true ? "LOCAL HOST" : "PRODUCTION";

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
                try
                {
                    while (!_cts.Token.IsCancellationRequested) {
                        var prevOnline = _isOnline;
                        var prevHardware = _isHardwareOnline;
                        var prevServer = _isServerReachable;

                        await CheckServerReachableAsync();

                        // Fire if ANY state changed (Diagnostics dots need this)
                        if (prevOnline != _isOnline || prevHardware != _isHardwareOnline || prevServer != _isServerReachable)
                        {
                            ConnectivityChanged?.Invoke(_isOnline);
                        }

                        await Task.Delay(5000, _cts.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Safe exit when task is cancelled
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Connectivity] Background loop exception: {ex.Message}");
                }
            }, _cts.Token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Connectivity] Init failed: {ex.Message}");
        }
    }

    private int _consecutiveFailures = 0;
    private const int MaxConsecutiveFailures = 3;

    public async Task<bool> CheckServerReachableAsync()
    {
        // 1. If Circuit is Open, check if the cool-down has expired to transition to Half-Open
        if (_circuitState == CircuitState.Open)
        {
            if ((DateTime.Now - _lastStateChange) > _openCooldown)
            {
                Console.WriteLine("[CIRCUIT] Open cooldown expired. Transitioning to Half-Open state.");
                _circuitState = CircuitState.HalfOpen;
                _lastStateChange = DateTime.Now;
            }
            else
            {
                // Circuit is Open: Immediately return false locally without network query
                _isServerReachable = false;
                _isOnline = false;
                return false;
            }
        }

        try
        {
            // 2. Instant Hardware Check
            _isHardwareOnline = await _js.InvokeAsync<bool>("GfcConnectivity.isOnline");
            if (!_isHardwareOnline)
            {
                _isServerReachable = false;
                _isOnline = false;
                _consecutiveFailures = MaxConsecutiveFailures; // Trigger offline immediately if hardware is cut
                
                // Immediately trip to Open state if hardware is cut
                if (_circuitState != CircuitState.Open)
                {
                    Console.WriteLine("[CIRCUIT] Hardware connection cut! Tripping Circuit to Open.");
                    _circuitState = CircuitState.Open;
                    _lastStateChange = DateTime.Now;
                }
                return false;
            }

            // 3. Real API Heartbeat (with Relaxed Timeout)
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4));
            var timestamp = DateTime.Now.Ticks;
            var response = await _http.GetAsync($"api/Health?t={timestamp}", cts.Token);
            
            if (response.IsSuccessStatusCode)
            {
                _consecutiveFailures = 0;
                _isServerReachable = true;
                _isOnline = true;

                // If it was Open or HalfOpen, close the circuit
                if (_circuitState != CircuitState.Closed)
                {
                    Console.WriteLine("[CIRCUIT] Server reachable. Closing circuit.");
                    _circuitState = CircuitState.Closed;
                    _lastStateChange = DateTime.Now;
                }
            }
            else
            {
                HandleFailure();
            }
            return _isOnline;
        }
        catch
        {
            HandleFailure();
            return false;
        }
    }

    private void HandleFailure()
    {
        _consecutiveFailures++;
        if (_consecutiveFailures >= MaxConsecutiveFailures)
        {
            _isServerReachable = false;
            _isOnline = false;
            
            if (_circuitState != CircuitState.Open)
            {
                Console.WriteLine($"[CIRCUIT] Server unreachable after {_consecutiveFailures} consecutive failures. Tripping Circuit to Open.");
                _circuitState = CircuitState.Open;
                _lastStateChange = DateTime.Now;
            }
        }
    }

    private DateTime _lastCheckTime = DateTime.MinValue;

    public async Task<bool> GateAsync(string actionName)
    {
        // SPEED OPTIMIZATION: If we just checked within the last 10s, use that result.
        // This prevents every button click from waiting for a 2s heartbeat timeout when offline.
        if ((DateTime.Now - _lastCheckTime).TotalSeconds < 10)
        {
            return _isServerReachable;
        }

        var reachable = await CheckServerReachableAsync();
        _lastCheckTime = DateTime.Now;

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
        try
        {
            _cts.Cancel();
        }
        catch (ObjectDisposedException) { }

        try { await _js.InvokeVoidAsync("GfcConnectivity.dispose"); } catch { }
        _selfRef?.Dispose();
        _cts.Dispose();
    }
}
