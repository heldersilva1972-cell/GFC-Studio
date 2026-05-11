using GFC.Pos.UI.Services;
using Microsoft.Maui.Storage;

namespace GFC.Pos.Mobile.Services;

public class MauiStationSettingsService : IStationSettingsService
{
    private const string Key = "GfcPos_TerminalName";

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
}
