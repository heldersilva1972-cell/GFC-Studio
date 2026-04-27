using GFC.Pos.UI.Services;
using Microsoft.Maui.Storage;
using System.Globalization;

namespace GFC.Pos.Mobile.Services;

public class PrinterConfigService : IPrinterConfigService
{
    private const string VidKey = "printer_vid";
    private const string PidKey = "printer_pid";

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

    public void SaveSettings(string vid, string pid)
    {
        PrinterVendorId = vid;
        PrinterProductId = pid;
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
