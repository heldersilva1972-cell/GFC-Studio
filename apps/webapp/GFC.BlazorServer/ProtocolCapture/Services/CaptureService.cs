using System.Diagnostics;
using System.IO;
using GFC.BlazorServer.ProtocolCapture.Helpers;
using GFC.BlazorServer.ProtocolCapture.Models;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.ProtocolCapture.Services;

/// <summary>
/// Launches/stops tshark.exe and parses capture files.
/// Delete this file (and the ProtocolCapture folder) to remove the feature.
/// </summary>
public class CaptureService
{
    // Update this path if tshark.exe is installed somewhere else on the Agent PC.
    private const string TsharkExecutablePath = @"C:\Program Files\Wireshark\tshark.exe";

    private readonly ILogger<CaptureService> _logger;
    private Process? _tsharkProcess;
    private string? _captureFilePath;

    public CaptureService(ILogger<CaptureService> logger)
    {
        _logger = logger;
    }

    public void StartCapture(string interfaceName, string controllerIp, int port)
    {
        if (string.IsNullOrWhiteSpace(interfaceName))
        {
            throw new ArgumentException("Interface name is required.", nameof(interfaceName));
        }

        if (string.IsNullOrWhiteSpace(controllerIp))
        {
            throw new ArgumentException("Controller IP is required.", nameof(controllerIp));
        }

        EnsureTsharkExists();

        if (_tsharkProcess is { HasExited: false })
        {
            throw new InvalidOperationException("Capture already running. Stop it before starting another.");
        }

        var captureFolder = Path.Combine(Path.GetTempPath(), "ProtocolCapture");
        Directory.CreateDirectory(captureFolder);
        _captureFilePath = Path.Combine(captureFolder, $"capture_{DateTime.UtcNow:yyyyMMdd_HHmmss}.pcapng");

        var arguments = $"-i \"{interfaceName}\" host {controllerIp} and udp port {port} -w \"{_captureFilePath}\"";

        var startInfo = new ProcessStartInfo
        {
            FileName = TsharkExecutablePath,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden
        };

        _logger.LogInformation("Starting tshark capture: {Args}", arguments);

        _tsharkProcess = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Failed to launch tshark.exe. Check the path and permissions.");
    }

    public void StopCapture()
    {
        if (_tsharkProcess == null)
        {
            return;
        }

        try
        {
            if (!_tsharkProcess.HasExited)
            {
                _tsharkProcess.Kill(entireProcessTree: true);
                _tsharkProcess.WaitForExit(TimeSpan.FromSeconds(2));
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to stop tshark.exe cleanly.");
        }
        finally
        {
            _tsharkProcess.Dispose();
            _tsharkProcess = null;
        }
    }

    public List<PacketInfo> ParseCapturedPackets()
    {
        EnsureTsharkExists();

        if (string.IsNullOrWhiteSpace(_captureFilePath) || !File.Exists(_captureFilePath))
        {
            throw new InvalidOperationException("No capture file found. Start a capture before analyzing.");
        }

        var arguments = $"-r \"{_captureFilePath}\" -x";

        var startInfo = new ProcessStartInfo
        {
            FileName = TsharkExecutablePath,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden
        };

        _logger.LogInformation("Parsing capture file {File}", _captureFilePath);

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Failed to launch tshark.exe for parsing.");

        var stdout = process.StandardOutput.ReadToEnd();
        var stderr = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"tshark parsing failed: {stderr}");
        }

        var packetPayloads = HexUtils.ParseTsharkHexDump(stdout);

        var results = new List<PacketInfo>();
        var index = 1;
        foreach (var payload in packetPayloads)
        {
            var packet = new PacketInfo
            {
                Id = index++,
                Length = payload.Length,
                Payload = payload,
                Direction = "Unknown",
                HexPreview = HexUtils.ToHexPreview(payload, 32),
                FullHexDump = HexUtils.ToFormattedHexDump(payload)
            };
            results.Add(packet);
        }

        return results;
    }

    private static void EnsureTsharkExists()
    {
        if (!File.Exists(TsharkExecutablePath))
        {
            throw new FileNotFoundException(
                $"tshark.exe not found at '{TsharkExecutablePath}'. Update TsharkExecutablePath in CaptureService.cs.",
                TsharkExecutablePath);
        }
    }
}

