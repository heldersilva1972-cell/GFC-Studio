using GFC.Pos.UI.Services;
#if ANDROID
using GFC.Pos.Mobile.Platforms.Android.Services;
#endif

namespace GFC.Pos.Mobile.Services;

public class MauiPrinterService : IPrinterService
{
    private readonly IPrinterConfigService _configService;
    private IPrinterService? _ethernetPrinter;
    private IPrinterService? _localPrinter;

    public MauiPrinterService(IPrinterConfigService configService)
    {
        _configService = configService;
    }

    private IPrinterService GetImplementation()
    {
        if (_configService.CurrentPrinterType == PrinterType.Ethernet)
        {
            return _ethernetPrinter ??= new EthernetPrinterService(_configService);
        }
        else
        {
#if ANDROID
            return _localPrinter ??= new AndroidPrinterService(_configService);
#else
            return _localPrinter ??= new DummyPrinterService();
#endif
        }
    }

    public Task<bool> PrintReceiptAsync(string content) => GetImplementation().PrintReceiptAsync(content);
    public Task<bool> PrintRawDataAsync(byte[] data, global::System.Threading.CancellationToken cancellationToken = default) => GetImplementation().PrintRawDataAsync(data, cancellationToken);
    public Task<bool> KickDrawerAsync() => GetImplementation().KickDrawerAsync();
    public Task<List<UsbDeviceDto>> GetConnectedDevicesAsync() => GetImplementation().GetConnectedDevicesAsync();
}
