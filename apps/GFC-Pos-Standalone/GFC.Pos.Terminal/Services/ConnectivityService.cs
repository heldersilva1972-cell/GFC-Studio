using Microsoft.JSInterop;

namespace GFC.Pos.Terminal.Services;

/// <summary>
/// Detects real-time online/offline state via browser events.
/// Register as Singleton so all components share one instance.
/// </summary>
public class ConnectivityService : IAsyncDisposable
{
    private readonly IJSRuntime _js;
    private DotNetObjectReference<ConnectivityService>? _selfRef;

    public bool IsOnline { get; private set; } = true;

    /// <summary>Fires whenever connectivity changes. Parameter is the new IsOnline value.</summary>
    public event Action<bool>? ConnectivityChanged;

    public ConnectivityService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task InitializeAsync()
    {
        try
        {
            _selfRef = DotNetObjectReference.Create(this);
            IsOnline = await _js.InvokeAsync<bool>("GfcConnectivity.isOnline");
            await _js.InvokeVoidAsync("GfcConnectivity.initialize", _selfRef);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Connectivity] Init failed: {ex.Message}");
        }
    }

    [JSInvokable]
    public void OnConnectivityChanged(bool isOnline)
    {
        if (IsOnline == isOnline) return;
        IsOnline = isOnline;
        Console.WriteLine($"[Connectivity] → {(isOnline ? "ONLINE" : "OFFLINE")}");
        ConnectivityChanged?.Invoke(isOnline);
    }

    public async ValueTask DisposeAsync()
    {
        try { await _js.InvokeVoidAsync("GfcConnectivity.dispose"); } catch { }
        _selfRef?.Dispose();
    }
}
