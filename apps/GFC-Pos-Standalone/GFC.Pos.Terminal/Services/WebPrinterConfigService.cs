using GFC.Pos.UI.Services;
using Microsoft.JSInterop;
using System.Globalization;

namespace GFC.Pos.Terminal.Services;

public class WebPrinterConfigService : IPrinterConfigService
{
    private readonly IJSRuntime _js;
    private const string VidKey = "printer_vid";
    private const string PidKey = "printer_pid";

    private string _vid = "04B8";
    private string _pid = "0202";

    public WebPrinterConfigService(IJSRuntime js)
    {
        _js = js;
        // Load initial values if possible (will be async though)
    }

    public string PrinterVendorId { get => _vid; set => _vid = value; }
    public string PrinterProductId { get => _pid; set => _pid = value; }

    public void SaveSettings(string vid, string pid)
    {
        _vid = vid;
        _pid = pid;
        _js.InvokeVoidAsync("localStorage.setItem", VidKey, vid);
        _js.InvokeVoidAsync("localStorage.setItem", PidKey, pid);
    }

    public async Task LoadSettingsAsync()
    {
        var vid = await _js.InvokeAsync<string>("localStorage.getItem", VidKey);
        var pid = await _js.InvokeAsync<string>("localStorage.getItem", PidKey);
        if (!string.IsNullOrEmpty(vid)) _vid = vid;
        if (!string.IsNullOrEmpty(pid)) _pid = pid;
    }

    public (int? vid, int? pid) GetParsedSettings()
    {
        int? vid = null;
        int? pid = null;

        if (int.TryParse(_vid, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int v))
            vid = v;
        else if (int.TryParse(_vid, out int v2))
            vid = v2;

        if (int.TryParse(_pid, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int p))
            pid = p;
        else if (int.TryParse(_pid, out int p2))
            pid = p2;

        return (vid, pid);
    }
}
