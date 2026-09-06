using System.Collections.Concurrent;

namespace GFC.BlazorServer.Services;

public class PosSyncErrorEntry
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string TerminalName { get; set; } = "Unknown Terminal";
    public string Endpoint { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public string? PayloadSummary { get; set; }
}

public class PosSyncLogService
{
    private readonly ConcurrentQueue<PosSyncErrorEntry> _errors = new();
    private const int MaxEntries = 30;

    public void RecordError(string terminalName, string endpoint, string errorMessage, string? payloadSummary = null)
    {
        var entry = new PosSyncErrorEntry
        {
            Timestamp = DateTime.UtcNow,
            TerminalName = string.IsNullOrWhiteSpace(terminalName) ? "POS Terminal" : terminalName,
            Endpoint = endpoint,
            ErrorMessage = errorMessage,
            PayloadSummary = payloadSummary
        };

        _errors.Enqueue(entry);

        while (_errors.Count > MaxEntries && _errors.TryDequeue(out _))
        {
            // Keep queue within MaxEntries
        }
    }

    public IReadOnlyList<PosSyncErrorEntry> GetRecentErrors()
    {
        return _errors.OrderByDescending(e => e.Timestamp).ToList();
    }

    public void ClearErrors()
    {
        while (_errors.TryDequeue(out _)) { }
    }
}
