using GFC.Pos.UI.Services;

namespace GFC.Pos.Mobile.Services;

public class DummyPrinterService : IPrinterService
{
    public Task<bool> PrintReceiptAsync(string content) => Task.FromResult(false);
    public Task<bool> PrintRawDataAsync(byte[] data, System.Threading.CancellationToken cancellationToken = default) => Task.FromResult(false);
    public Task<bool> KickDrawerAsync() => Task.FromResult(false);
    public Task<List<UsbDeviceDto>> GetConnectedDevicesAsync() => Task.FromResult(new List<UsbDeviceDto>());
}
