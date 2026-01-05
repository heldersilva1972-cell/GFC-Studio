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

    public async Task<bool> ExecuteBackupAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var config = _configService.Load();
            
            if (!config.IsConfigured)
            {
                _logger.LogWarning("Backup configuration is not complete. Skipping backup.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(config.BackupFolder))
            {
                _logger.LogError("Backup folder is not configured.");
                return false;
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
                    return false;
                }
            }

            // Generate backup file name with timestamp
            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
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
            var backupSql = $@"
                BACKUP DATABASE [{config.DatabaseName}]
                TO DISK = @BackupPath
                WITH FORMAT, INIT, NAME = N'{config.DatabaseName}-Full Database Backup', 
                SKIP, NOREWIND, NOUNLOAD, STATS = 10";

            using var command = new SqlCommand(backupSql, connection);
            command.CommandTimeout = 300; // 5 minutes timeout
            command.Parameters.AddWithValue("@BackupPath", backupFilePath);

            _logger.LogInformation("Starting database backup: {DatabaseName} to {BackupPath}", 
                config.DatabaseName, backupFilePath);

            await command.ExecuteNonQueryAsync(cancellationToken);

            _logger.LogInformation("Database backup completed successfully: {BackupPath}", backupFilePath);

            // Update last backup time in config
            config.LastBackupTime = DateTime.UtcNow;
            _configService.Save(config);

            // Cleanup old backups
            await CleanupOldBackupsAsync(config.RetentionDays, cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing database backup");
            return false;
        }
    }

    public async Task<bool> CleanupOldBackupsAsync(int retentionDays, CancellationToken cancellationToken = default)
    {
        try
        {
            // Run file operations on a background thread to avoid blocking
            return await Task.Run(() =>
            {
                var config = _configService.Load();
                
                if (string.IsNullOrWhiteSpace(config.BackupFolder) || !Directory.Exists(config.BackupFolder))
                {
                    return false;
                }

                var cutoffDate = DateTime.UtcNow.AddDays(-retentionDays);
                var directory = new DirectoryInfo(config.BackupFolder);
                var oldBackups = directory.GetFiles("*.bak")
                    .Where(f => f.LastWriteTimeUtc < cutoffDate)
                    .ToList();

                if (oldBackups.Count == 0)
                {
                    return true;
                }

                _logger.LogInformation("Cleaning up {Count} old backup files (older than {CutoffDate})", 
                    oldBackups.Count, cutoffDate);

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
            _logger.LogError(ex, "Error cleaning up old backups");
            return false;
        }
    }

    public async Task<bool> RestoreDatabaseAsync(string backupFilePath, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(backupFilePath) || !File.Exists(backupFilePath))
            {
                _logger.LogError("Backup file not found: {BackupPath}", backupFilePath);
                return false;
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
            return true;
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
            
            return false;
        }
    }
}

