using Android.App;
using Android.Content;
using Android.Hardware.Usb;
using System.Text;

namespace GFC.Pos.Mobile.Platforms.Android.Services;

public class UsbPrinterService
{
    private readonly UsbManager _usbManager;
    private const string ActionUsbPermission = "com.gfc.pos.USB_PERMISSION";
    private const int EpsonVendorId = 1208; // 0x04B8

    public UsbPrinterService()
    {
        _usbManager = (UsbManager)Platform.CurrentActivity.GetSystemService(Context.UsbService);
    }

    public async Task<bool> PrintReceiptAsync(string content)
    {
        var device = FindPrinter();
        if (device == null) return false;

        if (!_usbManager.HasPermission(device))
        {
            RequestPermission(device);
            return false; // User needs to grant permission first
        }

        return SendRawData(device, Encoding.ASCII.GetBytes(content));
    }

    public bool KickDrawer()
    {
        var device = FindPrinter();
        if (device == null || !_usbManager.HasPermission(device)) return false;

        // ESC/POS Drawer Kick command: 1B 70 00 19 FA
        byte[] kickCommand = new byte[] { 0x1B, 0x70, 0x00, 0x19, 0xFA };
        return SendRawData(device, kickCommand);
    }

    private UsbDevice FindPrinter()
    {
        foreach (var device in _usbManager.DeviceList.Values)
        {
            if (device.VendorId == EpsonVendorId) return device;
        }
        return null;
    }

    private void RequestPermission(UsbDevice device)
    {
        var intent = new Intent(ActionUsbPermission);
        var pendingIntent = PendingIntent.GetBroadcast(Platform.CurrentActivity, 0, intent, PendingIntentFlags.Immutable);
        _usbManager.RequestPermission(device, pendingIntent);
    }

    private bool SendRawData(UsbDevice device, byte[] data)
    {
        UsbInterface usbInterface = device.GetInterface(0);
        UsbEndpoint endpoint = null;

        for (int i = 0; i < usbInterface.EndpointCount; i++)
        {
            var ep = usbInterface.GetEndpoint(i);
            if (ep.Type == UsbAddressing.Bulk && ep.Direction == UsbAddressing.Out)
            {
                endpoint = ep;
                break;
            }
        }

        if (endpoint == null) return false;

        using (UsbDeviceConnection connection = _usbManager.OpenDevice(device))
        {
            if (connection == null) return false;

            // CRITICAL: Android requires claiming the interface before transfer
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
