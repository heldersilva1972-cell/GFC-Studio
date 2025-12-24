using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Service for managing system-wide settings.
/// </summary>
public class SystemSettingsService : ISystemSettingsService
{
    private readonly GfcDbContext _dbContext;
    private readonly ILogger<SystemSettingsService> _logger;

    public SystemSettingsService(GfcDbContext dbContext, ILogger<SystemSettingsService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SystemSettings> GetAsync()
    {
        var settings = await _dbContext.SystemSettings.FindAsync(1);
        
        if (settings == null)
        {
            // Create default settings if none exist
            settings = new SystemSettings
            {
                Id = 1,

                LastUpdatedUtc = null
            };
            
            _dbContext.SystemSettings.Add(settings);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Created default SystemSettings");
        }
        
        return settings;
    }

    public SystemSettings GetSettings()
    {
        var settings = _dbContext.SystemSettings.Find(1);
        
        if (settings == null)
        {
            // Create default settings if none exist
            settings = new SystemSettings
            {
                Id = 1,
                LastUpdatedUtc = null
            };
            
            _dbContext.SystemSettings.Add(settings);
            _dbContext.SaveChanges();
            _logger.LogInformation("Created default SystemSettings");
        }
        
        return settings;
    }

    public async Task UpdateNvrCredentialsAsync(string nvrIpAddress, int nvrPort, string username, string password)
    {
        var settings = await GetAsync();
        
        settings.NvrIpAddress = nvrIpAddress;
        settings.NvrPort = nvrPort;
        settings.NvrUsername = username;
        settings.NvrPassword = password; // Note: In production, encrypt this!
        settings.LastUpdatedUtc = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Updated NVR credentials");
    }

    public async Task UpdateSecuritySettingsAsync(SystemSettings settings)
    {
        var existingSettings = await GetAsync();

        // Remote Access Configuration
        existingSettings.CloudflareTunnelToken = settings.CloudflareTunnelToken;
        existingSettings.PublicDomain = settings.PublicDomain;
        existingSettings.WireGuardPort = settings.WireGuardPort;
        existingSettings.WireGuardSubnet = settings.WireGuardSubnet;

        // User & Permission Management
        existingSettings.DirectorAccessExpiryDate = settings.DirectorAccessExpiryDate;

        // Security "Hardening" Toggles
        existingSettings.EnableTwoFactorAuth = settings.EnableTwoFactorAuth;
        existingSettings.EnableSessionTimeout = settings.EnableSessionTimeout;
        existingSettings.SessionTimeoutMinutes = settings.SessionTimeoutMinutes;
        existingSettings.EnableFailedLoginProtection = settings.EnableFailedLoginProtection;
        existingSettings.MaxFailedLoginAttempts = settings.MaxFailedLoginAttempts;
        existingSettings.LoginLockDurationMinutes = settings.LoginLockDurationMinutes;
        existingSettings.EnableIPFiltering = settings.EnableIPFiltering;
        existingSettings.IPFilterMode = settings.IPFilterMode;
        existingSettings.EnableWatermarking = settings.EnableWatermarking;
        existingSettings.WatermarkPosition = settings.WatermarkPosition;

        // System Limits & "Mission Control"
        existingSettings.MaxSimultaneousViewers = settings.MaxSimultaneousViewers;
        existingSettings.RemoteQualityMaxBitrate = settings.RemoteQualityMaxBitrate;
        existingSettings.LocalQualityMaxBitrate = settings.LocalQualityMaxBitrate;

        // Monitoring & Alerts
        existingSettings.EnableGeofencing = settings.EnableGeofencing;
        existingSettings.EnableConnectionQualityAlerts = settings.EnableConnectionQualityAlerts;
        existingSettings.MinimumBandwidthMbps = settings.MinimumBandwidthMbps;

        existingSettings.LastUpdatedUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Updated security settings");
    }
}

