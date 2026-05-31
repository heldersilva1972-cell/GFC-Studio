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

    static AndroidPrinterService()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public AndroidPrinterService(IPrinterConfigService configService)
    {
        _configService = configService;
        _usbManager = (UsbManager)Platform.CurrentActivity.GetSystemService(Context.UsbService);
    }

    /// <summary>
    /// Receives pre-formatted 42-char plain-text from the receipt builder.
    /// Wraps it in ESC/POS init + cut commands and sends via USB bulk transfer.
    /// </summary>
    public async Task<bool> PrintReceiptAsync(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            System.Diagnostics.Debug.WriteLine("[AndroidPrinter] PrintReceiptAsync aborted: Content is null or empty.");
            return false;
        }

        try
        {
            var payload = BuildEscPosPayload(content);
            return await PrintRawDataAsync(payload);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AndroidPrinter] PrintReceiptAsync failed: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> PrintRawDataAsync(byte[] data, global::System.Threading.CancellationToken cancellationToken = default)
    {
        if (data == null || data.Length == 0)
        {
            System.Diagnostics.Debug.WriteLine("[AndroidPrinter] PrintRawDataAsync aborted: Data buffer is empty.");
            return false;
        }

        var (vid, pid) = _configService.GetParsedSettings();
        if (vid == null || pid == null)
        {
            System.Diagnostics.Debug.WriteLine("[AndroidPrinter] Ready-state failed: VID or PID not configured.");
            return false;
        }

        var device = FindPrinter(vid.Value, pid.Value);
        if (device == null)
        {
            System.Diagnostics.Debug.WriteLine($"[AndroidPrinter] Printer VID {vid:X4} PID {pid:X4} not found on USB bus.");
            return false;
        }

        System.Diagnostics.Debug.WriteLine($"[AndroidPrinter] USB device VID {vid:X4} PID {pid:X4} found. Checking permission...");

        if (!_usbManager.HasPermission(device))
        {
            System.Diagnostics.Debug.WriteLine("[AndroidPrinter] USB permission not granted — requesting...");
            var granted = await RequestPermissionAsync(device);
            if (!granted)
            {
                System.Diagnostics.Debug.WriteLine("[AndroidPrinter] USB permission denied.");
                return false;
            }
            System.Diagnostics.Debug.WriteLine("[AndroidPrinter] USB permission granted.");
        }

        System.Diagnostics.Debug.WriteLine("[AndroidPrinter] Initiating USB bulk transfer...");
        return SendRawData(device, data);
    }

    public async Task<bool> PrintTestAsync()
    {
        // NOTE: Ethernet routing is handled by MauiPrinterService before reaching this class.
        // This method only handles local USB printing.
        try
        {
            var sb = new StringBuilder();
            ReceiptFormatter.AppendDivider(sb, '=');
            ReceiptFormatter.AppendCenter(sb, "GFC POS TEST PRINT");
            ReceiptFormatter.AppendDivider(sb, '=');
            ReceiptFormatter.AppendBlank(sb);
            ReceiptFormatter.AppendLine(sb, "STATUS", "ONLINE (USB)");
            ReceiptFormatter.AppendBlank(sb);
            ReceiptFormatter.AppendCenter(sb, "PRINTER READY");
            ReceiptFormatter.AppendDivider(sb, '-');

            var payload = BuildEscPosPayload(sb.ToString());
            return await PrintRawDataAsync(payload);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AndroidPrinter] PrintTest failed: {ex.Message}");
            return false;
        }
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

    // ── Private helpers ──────────────────────────────────────────────────────

    private byte[] BuildEscPosPayload(string plainText)
    {
        var cp437 = Encoding.GetEncoding(437);
        var bytes = new List<byte>();

        // ESC @ — Initialise printer (reset all settings)
        bytes.Add(0x1B); bytes.Add(0x40);

        // GS L nL nH — Set left margin to 0
        bytes.AddRange(new byte[] { 0x1D, 0x4C, 0x00, 0x00 });

        // Plain-text body — pre-formatted, line endings are \n
        bytes.AddRange(cp437.GetBytes(plainText));

        // 4× line feeds before cut so text clears the cutter blade
        bytes.AddRange(new byte[] { 0x0A, 0x0A, 0x0A, 0x0A });

        // GS V B 0 — Full cut (feed & cut)
        bytes.AddRange(new byte[] { 0x1D, 0x56, 0x42, 0x00 });

        return bytes.ToArray();
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
