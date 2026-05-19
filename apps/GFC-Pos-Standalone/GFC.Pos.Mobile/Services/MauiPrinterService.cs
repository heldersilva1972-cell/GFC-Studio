using GFC.Pos.UI.Services;
#if ANDROID
using GFC.Pos.Mobile.Platforms.Android.Services;
#endif

namespace GFC.Pos.Mobile.Services;

public class MauiPrinterService : IPrinterService
{
    private readonly IPrinterService _implementation;

    public MauiPrinterService(IPrinterConfigService configService)
    {
        if (configService.CurrentPrinterType == PrinterType.Ethernet)
        {
            _implementation = new EthernetPrinterService(configService);
        }
        else
        {
#if ANDROID
            _implementation = new AndroidPrinterService(configService);
#else
            _implementation = new DummyPrinterService();
#endif
        }
    }

    public Task<bool> PrintReceiptAsync(string content) => _implementation.PrintReceiptAsync(content);
    public Task<bool> PrintRawDataAsync(byte[] data) => _implementation.PrintRawDataAsync(data);
    public Task<bool> KickDrawerAsync() => _implementation.KickDrawerAsync();
    public Task<List<UsbDeviceDto>> GetConnectedDevicesAsync() => _implementation.GetConnectedDevicesAsync();
}
