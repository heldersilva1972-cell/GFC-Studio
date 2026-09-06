using System;
using System.Collections.Generic;

namespace GFC.Core.DTOs;

public class PosDiagnosticEventDto
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Category { get; set; } = "General"; // Network, Printer, Sync, Hardware, Lifecycle
    public string Severity { get; set; } = "Info";    // Info, Warning, Error
    public string Message { get; set; } = string.Empty;
}

public class PosTelemetryDto
{
    public string TerminalName { get; set; } = string.Empty;
    public string AppVersion { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public bool IsOnline { get; set; } = true;
    public string CircuitState { get; set; } = "Closed"; // Closed (Healthy), Open (Offline), HalfOpen (Testing)
    public int PendingVaultCount { get; set; } = 0;
    public string PrinterStatus { get; set; } = "Ready"; // Ready, Offline, Error, NotConfigured
    public string? PrinterEndpoint { get; set; }
    public string? ApiBaseAddress { get; set; }
    public double? BatteryLevel { get; set; } // e.g. 0.95 (95%)
    public bool? IsCharging { get; set; }
    public string? CurrentUser { get; set; }
    public List<PosDiagnosticEventDto> DiagnosticEvents { get; set; } = new();
}

public class PosTelemetryResponseDto
{
    public bool Success { get; set; } = true;
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public bool ResetRetriesRequested { get; set; } = false;
    public bool ForceSyncRequested { get; set; } = false;
}
