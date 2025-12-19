namespace GFC.Core.Interfaces;

public interface IDatabaseBackupService
{
    Task<bool> ExecuteBackupAsync(CancellationToken cancellationToken = default);
    Task<bool> CleanupOldBackupsAsync(int retentionDays, CancellationToken cancellationToken = default);
}

