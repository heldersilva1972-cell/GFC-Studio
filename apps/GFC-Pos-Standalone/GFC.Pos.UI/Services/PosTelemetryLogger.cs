using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GFC.Core.DTOs;

namespace GFC.Pos.UI.Services;

public static class PosTelemetryLogger
{
    private static readonly ConcurrentQueue<PosDiagnosticEventDto> _eventQueue = new();
    private const int MaxBufferedEvents = 30;

    public static void Log(string category, string severity, string message)
    {
        try
        {
            var evt = new PosDiagnosticEventDto
            {
                Timestamp = DateTime.UtcNow,
                Category = category,
                Severity = severity,
                Message = message
            };

            _eventQueue.Enqueue(evt);

            while (_eventQueue.Count > MaxBufferedEvents && _eventQueue.TryDequeue(out _))
            {
                // Circular buffer limit
            }
        }
        catch { }
    }

    public static List<PosDiagnosticEventDto> FlushEvents()
    {
        var list = new List<PosDiagnosticEventDto>();
        while (_eventQueue.TryDequeue(out var item))
        {
            list.Add(item);
        }
        return list;
    }

    public static IReadOnlyList<PosDiagnosticEventDto> PeekRecentEvents()
    {
        return _eventQueue.ToList();
    }
}
