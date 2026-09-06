using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GFC.Core.DTOs;

namespace GFC.BlazorServer.Services;

public class PosDiagnosticEventEntry
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string TerminalName { get; set; } = string.Empty;
    public string Category { get; set; } = "General";
    public string Severity { get; set; } = "Info";
    public string Message { get; set; } = string.Empty;
}

public class PosTelemetryStateService
{
    private readonly ConcurrentDictionary<string, PosTelemetryDto> _latestSnapshots = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentQueue<PosDiagnosticEventEntry> _eventHistory = new();
    private readonly ConcurrentDictionary<string, bool> _pendingResetRetries = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, bool> _pendingForceSync = new(StringComparer.OrdinalIgnoreCase);
    private const int MaxHistoryEvents = 200;

    public void RequestResetRetries(string terminalName)
    {
        if (string.IsNullOrWhiteSpace(terminalName)) return;
        _pendingResetRetries[terminalName.Trim()] = true;
        RecordEvent(terminalName.Trim(), "Command", "Info", "Remote retry reset requested from Web App");
    }

    public void RequestForceSync(string terminalName)
    {
        if (string.IsNullOrWhiteSpace(terminalName)) return;
        _pendingForceSync[terminalName.Trim()] = true;
        RecordEvent(terminalName.Trim(), "Command", "Info", "Remote force sync requested from Web App");
    }

    public bool ConsumeResetRetries(string terminalName)
    {
        if (string.IsNullOrWhiteSpace(terminalName)) return false;
        return _pendingResetRetries.TryRemove(terminalName.Trim(), out _);
    }

    public bool ConsumeForceSync(string terminalName)
    {
        if (string.IsNullOrWhiteSpace(terminalName)) return false;
        return _pendingForceSync.TryRemove(terminalName.Trim(), out _);
    }

    public void UpdateTelemetry(PosTelemetryDto telemetry)
    {
        if (telemetry == null || string.IsNullOrWhiteSpace(telemetry.TerminalName)) return;

        var termKey = telemetry.TerminalName.Trim();
        _latestSnapshots[termKey] = telemetry;

        // Record any events contained in payload
        if (telemetry.DiagnosticEvents != null && telemetry.DiagnosticEvents.Any())
        {
            foreach (var ev in telemetry.DiagnosticEvents)
            {
                RecordEvent(termKey, ev.Category, ev.Severity, ev.Message, ev.Timestamp);
            }
        }
    }

    public void RecordEvent(string terminalName, string category, string severity, string message, DateTime? timestamp = null)
    {
        var entry = new PosDiagnosticEventEntry
        {
            Timestamp = timestamp ?? DateTime.UtcNow,
            TerminalName = terminalName,
            Category = category,
            Severity = severity,
            Message = message
        };

        _eventHistory.Enqueue(entry);

        while (_eventHistory.Count > MaxHistoryEvents && _eventHistory.TryDequeue(out _))
        {
            // Keep within bounded history
        }
    }

    public PosTelemetryDto? GetLatestTelemetry(string terminalName)
    {
        if (string.IsNullOrWhiteSpace(terminalName)) return null;
        var clean = terminalName.Trim();
        if (_latestSnapshots.TryGetValue(clean, out var val)) return val;

        var match = _latestSnapshots.FirstOrDefault(kvp => kvp.Key.Equals(clean, StringComparison.OrdinalIgnoreCase) ||
                                                           kvp.Key.Contains(clean, StringComparison.OrdinalIgnoreCase) ||
                                                           clean.Contains(kvp.Key, StringComparison.OrdinalIgnoreCase));
        if (match.Value != null) return match.Value;

        if (_latestSnapshots.Count == 1) return _latestSnapshots.Values.First();
        return null;
    }

    public IReadOnlyList<PosTelemetryDto> GetAllLatestTelemetry()
    {
        return _latestSnapshots.Values.ToList();
    }

    public IReadOnlyList<PosDiagnosticEventEntry> GetHistory(string? terminalName = null)
    {
        var list = _eventHistory.OrderByDescending(e => e.Timestamp);
        if (!string.IsNullOrWhiteSpace(terminalName))
        {
            return list.Where(e => e.TerminalName.Equals(terminalName.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
        }
        return list.ToList();
    }

    public void ClearHistory(string? terminalName = null)
    {
        if (string.IsNullOrWhiteSpace(terminalName))
        {
            while (_eventHistory.TryDequeue(out _)) { }
        }
    }
}
