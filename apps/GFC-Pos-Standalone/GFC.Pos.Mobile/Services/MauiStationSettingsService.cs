using GFC.Pos.UI.Services;
using Microsoft.Maui.Storage;

namespace GFC.Pos.Mobile.Services;

public class MauiStationSettingsService : IStationSettingsService
{
    private const string Key = "GfcPos_TerminalName";
    private const string CfdKey = "GfcPos_CustomerDisplayEnabled";
    private const string CfdDurationKey = "GfcPos_CustomerDisplayDurationSeconds";
    private const string CfdAnnouncementsKey = "GfcPos_CustomerAnnouncementsEnabled";
    private const string CfdAnnouncementIntervalKey = "GfcPos_CustomerAnnouncementIntervalSeconds";

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

    public Task<bool> GetCustomerAnnouncementsEnabledAsync()
    {
        var enabled = Preferences.Default.Get(CfdAnnouncementsKey, true);
        return Task.FromResult(enabled);
    }

    public Task SaveCustomerAnnouncementsEnabledAsync(bool enabled)
    {
        Preferences.Default.Set(CfdAnnouncementsKey, enabled);
        return Task.CompletedTask;
    }

    public Task<int> GetCustomerAnnouncementIntervalSecondsAsync()
    {
        var secs = Preferences.Default.Get(CfdAnnouncementIntervalKey, 8);
        return Task.FromResult(secs > 0 ? secs : 8);
    }

    public Task SaveCustomerAnnouncementIntervalSecondsAsync(int seconds)
    {
        Preferences.Default.Set(CfdAnnouncementIntervalKey, seconds);
        return Task.CompletedTask;
    }
}
