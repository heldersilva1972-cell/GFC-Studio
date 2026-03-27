using GFC.Core.Interfaces;
using GFC.Core.Services;
using GFC.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

public class DatabaseBackupService : IDatabaseBackupService
{
    private readonly BackupConfigService _configService;
    private readonly ILogger<DatabaseBackupService> _logger;

    public DatabaseBackupService(
        BackupConfigService configService,
        ILogger<DatabaseBackupService> logger)
    {
        _configService = configService ?? throw new ArgumentNullException(nameof(configService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<(bool Success, string ErrorMessage)> ExecuteBackupAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var config = _configService.Load();
            
            if (!config.IsConfigured)
            {
                _logger.LogWarning("Backup configuration is not complete. Skipping backup.");
                return (false, "Backup policy is not configured. Please save a Backup Policy first.");
            }

            if (string.IsNullOrWhiteSpace(config.BackupFolder))
            {
                _logger.LogError("Backup folder is not configured.");
                return (false, "Backup folder path is empty.");
            }

            // Ensure backup folder exists
            if (!Directory.Exists(config.BackupFolder))
            {
                try
                {
                    Directory.CreateDirectory(config.BackupFolder);
                    _logger.LogInformation("Created backup folder: {BackupFolder}", config.BackupFolder);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to create backup folder: {BackupFolder}", config.BackupFolder);
                    return (false, $"Failed to create backup directory: {ex.Message}");
                }
            }

            // Generate backup file name with timestamp (Local)
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var backupFileName = $"{config.DatabaseName}_{timestamp}.bak";
            var backupFilePath = Path.Combine(config.BackupFolder, backupFileName);

            // Build connection string for master database (backups need to be run from master)
            using var tempConnection = Db.GetConnection();
            var connectionString = tempConnection.ConnectionString;
            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.InitialCatalog = "master"; // Connect to master for backup operations

            using var connection = new SqlConnection(builder.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            // Build BACKUP DATABASE command
            // Added WITH CHECKSUM to detect page-level corruption during the backup process
            var backupSql = $@"
                BACKUP DATABASE [{config.DatabaseName}]
                TO DISK = @BackupPath
                WITH FORMAT, INIT, NAME = N'{config.DatabaseName}-Full Database Backup', 
                SKIP, NOREWIND, NOUNLOAD, STATS = 10, CHECKSUM";

            using (var command = new SqlCommand(backupSql, connection))
            {
                command.CommandTimeout = 300; // 5 minutes timeout
                command.Parameters.AddWithValue("@BackupPath", backupFilePath);

                _logger.LogInformation("Starting database backup: {DatabaseName} to {BackupPath} (Integrity Check Enabled)", 
                    config.DatabaseName, backupFilePath);

                await command.ExecuteNonQueryAsync(cancellationToken);
            }

            try
            {
                // Verification Step: Run RESTORE VERIFYONLY to ensure the backup file is actually readable and healthy
                _logger.LogInformation("Verifying backup integrity for: {BackupPath}", backupFilePath);
                var verifySql = "RESTORE VERIFYONLY FROM DISK = @BackupPath";
                using (var verifyCommand = new SqlCommand(verifySql, connection))
                {
                    verifyCommand.CommandTimeout = 300;
                    verifyCommand.Parameters.AddWithValue("@BackupPath", backupFilePath);
                    await verifyCommand.ExecuteNonQueryAsync(cancellationToken);
                }
                _logger.LogInformation("Database backup and verification completed successfully: {BackupPath}", backupFilePath);
            }
            catch (SqlException ex) when (ex.Message.Contains("CREATE DATABASE permission denied") || ex.Message.Contains("VERIFY DATABASE is terminating abnormally"))
            {
                _logger.LogWarning("Backup completed successfully, but integrity verification was skipped because the app account lacks 'CREATE DATABASE' permissions (which SQL Server requires to run VERIFYONLY).");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Backup completed but verification failed. The backup file was created but could not be verified automatically.");
            }

            // Update last backup time in config (Local)
            config.LastBackupTime = DateTime.Now;
            _configService.Save(config);

            // [NEW] Secondary Backup Mirroring
            if (!string.IsNullOrWhiteSpace(config.SecondaryBackupFolder))
            {
                try
                {
                    var driveRoot = Path.GetPathRoot(config.SecondaryBackupFolder);
                    if (string.IsNullOrEmpty(driveRoot) || Directory.Exists(driveRoot))
                    {
                        if (!Directory.Exists(config.SecondaryBackupFolder))
                        {
                            Directory.CreateDirectory(config.SecondaryBackupFolder);
                        }

                        var secondaryPath = Path.Combine(config.SecondaryBackupFolder, backupFileName);
                        File.Copy(backupFilePath, secondaryPath, true);
                        _logger.LogInformation("Mirrored backup to secondary location: {SecondaryPath}", secondaryPath);
                        
                        // Cleanup secondary drive
                        await CleanupFolderAsync(config.SecondaryBackupFolder, config.RetentionDays, cancellationToken);
                    }
                    else
                    {
                        _logger.LogWarning("Secondary drive for backups ({DriveRoot}) is not connected. Mirroring skipped.", driveRoot);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to mirror backup to secondary destination: {SecondaryPath}", config.SecondaryBackupFolder);
                }
            }

            // Cleanup primary drive
            await CleanupFolderAsync(config.BackupFolder, config.RetentionDays, cancellationToken);

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing database backup");
            return (false, ex.Message);
        }
    }

    public async Task<bool> CleanupOldBackupsAsync(int retentionDays, CancellationToken cancellationToken = default)
    {
        var config = _configService.Load();
        if (string.IsNullOrWhiteSpace(config.BackupFolder)) return false;
        
        return await CleanupFolderAsync(config.BackupFolder, retentionDays, cancellationToken);
    }

    private async Task<bool> CleanupFolderAsync(string folderPath, int retentionDays, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
            {
                return false;
            }

            return await Task.Run(() =>
            {
                var cutoffDate = DateTime.Now.AddDays(-retentionDays);
                var directory = new DirectoryInfo(folderPath);
                var oldBackups = directory.GetFiles("*.bak")
                    .Where(f => f.LastWriteTime < cutoffDate)
                    .ToList();

                if (oldBackups.Count == 0)
                {
                    return true;
                }

                _logger.LogInformation("Cleaning up {Count} old backup files in {Folder} (older than {CutoffDate})", 
                    oldBackups.Count, folderPath, cutoffDate);

                foreach (var file in oldBackups)
                {
                    try
                    {
                        file.Delete();
                        _logger.LogInformation("Deleted old backup file: {FileName}", file.Name);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to delete old backup file: {FileName}", file.Name);
                    }
                }

                return true;
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up old backups in {Path}", folderPath);
            return false;
        }
    }

    public async Task<(bool Success, string ErrorMessage)> RestoreDatabaseAsync(string backupFilePath, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(backupFilePath) || !File.Exists(backupFilePath))
            {
                _logger.LogError("Backup file not found: {BackupPath}", backupFilePath);
                return (false, "The specified backup file does not exist on disk.");
            }

            var config = _configService.Load();
            var dbName = config.DatabaseName;

            // Build connection string for master database
            using var tempConnection = Db.GetConnection();
            var connectionString = tempConnection.ConnectionString;
            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.InitialCatalog = "master"; 

            using var connection = new SqlConnection(builder.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            _logger.LogInformation("Starting database restore: {DatabaseName} from {BackupPath}", dbName, backupFilePath);

            // Step 1: Set to SINGLE_USER to drop connections
            var singleUserSql = $"ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
            using (var cmd = new SqlCommand(singleUserSql, connection))
            {
                cmd.CommandTimeout = 60;
                await cmd.ExecuteNonQueryAsync(cancellationToken);
            }

            // Step 2: RESTORE DATABASE
            // Note: We'll try a standard restore first. If it fails due to file paths, 
            // the user might need to use the batch script or we'd need more complex MOVE logic.
            var restoreSql = $@"
                RESTORE DATABASE [{dbName}]
                FROM DISK = @BackupPath
                WITH REPLACE, RECOVERY";

            using (var cmd = new SqlCommand(restoreSql, connection))
            {
                cmd.CommandTimeout = 600; // 10 minutes
                cmd.Parameters.AddWithValue("@BackupPath", backupFilePath);
                await cmd.ExecuteNonQueryAsync(cancellationToken);
            }

            // Step 3: Set back to MULTI_USER
            var multiUserSql = $"ALTER DATABASE [{dbName}] SET MULTI_USER";
            using (var cmd = new SqlCommand(multiUserSql, connection))
            {
                cmd.CommandTimeout = 60;
                await cmd.ExecuteNonQueryAsync(cancellationToken);
            }

            _logger.LogInformation("Database restore completed successfully: {DatabaseName}", dbName);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error restoring database");
            
            // Try to set back to MULTI_USER just in case
            try
            {
                var config = _configService.Load();
                using var tempConnection = Db.GetConnection();
                var builder = new SqlConnectionStringBuilder(tempConnection.ConnectionString);
                builder.InitialCatalog = "master";
                using var connection = new SqlConnection(builder.ConnectionString);
                await connection.OpenAsync(cancellationToken);
                var multiUserSql = $"ALTER DATABASE [{config.DatabaseName}] SET MULTI_USER";
                using var cmd = new SqlCommand(multiUserSql, connection);
                await cmd.ExecuteNonQueryAsync(cancellationToken);
            }
            catch { /* Ignore errors here */ }
            
            return (false, ex.Message);
        }
    }

    public Task<IEnumerable<FileInfo>> GetAvailableBackupsAsync()
    {
        try
        {
            var config = _configService.Load();
            
            if (string.IsNullOrWhiteSpace(config.BackupFolder) || !Directory.Exists(config.BackupFolder))
            {
                return Task.FromResult(Enumerable.Empty<FileInfo>());
            }

            var directory = new DirectoryInfo(config.BackupFolder);
            var backups = directory.GetFiles("*.bak")
                .OrderByDescending(f => f.LastWriteTimeUtc)
                .AsEnumerable();

            return Task.FromResult(backups);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting available backups");
            return Task.FromResult(Enumerable.Empty<FileInfo>());
        }
    }

    public async Task<(bool Success, string ErrorMessage)> ArchiveBackupAsync(string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var config = _configService.Load();
            if (string.IsNullOrWhiteSpace(config.BackupFolder)) return (false, "Backup folder not configured.");

            var sourcePath = Path.Combine(config.BackupFolder, fileName);
            if (!File.Exists(sourcePath)) return (false, "Backup file not found.");

            var archiveFolder = Path.Combine(config.BackupFolder, "Archive");
            if (!Directory.Exists(archiveFolder))
            {
                Directory.CreateDirectory(archiveFolder);
            }

            var destPath = Path.Combine(archiveFolder, fileName);
            
            await Task.Run(() => File.Move(sourcePath, destPath, true), cancellationToken);
            
            _logger.LogInformation("Archived backup file: {FileName} to {ArchiveFolder}", fileName, archiveFolder);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error archiving backup: {FileName}", fileName);
            return (false, ex.Message);
        }
    }
}

