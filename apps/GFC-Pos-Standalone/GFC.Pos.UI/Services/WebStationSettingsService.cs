using Microsoft.JSInterop;

namespace GFC.Pos.UI.Services;

public class WebStationSettingsService : IStationSettingsService
{
    private readonly IJSRuntime _js;
    private const string Key = "GfcPos_TerminalName";

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
}
