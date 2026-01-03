using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.BlazorServer.Services.DataProtection;

public class DataProtectionService : IDataProtectionService
{
    private readonly IBlazorSystemSettingsService _settingsService;
    private readonly IAuditLogRepository _auditLogRepository;

    public DataProtectionService(IBlazorSystemSettingsService settingsService, IAuditLogRepository auditLogRepository)
    {
        _settingsService = settingsService;
        _auditLogRepository = auditLogRepository;
    }

    public async Task<DataHealthStatus> GetHealthStatusAsync()
    {
        var settings = await _settingsService.GetAsync();
        if (settings.LastSuccessfulBackupUtc == null)
        {
            return DataHealthStatus.Critical; // No backup ever = Critical
        }

        var age = DateTime.UtcNow - settings.LastSuccessfulBackupUtc.Value;
        if (age.TotalHours > 72)
        {
            return DataHealthStatus.Critical;
        }
        if (age.TotalHours > 24)
        {
            return DataHealthStatus.Warning;
        }
        
        return DataHealthStatus.Healthy;
    }

    public async Task LogBackupCompletesAsync(int userId)
    {
        var settings = await _settingsService.GetAsync();
        settings.LastSuccessfulBackupUtc = DateTime.UtcNow;
        await _settingsService.UpdateAsync(settings);

        _auditLogRepository.Insert(new AuditLogEntry
        {
            PerformedByUserId = userId,
            TimestampUtc = DateTime.UtcNow,
            Action = "BackupRecorded",
            Details = "External USB Backup marked as successful."
        });
    }

    public async Task LogRestoreTestCompletedAsync(int userId)
    {
        var settings = await _settingsService.GetAsync();
        settings.LastRestoreTestUtc = DateTime.UtcNow;
        await _settingsService.UpdateAsync(settings);

        _auditLogRepository.Insert(new AuditLogEntry
        {
            PerformedByUserId = userId,
            TimestampUtc = DateTime.UtcNow,
            Action = "RestoreTestRecorded",
            Details = "Disaster Recovery Restore Test marked as successful."
        });
    }

    public async Task<DateTime?> GetLastBackupTimeAsync()
    {
        var settings = await _settingsService.GetAsync();
        return settings.LastSuccessfulBackupUtc;
    }

    public async Task<DateTime?> GetLastRestoreTestTimeAsync()
    {
        var settings = await _settingsService.GetAsync();
        return settings.LastRestoreTestUtc;
    }
}
