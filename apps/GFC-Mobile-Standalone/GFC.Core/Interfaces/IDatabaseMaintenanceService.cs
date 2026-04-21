using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GFC.Core.DTOs;
using GFC.Core.Models;

namespace GFC.Core.Interfaces;

/// <summary>
/// Service for database maintenance operations: backups, restores, and migrations.
/// </summary>
public interface IDatabaseMaintenanceService
{
    // ===== Status =====
    
    /// <summary>
    /// Get current database status including environment, migrations, and backups.
    /// </summary>
    Task<DatabaseStatusDto> GetStatusAsync();
    
    // ===== Backups =====
    
    /// <summary>
    /// Create a new database backup.
    /// </summary>
    /// <param name="userId">User ID creating the backup</param>
    /// <param name="backupType">Type: "Manual", "PreRestore", "Scheduled"</param>
    /// <param name="notes">Optional notes about this backup</param>
    /// <param name="progress">Progress reporter for UI updates</param>
    Task<BackupResult> CreateBackupAsync(int userId, string backupType = "Manual", string? notes = null, IProgress<string>? progress = null);
    
    /// <summary>
    /// List all available backups (non-deleted).
    /// </summary>
    Task<List<BackupInfoDto>> ListBackupsAsync();
    
    /// <summary>
    /// Get a backup file stream for download.
    /// </summary>
    /// <param name="backupId">Backup ID to download</param>
    /// <param name="userId">User requesting the download (for audit)</param>
    Task<Stream> DownloadBackupAsync(int backupId, int userId);
    
    /// <summary>
    /// Delete old backups beyond the retention count.
    /// </summary>
    Task DeleteOldBackupsAsync();
    
    // ===== Migrations =====
    
    /// <summary>
    /// Get list of pending EF Core migrations.
    /// </summary>
    Task<List<PendingMigrationDto>> GetPendingMigrationsAsync();
    
    /// <summary>
    /// Apply all pending migrations to the database.
    /// </summary>
    /// <param name="userId">User applying the migrations</param>
    /// <param name="progress">Progress reporter for UI updates</param>
    Task<MigrationResult> ApplyMigrationsAsync(int userId, IProgress<string>? progress = null);
    
    // ===== Restore =====
    
    /// <summary>
    /// Restore database from an existing backup record.
    /// </summary>
    /// <param name="userId">User performing the restore</param>
    /// <param name="backupId">Backup ID to restore from</param>
    /// <param name="progress">Progress reporter for UI updates</param>
    Task<RestoreResult> RestoreFromBackupAsync(int userId, int backupId, IProgress<string>? progress = null);
    
    /// <summary>
    /// Restore database from an uploaded backup file.
    /// </summary>
    /// <param name="userId">User performing the restore</param>
    /// <param name="fileStream">Backup file stream</param>
    /// <param name="fileName">Original filename</param>
    /// <param name="progress">Progress reporter for UI updates</param>
    Task<RestoreResult> RestoreFromUploadAsync(int userId, Stream fileStream, string fileName, IProgress<string>? progress = null);
    
    // ===== Operation Lock =====
    
    /// <summary>
    /// Try to acquire an operation lock to prevent concurrent operations.
    /// </summary>
    /// <param name="operationType">Type of operation: "Backup", "Restore", "Migration"</param>
    /// <param name="userId">User starting the operation</param>
    /// <returns>Operation ID if lock acquired, null if another operation is running</returns>
    Task<int?> TryAcquireOperationLockAsync(string operationType, int userId);
    
    /// <summary>
    /// Release an operation lock and mark it as completed.
    /// </summary>
    /// <param name="operationId">Operation ID to release</param>
    /// <param name="status">"Completed", "Failed", or "Cancelled"</param>
    /// <param name="errorMessage">Error message if failed</param>
    Task ReleaseOperationLockAsync(int operationId, string status = "Completed", string? errorMessage = null);
    
    /// <summary>
    /// Get the currently running operation, if any.
    /// </summary>
    Task<DatabaseOperation?> GetCurrentOperationAsync();
    
    /// <summary>
    /// Update the progress log for an operation.
    /// </summary>
    Task UpdateOperationProgressAsync(int operationId, string progressMessage);
    
    // ===== Maintenance Mode =====
    
    /// <summary>
    /// Enable or disable maintenance mode.
    /// </summary>
    Task SetMaintenanceModeAsync(bool enabled, int userId);
}
