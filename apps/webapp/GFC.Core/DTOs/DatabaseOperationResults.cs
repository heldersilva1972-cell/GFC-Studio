namespace GFC.Core.DTOs;

/// <summary>
/// Result of a backup operation.
/// </summary>
public class BackupResult
{
    public bool Success { get; set; }
    
    public string? ErrorMessage { get; set; }
    
    public int? BackupId { get; set; }
    
    public string? FileName { get; set; }
    
    public long FileSizeBytes { get; set; }
    
    public string? FileHash { get; set; }
    
    public int DurationSeconds { get; set; }
}

/// <summary>
/// Result of a migration operation.
/// </summary>
public class MigrationResult
{
    public bool Success { get; set; }
    
    public string? ErrorMessage { get; set; }
    
    public int MigrationsApplied { get; set; }
    
    public List<string> AppliedMigrationIds { get; set; } = new();
    
    public int DurationSeconds { get; set; }
    
    public string? ProgressLog { get; set; }
}

/// <summary>
/// Result of a restore operation.
/// </summary>
public class RestoreResult
{
    public bool Success { get; set; }
    
    public string? ErrorMessage { get; set; }
    
    public int? PreRestoreBackupId { get; set; }
    
    public string? RestoredFromFileName { get; set; }
    
    public int DurationSeconds { get; set; }
    
    public string? ProgressLog { get; set; }
    
    public SanityCheckResult? SanityCheck { get; set; }
}

/// <summary>
/// Result of post-restore sanity checks.
/// </summary>
public class SanityCheckResult
{
    public bool Passed { get; set; }
    
    public List<string> Checks { get; set; } = new();
    
    public List<string> Warnings { get; set; } = new();
    
    public List<string> Errors { get; set; } = new();
}
