using Android.App;
using Android.Content;
using Android.Hardware.Usb;
using GFC.Pos.UI.Services;
using System.Text;
using Microsoft.Maui.ApplicationModel;

namespace GFC.Pos.Mobile.Platforms.Android.Services;

public class AndroidPrinterService : IPrinterService
{
    private readonly UsbManager _usbManager;
    private readonly IPrinterConfigService _configService;
    private const string ActionUsbPermission = "com.gfc.pos.USB_PERMISSION";

    public AndroidPrinterService(IPrinterConfigService configService)
    {
        _configService = configService;
        _usbManager = (UsbManager)Platform.CurrentActivity.GetSystemService(Context.UsbService);
    }

    public async Task<bool> PrintReceiptAsync(string content)
    {
        return await PrintRawDataAsync(Encoding.ASCII.GetBytes(content));
    }

    public async Task<bool> PrintRawDataAsync(byte[] data)
    {
        var (vid, pid) = _configService.GetParsedSettings();
        if (vid == null || pid == null) return false;

        var device = FindPrinter(vid.Value, pid.Value);
        if (device == null) return false;

        if (!_usbManager.HasPermission(device))
        {
            await RequestPermissionAsync(device);
            return false;
        }

        return SendRawData(device, data);
    }

    public async Task<bool> KickDrawerAsync()
    {
        // ESC/POS Drawer Kick command: 1B 70 00 19 FA
        byte[] kickCommand = new byte[] { 0x1B, 0x70, 0x00, 0x19, 0xFA };
        return await PrintRawDataAsync(kickCommand);
    }

    public Task<List<UsbDeviceDto>> GetConnectedDevicesAsync()
    {
        var devices = new List<UsbDeviceDto>();
        foreach (var device in _usbManager.DeviceList.Values)
        {
            devices.Add(new UsbDeviceDto
            {
                Name = string.IsNullOrEmpty(device.ProductName) ? device.DeviceName : device.ProductName,
                VendorId = device.VendorId,
                ProductId = device.ProductId
            });
        }
        return Task.FromResult(devices);
    }

    private UsbDevice FindPrinter(int vid, int pid)
    {
        foreach (var device in _usbManager.DeviceList.Values)
        {
            if (device.VendorId == vid && device.ProductId == pid) return device;
        }
        return null;
    }

    private Task RequestPermissionAsync(UsbDevice device)
    {
        var tcs = new TaskCompletionSource<bool>();
        var intent = new Intent(ActionUsbPermission);
        var pendingIntent = PendingIntent.GetBroadcast(Platform.CurrentActivity, 0, intent, PendingIntentFlags.Immutable);
        _usbManager.RequestPermission(device, pendingIntent);
        // Note: In a real app, you'd register a BroadcastReceiver to listen for the result.
        // For simplicity here, we trigger the request and return. The next print attempt will check HasPermission again.
        return Task.CompletedTask;
    }

    private bool SendRawData(UsbDevice device, byte[] data)
    {
        UsbInterface usbInterface = null;
        UsbEndpoint endpoint = null;

        // Try to find a suitable interface and endpoint
        for (int i = 0; i < device.InterfaceCount; i++)
        {
            var iface = device.GetInterface(i);
            for (int j = 0; j < iface.EndpointCount; j++)
            {
                var ep = iface.GetEndpoint(j);
                if (ep.Type == UsbAddressing.Bulk && ep.Direction == UsbAddressing.Out)
                {
                    usbInterface = iface;
                    endpoint = ep;
                    break;
                }
            }
            if (endpoint != null) break;
        }

        if (endpoint == null || usbInterface == null) return false;

        using (UsbDeviceConnection connection = _usbManager.OpenDevice(device))
        {
            if (connection == null) return false;

            if (connection.ClaimInterface(usbInterface, true))
            {
                try
                {
                    int result = connection.BulkTransfer(endpoint, data, data.Length, 5000);
                    return result >= 0;
                }
                finally
                {
                    connection.ReleaseInterface(usbInterface);
                }
            }
        }

        return false;
    }
}
