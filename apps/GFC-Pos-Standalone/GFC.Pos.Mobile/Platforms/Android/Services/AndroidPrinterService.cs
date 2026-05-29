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
    private static TaskCompletionSource<bool>? _permissionTcs;

    public AndroidPrinterService(IPrinterConfigService configService)
    {
        _configService = configService;
        _usbManager = (UsbManager)Platform.CurrentActivity.GetSystemService(Context.UsbService);
    }

    public async Task<bool> PrintReceiptAsync(string content)
    {
        var plainText = HtmlToPlainTextConverter.Convert(content);
        return await PrintRawDataAsync(Encoding.ASCII.GetBytes(plainText));
    }

    public async Task<bool> PrintRawDataAsync(byte[] data, System.Threading.CancellationToken cancellationToken = default)
    {
        var (vid, pid) = _configService.GetParsedSettings();
        if (vid == null || pid == null) return false;

        var device = FindPrinter(vid.Value, pid.Value);
        if (device == null) return false;

        if (!_usbManager.HasPermission(device))
        {
            var granted = await RequestPermissionAsync(device);
            if (!granted) return false;
        }

        return SendRawData(device, data);
    }

    public async Task<bool> KickDrawerAsync()
    {
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

    private UsbDevice? FindPrinter(int vid, int pid)
    {
        foreach (var device in _usbManager.DeviceList.Values)
        {
            if (device.VendorId == vid && device.ProductId == pid) return device;
        }
        return null;
    }

    private async Task<bool> RequestPermissionAsync(UsbDevice device)
    {
        _permissionTcs = new TaskCompletionSource<bool>();
        
        var context = Platform.CurrentActivity;
        var receiver = new UsbPermissionReceiver();
        context.RegisterReceiver(receiver, new IntentFilter(ActionUsbPermission), ReceiverFlags.NotExported);

        var intent = new Intent(ActionUsbPermission);
        var pendingIntent = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.Mutable);
        _usbManager.RequestPermission(device, pendingIntent);

        var result = await _permissionTcs.Task;
        context.UnregisterReceiver(receiver);
        return result;
    }

    [BroadcastReceiver(Enabled = true, Exported = false)]
    private class UsbPermissionReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context? context, Intent? intent)
        {
            if (intent?.Action == ActionUsbPermission)
            {
                var granted = intent.GetBooleanExtra(UsbManager.ExtraPermissionGranted, false);
                _permissionTcs?.TrySetResult(granted);
            }
        }
    }

    private bool SendRawData(UsbDevice device, byte[] data)
    {
        UsbInterface? usbInterface = null;
        UsbEndpoint? endpoint = null;

        for (int i = 0; i < device.InterfaceCount; i++)
        {
            var iface = device.GetInterface(i);
            for (int j = 0; j < iface.EndpointCount; j++)
            {
                var ep = iface.GetEndpoint(j);
                if (ep.Type == UsbAddressing.XferBulk && ep.Direction == UsbAddressing.Out)
                {
                    usbInterface = iface;
                    endpoint = ep;
                    break;
                }
            }
            if (endpoint != null) break;
        }

        if (endpoint == null || usbInterface == null) return false;

        using var connection = _usbManager.OpenDevice(device);
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

        return false;
    }
}
