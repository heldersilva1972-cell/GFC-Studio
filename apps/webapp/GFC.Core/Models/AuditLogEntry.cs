using System;

namespace GFC.Core.Models;

public class AuditLogEntry
{
    public int AuditLogId { get; set; }
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    public int? PerformedByUserId { get; set; }
    public AppUser? PerformedBy { get; set; }
    public int? TargetUserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? Details { get; set; }
}
