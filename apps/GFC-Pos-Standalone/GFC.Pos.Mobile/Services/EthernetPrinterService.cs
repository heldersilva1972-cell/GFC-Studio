using GFC.Pos.UI.Services;
using System.Net.Sockets;
using System.Text;

namespace GFC.Pos.Mobile.Services;

public class EthernetPrinterService : IPrinterService
{
    private readonly IPrinterConfigService _config;

    static EthernetPrinterService()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public EthernetPrinterService(IPrinterConfigService config)
    {
        _config = config;
    }

    /// <summary>
    /// Receives pre-formatted 42-char plain-text from the receipt builder.
    /// Wraps it in ESC/POS init + cut commands and sends over TCP.
    /// </summary>
    public async Task<bool> PrintReceiptAsync(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            System.Diagnostics.Debug.WriteLine("[EthernetPrinter] PrintReceiptAsync aborted: Content is null or empty.");
            return false;
        }

        try
        {
            var payload = BuildEscPosPayload(content);
            return await SendBytesAsync(payload);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EthernetPrinter] PrintReceiptAsync failed: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> PrintRawDataAsync(byte[] data, CancellationToken cancellationToken = default)
    {
        if (data == null || data.Length == 0)
        {
            System.Diagnostics.Debug.WriteLine("[EthernetPrinter] PrintRawDataAsync aborted: Data buffer is empty.");
            return false;
        }

        var printerIp = _config.PrinterIpAddress;
        System.Diagnostics.Debug.WriteLine($"[EthernetPrinter] Transmitting {data.Length} bytes to {printerIp}:9100...");

        try
        {
            using var client = new TcpClient();
            client.SendTimeout = 5000;
            client.ReceiveTimeout = 5000;

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(3000);

            await client.ConnectAsync(printerIp, 9100, cts.Token);

            using var stream = client.GetStream();
            await stream.WriteAsync(data, 0, data.Length, cancellationToken);
            await stream.FlushAsync(cancellationToken);
            await Task.Delay(500, cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"[EthernetPrinter] Transmission failed: {ex.Message}");
            sb.AppendLine($"Stack: {ex.StackTrace}");
            var inner = ex.InnerException;
            int depth = 1;
            while (inner != null)
            {
                sb.AppendLine($"  Inner {depth}: {inner.Message}");
                inner = inner.InnerException;
                depth++;
            }
            var msg = sb.ToString();
            Console.WriteLine(msg);
            System.Diagnostics.Debug.WriteLine(msg);
            return false;
        }
    }

    public async Task<bool> PrintTestAsync()
    {
        try
        {
            var sb = new StringBuilder();
            ReceiptFormatter.AppendDivider(sb, '=');
            ReceiptFormatter.AppendCenter(sb, "GFC POS TEST PRINT");
            ReceiptFormatter.AppendDivider(sb, '=');
            ReceiptFormatter.AppendBlank(sb);
            ReceiptFormatter.AppendLine(sb, "STATUS", "ONLINE (ETHERNET)");
            ReceiptFormatter.AppendLine(sb, "IP", _config.PrinterIpAddress);
            ReceiptFormatter.AppendBlank(sb);
            ReceiptFormatter.AppendCenter(sb, "PRINTER READY");
            ReceiptFormatter.AppendDivider(sb, '-');

            var payload = BuildEscPosPayload(sb.ToString());
            return await SendBytesAsync(payload);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EthernetPrinter] PrintTest failed: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> KickDrawerAsync()
    {
        byte[] kickCommand = new byte[] { 0x1B, 0x70, 0x00, 0x19, 0xFA };
        return await PrintRawDataAsync(kickCommand);
    }

    public Task<List<UsbDeviceDto>> GetConnectedDevicesAsync()
        => Task.FromResult(new List<UsbDeviceDto>());

    // ── Private helpers ──────────────────────────────────────────────────────

    private byte[] BuildEscPosPayload(string plainText)
    {
        var cp437 = Encoding.GetEncoding(437);
        var bytes = new List<byte>();

        // ESC @ — Initialise printer (reset all settings)
        bytes.Add(0x1B); bytes.Add(0x40);

        // GS L nL nH — Set left margin to 0
        bytes.AddRange(new byte[] { 0x1D, 0x4C, 0x00, 0x00 });

        // Plain-text body — pre-formatted, line endings already \n
        bytes.AddRange(cp437.GetBytes(plainText));

        // 4× line feeds before cut so text clears the cutter blade
        bytes.AddRange(new byte[] { 0x0A, 0x0A, 0x0A, 0x0A });

        // GS V B 0 — Full cut (feed & cut)
        bytes.AddRange(new byte[] { 0x1D, 0x56, 0x42, 0x00 });

        return bytes.ToArray();
    }

    private async Task<bool> SendBytesAsync(byte[] data)
    {
        var printerIp = _config.PrinterIpAddress;
        System.Diagnostics.Debug.WriteLine($"[EthernetPrinter] Sending {data.Length} bytes to {printerIp}:9100...");

        try
        {
            using var client = new TcpClient();
            client.SendTimeout = 5000;
            client.ReceiveTimeout = 5000;

            using var cts = new CancellationTokenSource(3000);
            await client.ConnectAsync(printerIp, 9100, cts.Token);

            using var stream = client.GetStream();
            await stream.WriteAsync(data, 0, data.Length, cts.Token);
            await stream.FlushAsync(cts.Token);

            // Hold the socket open briefly so the printer's TCP buffer is fully consumed
            await Task.Delay(500, cts.Token);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EthernetPrinter] Send failed ({printerIp}:9100): {ex.Message}");
            return false;
        }
    }
}
