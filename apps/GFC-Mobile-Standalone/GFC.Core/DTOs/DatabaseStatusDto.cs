using System;

namespace GFC.Core.DTOs;

/// <summary>
/// DTO representing the current database status for the maintenance page.
/// </summary>
public class DatabaseStatusDto
{
    /// <summary>
    /// Current environment: "Development", "Staging", "Production"
    /// </summary>
    public string Environment { get; set; } = string.Empty;

    /// <summary>
    /// Server name (may be masked for security).
    /// </summary>
    public string ServerName { get; set; } = string.Empty;

    /// <summary>
    /// Database name.
    /// </summary>
    public string DatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// Current migration applied on the database.
    /// </summary>
    public string? CurrentMigration { get; set; }

    /// <summary>
    /// Latest migration available in the application code.
    /// </summary>
    public string? LatestMigration { get; set; }

    /// <summary>
    /// Number of pending migrations.
    /// </summary>
    public int PendingMigrationsCount { get; set; }

    /// <summary>
    /// Status indicator: "UpToDate", "Behind", "Ahead"
    /// </summary>
    public string MigrationStatus { get; set; } = "Unknown";

    /// <summary>
    /// When the last backup was created.
    /// </summary>
    public DateTime? LastBackupTime { get; set; }

    /// <summary>
    /// Total number of backups retained.
    /// </summary>
    public int BackupCount { get; set; }

    /// <summary>
    /// Total size of all backups in bytes.
    /// </summary>
    public long TotalBackupSizeBytes { get; set; }

    /// <summary>
    /// Whether maintenance mode is currently enabled.
    /// </summary>
    public bool MaintenanceModeEnabled { get; set; }

    /// <summary>
    /// Whether there's a currently running operation.
    /// </summary>
    public bool HasRunningOperation { get; set; }

    /// <summary>
    /// Details of the currently running operation, if any.
    /// </summary>
    public string? CurrentOperationType { get; set; }
}
