using GFC.Pos.UI.Services;
using Microsoft.JSInterop;
using System.Globalization;

namespace GFC.Pos.Terminal.Services;

public class WebPrinterConfigService : IPrinterConfigService
{
    private readonly IJSRuntime _js;
    private const string VidKey = "printer_vid";
    private const string PidKey = "printer_pid";
    private const string TypeKey = "printer_type";
    private const string IpKey = "printer_ip";

    private string _vid = "04B8";
    private string _pid = "0202";
    private string _ip = "192.168.1.100";
    private PrinterType _type = PrinterType.USB;

    public WebPrinterConfigService(IJSRuntime js)
    {
        _js = js;
    }

    public PrinterType CurrentPrinterType { get => _type; set => _type = value; }
    public string PrinterVendorId { get => _vid; set => _vid = value; }
    public string PrinterProductId { get => _pid; set => _pid = value; }
    public string PrinterIpAddress { get => _ip; set => _ip = value; }

    public void SaveSettings(string vid, string pid, string ip, PrinterType type)
    {
        _vid = vid;
        _pid = pid;
        _ip = ip;
        _type = type;

        _js.InvokeVoidAsync("localStorage.setItem", VidKey, vid);
        _js.InvokeVoidAsync("localStorage.setItem", PidKey, pid);
        _js.InvokeVoidAsync("localStorage.setItem", IpKey, ip);
        _js.InvokeVoidAsync("localStorage.setItem", TypeKey, ((int)type).ToString());
    }

    public async Task LoadSettingsAsync()
    {
        var vid = await _js.InvokeAsync<string>("localStorage.getItem", VidKey);
        var pid = await _js.InvokeAsync<string>("localStorage.getItem", PidKey);
        var ip = await _js.InvokeAsync<string>("localStorage.getItem", IpKey);
        var type = await _js.InvokeAsync<string>("localStorage.getItem", TypeKey);

        if (!string.IsNullOrEmpty(vid)) _vid = vid;
        if (!string.IsNullOrEmpty(pid)) _pid = pid;
        if (!string.IsNullOrEmpty(ip)) _ip = ip;
        if (!string.IsNullOrEmpty(type) && int.TryParse(type, out int t)) _type = (PrinterType)t;
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
