using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using GFC.BlazorServer.Auth;
using GFC.BlazorServer.ProtocolCapture.Models;
using GFC.BlazorServer.ProtocolCapture.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace GFC.BlazorServer.Pages.ProtocolCapture;

[Authorize(Policy = AppPolicies.RequireAdmin)]
public class IndexModel : PageModel
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    private readonly CaptureService _captureService;

    public IndexModel(CaptureService captureService)
    {
        _captureService = captureService;
    }

    [BindProperty]
    public string ControllerIp { get; set; } = string.Empty;

    [BindProperty]
    public int Port { get; set; } = 60000;

    [BindProperty]
    public string InterfaceName { get; set; } = string.Empty;

    [BindProperty]
    public string? PacketsJson { get; set; }

    public List<PacketInfo> Packets { get; private set; } = new();

    [BindProperty]
    public int? SelectedPacketId { get; set; }

    [BindProperty]
    public string? SelectedPacketHex { get; set; } = string.Empty;

    [BindProperty]
    public string GeneratedMethodName { get; set; } = "SendCustomPacket";

    [BindProperty]
    public string? GeneratedCSharp { get; set; } = string.Empty;

    public string? StatusMessage { get; private set; }
    public string? ErrorMessage { get; private set; }

    public void OnGet()
    {
        LoadPackets();
    }

    public IActionResult OnPostStartCapture()
    {
        LoadPackets();

        if (!ValidateCaptureInputs())
        {
            PersistState();
            return Page();
        }

        try
        {
            _captureService.StartCapture(InterfaceName, ControllerIp, Port);
            StatusMessage = "Capture started. Perform the controller action you want to observe.";
            GeneratedCSharp = null;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }

        PersistState();
        return Page();
    }

    public IActionResult OnPostStopAnalyze()
    {
        LoadPackets();

        try
        {
            _captureService.StopCapture();
            Packets = _captureService.ParseCapturedPackets();
            SelectedPacketId = null;
            SelectedPacketHex = null;
            GeneratedCSharp = null;

            StatusMessage = Packets.Count == 0
                ? "Capture stopped, but no packets were parsed. Try again."
                : $"Capture stopped. Parsed {Packets.Count} packet(s).";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }

        PersistState();
        return Page();
    }

    public IActionResult OnPostClear()
    {
        LoadPackets();

        Packets.Clear();
        PacketsJson = string.Empty;
        SelectedPacketId = null;
        SelectedPacketHex = null;
        GeneratedCSharp = null;
        StatusMessage = "Cleared capture results.";

        PersistState();
        return Page();
    }

    public IActionResult OnPostSelectPacket(int packetId)
    {
        LoadPackets();

        var packet = Packets.FirstOrDefault(p => p.Id == packetId);
        if (packet == null)
        {
            ErrorMessage = "Packet not found. Re-run Stop & Analyze if necessary.";
            SelectedPacketId = null;
            SelectedPacketHex = null;
        }
        else
        {
            SelectedPacketId = packet.Id;
            SelectedPacketHex = packet.FullHexDump;
            GeneratedCSharp = null;
            StatusMessage = $"Packet {packet.Id} selected.";
        }

        PersistState();
        return Page();
    }

    public IActionResult OnPostGenerateCSharp()
    {
        LoadPackets();

        if (SelectedPacketId is null)
        {
            ErrorMessage = "Select a packet before generating code.";
            PersistState();
            return Page();
        }

        var packet = Packets.FirstOrDefault(p => p.Id == SelectedPacketId);
        if (packet == null)
        {
            ErrorMessage = "Selected packet no longer exists. Stop & Analyze again.";
            PersistState();
            return Page();
        }

        var methodName = SanitizeMethodName(GeneratedMethodName);
        var byteLiterals = string.Join(", ", packet.Payload.Select(b => $"0x{b:X2}"));

        var builder = new StringBuilder();
        builder.AppendLine($"public void {methodName}(string controllerIp, int port = 60000)");
        builder.AppendLine("{");
        builder.AppendLine($"    var payload = new byte[] {{ {byteLiterals} }};");
        builder.AppendLine("    using var client = new UdpClient();");
        builder.AppendLine("    client.Connect(controllerIp, port);");
        builder.AppendLine("    client.Send(payload, payload.Length);");
        builder.AppendLine("}");

        GeneratedCSharp = builder.ToString();
        StatusMessage = $"Generated method {methodName}.";

        PersistState();
        return Page();
    }

    private void LoadPackets()
    {
        if (string.IsNullOrWhiteSpace(PacketsJson))
        {
            Packets = new List<PacketInfo>();
            return;
        }

        try
        {
            Packets = JsonSerializer.Deserialize<List<PacketInfo>>(PacketsJson, JsonOptions)
                      ?? new List<PacketInfo>();
        }
        catch
        {
            Packets = new List<PacketInfo>();
            ErrorMessage = "Failed to read saved packets. Clear results and analyze again.";
        }
    }

    private void PersistState()
    {
        PacketsJson = Packets.Count == 0
            ? string.Empty
            : JsonSerializer.Serialize(Packets, JsonOptions);
    }

    private bool ValidateCaptureInputs()
    {
        if (string.IsNullOrWhiteSpace(ControllerIp))
        {
            ErrorMessage = "Controller IP is required.";
            return false;
        }

        if (Port <= 0 || Port > 65535)
        {
            ErrorMessage = "Port must be between 1 and 65535.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(InterfaceName))
        {
            ErrorMessage = "Interface name is required.";
            return false;
        }

        return true;
    }

    private static string SanitizeMethodName(string? input)
    {
        var fallback = "SendCustomPacket";
        if (string.IsNullOrWhiteSpace(input))
        {
            return fallback;
        }

        var cleaned = Regex.Replace(input, @"[^A-Za-z0-9_]", string.Empty);
        return string.IsNullOrWhiteSpace(cleaned) ? fallback : cleaned;
    }
}

