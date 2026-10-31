using GFC.Pos.UI.Services;
using Microsoft.Maui.Storage;

namespace GFC.Pos.Mobile.Services;

public class MauiStationSettingsService : IStationSettingsService
{
    private const string Key = "GfcPos_TerminalName";
    private const string CfdKey = "GfcPos_CustomerDisplayEnabled";
    private const string CfdDurationKey = "GfcPos_CustomerDisplayDurationSeconds";

    public Task<string> GetTerminalNameAsync()
    {
        // Preferences survive WebView cache clearing
        var name = Preferences.Default.Get(Key, "TERMINAL 1");
        return Task.FromResult(name);
    }

    public Task SaveTerminalNameAsync(string name)
    {
        Preferences.Default.Set(Key, name);
        return Task.CompletedTask;
    }

    public Task<bool> GetCustomerDisplayEnabledAsync()
    {
        var enabled = Preferences.Default.Get(CfdKey, false);
        return Task.FromResult(enabled);
    }

    public Task SaveCustomerDisplayEnabledAsync(bool enabled)
    {
        Preferences.Default.Set(CfdKey, enabled);
        return Task.CompletedTask;
    }

    public Task<int> GetCustomerDisplayDurationSecondsAsync()
    {
        var secs = Preferences.Default.Get(CfdDurationKey, 20);
        return Task.FromResult(secs > 0 ? secs : 20);
    }

    public Task SaveCustomerDisplayDurationSecondsAsync(int seconds)
    {
        Preferences.Default.Set(CfdDurationKey, seconds);
        return Task.CompletedTask;
    }
}
