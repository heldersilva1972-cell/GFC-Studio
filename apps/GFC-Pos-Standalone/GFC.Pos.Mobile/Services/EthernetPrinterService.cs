using GFC.Pos.UI.Services;
using System.Net.Sockets;
using System.Text;

namespace GFC.Pos.Mobile.Services;

public class EthernetPrinterService : IPrinterService
{
    private readonly IPrinterConfigService _config;

    static EthernetPrinterService()
    {
        // Register CodePages encoding provider for CodePage 437 support
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public EthernetPrinterService(IPrinterConfigService config)
    {
        _config = config;
    }

    public async Task<bool> PrintReceiptAsync(string content)
    {
        try
        {
            // Convert HTML to standard plain-text formatting
            var plainText = HtmlToPlainTextConverter.Convert(content);
            
            // Encode the string in CodePage 437 (compatible with ESC/POS for symbols like $)
            var cp437 = Encoding.GetEncoding(437);
            byte[] textBytes = cp437.GetBytes(plainText);

            // Prepend ESC @ (Initialize printer reset) -> Hex: 1B 40
            byte[] initCmd = new byte[] { 0x1B, 0x40 };

            // Append GS V 66 n (Feed and Cut) -> Hex: 1D 56 42 [n]
            // n = 0x1E (30 decimal) feeds the paper 30 units past the printhead before cutting
            byte[] cutCmd = new byte[] { 0x1D, 0x56, 0x42, 0x1E };

            // Assemble into a single atomic byte stream
            byte[] mergedJob = new byte[initCmd.Length + textBytes.Length + cutCmd.Length];
            Buffer.BlockCopy(initCmd, 0, mergedJob, 0, initCmd.Length);
            Buffer.BlockCopy(textBytes, 0, mergedJob, initCmd.Length, textBytes.Length);
            Buffer.BlockCopy(cutCmd, 0, mergedJob, initCmd.Length + textBytes.Length, cutCmd.Length);

            return await PrintRawDataAsync(mergedJob);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EthernetPrinter] Error assembling ESC/POS job: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> PrintRawDataAsync(byte[] data, CancellationToken cancellationToken = default)
    {
        try
        {
            using var client = new TcpClient();
            client.SendTimeout = 5000;
            client.ReceiveTimeout = 5000;

            // Combine caller's cancellationToken with a strict 3-second connection timeout source
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(3000);

            // Open the TCP socket to port 9100 with active cancellation/timeout support
            await client.ConnectAsync(_config.PrinterIpAddress, 9100, cts.Token);

            using var stream = client.GetStream();
            
            // Transmit and flush data safely with the caller's cancellation token
            await stream.WriteAsync(data, 0, data.Length, cancellationToken);
            await stream.FlushAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EthernetPrinter] Direct transmission failed: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> KickDrawerAsync()
    {
        // Standard ESC/POS drawer kick command
        byte[] kickCommand = new byte[] { 0x1B, 0x70, 0x00, 0x19, 0xFA };
        return await PrintRawDataAsync(kickCommand);
    }

    public Task<List<UsbDeviceDto>> GetConnectedDevicesAsync()
    {
        // Not applicable for Ethernet
        return Task.FromResult(new List<UsbDeviceDto>());
    }
}
