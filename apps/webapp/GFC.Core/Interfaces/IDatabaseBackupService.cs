namespace GFC.Core.Interfaces;

public interface IDatabaseBackupService
{
    Task<(bool Success, string ErrorMessage)> ExecuteBackupAsync(CancellationToken cancellationToken = default);
    Task<bool> CleanupOldBackupsAsync(int retentionDays, CancellationToken cancellationToken = default);
    Task<(bool Success, string ErrorMessage)> RestoreDatabaseAsync(string backupFilePath, CancellationToken cancellationToken = default);
    Task<IEnumerable<System.IO.FileInfo>> GetAvailableBackupsAsync();
}

