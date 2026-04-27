namespace GFC.Pos.UI.Services;

public interface IPrinterConfigService
{
    string PrinterVendorId { get; set; }
    string PrinterProductId { get; set; }
    void SaveSettings(string vid, string pid);
    Task LoadSettingsAsync();
    (int? vid, int? pid) GetParsedSettings();
}
