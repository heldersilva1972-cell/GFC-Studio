namespace GFC.Pos.UI.Services;

public interface IPrinterService
{
    Task<bool> PrintReceiptAsync(string content);
    Task<bool> PrintRawDataAsync(byte[] data);
    Task<bool> KickDrawerAsync();
    Task<List<UsbDeviceDto>> GetConnectedDevicesAsync();
}

public class UsbDeviceDto
{
    public string Name { get; set; } = "";
    public int VendorId { get; set; }
    public int ProductId { get; set; }
    public string VidHex => VendorId.ToString("X4");
    public string PidHex => ProductId.ToString("X4");
}
