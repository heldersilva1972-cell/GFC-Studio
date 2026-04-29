using GFC.Pos.UI.Services;
using Microsoft.Maui.Storage;
using System.Globalization;

namespace GFC.Pos.Mobile.Services;

public class PrinterConfigService : IPrinterConfigService
{
    private const string TypeKey = "printer_type";
    private const string IpKey = "printer_ip";
    private const string VidKey = "printer_vid";
    private const string PidKey = "printer_pid";

    public PrinterType CurrentPrinterType
    {
        get => (PrinterType)Preferences.Default.Get(TypeKey, (int)PrinterType.USB);
        set => Preferences.Default.Set(TypeKey, (int)value);
    }

    public string PrinterVendorId 
    { 
        get => Preferences.Default.Get(VidKey, "04B8"); // Default to Epson
        set => Preferences.Default.Set(VidKey, value);
    }

    public string PrinterProductId 
    { 
        get => Preferences.Default.Get(PidKey, "0202"); // Default common TM-T20
        set => Preferences.Default.Set(PidKey, value);
    }

    public string PrinterIpAddress
    {
        get => Preferences.Default.Get(IpKey, "192.168.1.100");
        set => Preferences.Default.Set(IpKey, value);
    }

    public void SaveSettings(string vid, string pid, string ip, PrinterType type)
    {
        PrinterVendorId = vid;
        PrinterProductId = pid;
        PrinterIpAddress = ip;
        CurrentPrinterType = type;
    }

    public Task LoadSettingsAsync() => Task.CompletedTask;

    public (int? vid, int? pid) GetParsedSettings()
    {
        int? vid = null;
        int? pid = null;

        if (int.TryParse(PrinterVendorId, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int v))
            vid = v;
        else if (int.TryParse(PrinterVendorId, out int v2))
            vid = v2;

        if (int.TryParse(PrinterProductId, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int p))
            pid = p;
        else if (int.TryParse(PrinterProductId, out int p2))
            pid = p2;

        return (vid, pid);
    }
}
