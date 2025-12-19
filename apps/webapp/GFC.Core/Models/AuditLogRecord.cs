using System;

namespace GFC.Core.Models;

/// <summary>
/// Read-only projection of an audit log row with friendly display names.
/// </summary>
public class AuditLogRecord
{
    public int AuditLogId { get; set; }

    public DateTime TimestampUtc { get; set; }

    public int? PerformedByUserId { get; set; }

    public string PerformedByDisplayName { get; set; } = "System";

    public int? TargetUserId { get; set; }

    public string TargetDisplayName { get; set; } = "--";

    public string Action { get; set; } = string.Empty;

    public string? Details { get; set; }
}
