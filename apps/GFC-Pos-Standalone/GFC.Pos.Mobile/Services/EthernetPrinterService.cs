using GFC.Pos.UI.Services;
using System.Net.Sockets;
using System.Text;

namespace GFC.Pos.Mobile.Services;

public class EthernetPrinterService : IPrinterService
{
    private readonly IPrinterConfigService _config;

    public EthernetPrinterService(IPrinterConfigService config)
    {
        _config = config;
    }

    public async Task<bool> PrintReceiptAsync(string content)
    {
        return await PrintRawDataAsync(Encoding.ASCII.GetBytes(content));
    }

    public async Task<bool> PrintRawDataAsync(byte[] data)
    {
        try
        {
            using var client = new TcpClient();
            var connectTask = client.ConnectAsync(_config.PrinterIpAddress, 9100);
            
            // Timeout after 3 seconds
            if (await Task.WhenAny(connectTask, Task.Delay(3000)) != connectTask)
            {
                return false;
            }

            using var stream = client.GetStream();
            await stream.WriteAsync(data, 0, data.Length);
            await stream.FlushAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EthernetPrinter] Print failed: {ex.Message}");
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
        // Not applicable for Ethernet
        return Task.FromResult(new List<UsbDeviceDto>());
    }
}
