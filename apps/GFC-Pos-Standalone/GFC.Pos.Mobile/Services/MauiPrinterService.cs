using GFC.Pos.UI.Services;
using GFC.Pos.Mobile.Platforms.Android.Services;

namespace GFC.Pos.Mobile.Services;

public class MauiPrinterService : IPrinterService
{
#if ANDROID
    private readonly UsbPrinterService _androidPrinter;

    public MauiPrinterService()
    {
        _androidPrinter = new UsbPrinterService();
    }

    public Task<bool> PrintReceiptAsync(string content)
    {
        return _androidPrinter.PrintReceiptAsync(content);
    }

    public Task<bool> KickDrawerAsync()
    {
        return Task.FromResult(_androidPrinter.KickDrawer());
    }
#else
    public Task<bool> PrintReceiptAsync(string content) => Task.FromResult(false);
    public Task<bool> KickDrawerAsync() => Task.FromResult(false);
#endif
}
