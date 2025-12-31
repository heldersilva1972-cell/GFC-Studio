using System;
using System.Collections.Generic;
using System.Buffers;

namespace GFC.BlazorServer.Services.Controllers;

public class CommunicationLogEntry
{
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public uint ControllerSn { get; set; }
    public string Operation { get; set; } = string.Empty;
    public byte[]? RequestPacket { get; set; }
    public byte[]? ResponsePacket { get; set; }
    public string Description { get; set; } = string.Empty;
    public string PlainEnglish { get; set; } = string.Empty;
    public bool IsError { get; set; }
    public string? ErrorMessage { get; set; }
}

public interface ICommunicationLogService
{
    event Action<CommunicationLogEntry> OnLog;
    void Log(CommunicationLogEntry entry);
    IReadOnlyList<CommunicationLogEntry> GetRecentLogs(int count = 50);
}

public class CommunicationLogService : ICommunicationLogService
{
    private readonly List<CommunicationLogEntry> _logs = new();
    private readonly int _maxLogs = 100;
    private readonly object _lock = new();

    public event Action<CommunicationLogEntry>? OnLog;

    public void Log(CommunicationLogEntry entry)
    {
        lock (_lock)
        {
            _logs.Add(entry);
            if (_logs.Count > _maxLogs)
            {
                _logs.RemoveAt(0);
            }
        }
        OnLog?.Invoke(entry);
    }

    public IReadOnlyList<CommunicationLogEntry> GetRecentLogs(int count = 50)
    {
        lock (_lock)
        {
            return _logs.AsReadOnly();
        }
    }
}
