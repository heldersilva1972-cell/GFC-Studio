namespace GFC.Pos.UI.Services;

public enum PrinterType { USB, Ethernet }
public interface IPrinterConfigService
{
    PrinterType CurrentPrinterType { get; set; }
    string PrinterVendorId { get; set; }
    string PrinterProductId { get; set; }
    string PrinterIpAddress { get; set; }
    
    void SaveSettings(string vid, string pid, string ip, PrinterType type);
    Task LoadSettingsAsync();
    (int? vid, int? pid) GetParsedSettings();
}
