using GFC.Core.Interfaces;
using GFC.Core.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Background service that runs database backups on a scheduled basis.
/// </summary>
public class BackupSchedulerService : BackgroundService
{
    private readonly BackupConfigService _configService;
    private readonly IDatabaseBackupService _backupService;
    private readonly ILogger<BackupSchedulerService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1); // Check every minute

    public BackupSchedulerService(
        BackupConfigService configService,
        IDatabaseBackupService backupService,
        ILogger<BackupSchedulerService> logger)
    {
        _configService = configService ?? throw new ArgumentNullException(nameof(configService));
        _backupService = backupService ?? throw new ArgumentNullException(nameof(backupService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Backup Scheduler Service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var config = _configService.Load();
                
                if (config.IsConfigured && ShouldRunBackup(config))
                {
                    _logger.LogInformation("Scheduled backup time reached. Starting backup...");
                    var success = await _backupService.ExecuteBackupAsync(stoppingToken);
                    
                    if (success)
                    {
                        _logger.LogInformation("Scheduled backup completed successfully.");
                    }
                    else
                    {
                        _logger.LogWarning("Scheduled backup failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in backup scheduler");
            }

            // Wait before checking again
            await Task.Delay(_checkInterval, stoppingToken);
        }

        _logger.LogInformation("Backup Scheduler Service stopped.");
    }

    private bool ShouldRunBackup(BackupConfig config)
    {
        try
        {
            var now = DateTime.UtcNow;
            var scheduledTime = now.Date.Add(config.DailyBackupTime);
            
            // If scheduled time has passed today, check if we've already backed up today
            if (now >= scheduledTime)
            {
                // Check if we've already backed up today
                if (config.LastBackupTime.HasValue)
                {
                    var lastBackupDate = config.LastBackupTime.Value.Date;
                    var today = now.Date;
                    
                    // If we already backed up today, don't run again
                    if (lastBackupDate == today)
                    {
                        return false;
                    }
                }
                
                // Check if we're within the backup window (scheduled time + 1 hour)
                var backupWindowEnd = scheduledTime.AddHours(1);
                if (now <= backupWindowEnd)
                {
                    return true;
                }
            }
            
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error determining if backup should run");
            return false;
        }
    }
}

