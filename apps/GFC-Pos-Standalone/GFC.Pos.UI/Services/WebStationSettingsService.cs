using Microsoft.JSInterop;

namespace GFC.Pos.UI.Services;

public class WebStationSettingsService : IStationSettingsService
{
    private readonly IJSRuntime _js;
    private const string Key = "GfcPos_TerminalName";
    private const string CfdKey = "GfcPos_CustomerDisplayEnabled";
    private const string CfdDurationKey = "GfcPos_CustomerDisplayDurationSeconds";
    private const string CfdAnnouncementsKey = "GfcPos_CustomerAnnouncementsEnabled";
    private const string CfdAnnouncementIntervalKey = "GfcPos_CustomerAnnouncementIntervalSeconds";

    public WebStationSettingsService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<string> GetTerminalNameAsync()
    {
        return await _js.InvokeAsync<string>("localStorage.getItem", Key) ?? "TERMINAL 1";
    }

    public async Task SaveTerminalNameAsync(string name)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", Key, name);
    }

    public async Task<bool> GetCustomerDisplayEnabledAsync()
    {
        var val = await _js.InvokeAsync<string>("localStorage.getItem", CfdKey);
        return bool.TryParse(val, out var enabled) && enabled;
    }

    public async Task SaveCustomerDisplayEnabledAsync(bool enabled)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", CfdKey, enabled.ToString().ToLower());
    }

    public async Task<int> GetCustomerDisplayDurationSecondsAsync()
    {
        var val = await _js.InvokeAsync<string>("localStorage.getItem", CfdDurationKey);
        return int.TryParse(val, out var secs) && secs > 0 ? secs : 20;
    }

    public async Task SaveCustomerDisplayDurationSecondsAsync(int seconds)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", CfdDurationKey, seconds.ToString());
    }

    public async Task<bool> GetCustomerAnnouncementsEnabledAsync()
    {
        var val = await _js.InvokeAsync<string>("localStorage.getItem", CfdAnnouncementsKey);
        return bool.TryParse(val, out var enabled) ? enabled : true; // Default enabled
    }

    public async Task SaveCustomerAnnouncementsEnabledAsync(bool enabled)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", CfdAnnouncementsKey, enabled.ToString().ToLower());
    }

    public async Task<int> GetCustomerAnnouncementIntervalSecondsAsync()
    {
        var val = await _js.InvokeAsync<string>("localStorage.getItem", CfdAnnouncementIntervalKey);
        return int.TryParse(val, out var secs) && secs > 0 ? secs : 8; // Default 8s
    }

    public async Task SaveCustomerAnnouncementIntervalSecondsAsync(int seconds)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", CfdAnnouncementIntervalKey, seconds.ToString());
    }
}
