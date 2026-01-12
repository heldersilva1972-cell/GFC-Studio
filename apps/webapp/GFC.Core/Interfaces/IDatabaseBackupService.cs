namespace GFC.Core.Interfaces;

public interface IDatabaseBackupService
{
    Task<bool> ExecuteBackupAsync(CancellationToken cancellationToken = default);
    Task<bool> CleanupOldBackupsAsync(int retentionDays, CancellationToken cancellationToken = default);
    Task<bool> RestoreDatabaseAsync(string backupFilePath, CancellationToken cancellationToken = default);
    Task<IEnumerable<System.IO.FileInfo>> GetAvailableBackupsAsync();
}

