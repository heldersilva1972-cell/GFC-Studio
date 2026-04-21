namespace GFC.Core.DTOs;

/// <summary>
/// Represents backup timing metadata for dashboards.
/// </summary>
public record BackupStatusDto(DateTime? LastBackupUtc, DateTime? NextScheduledUtc);

