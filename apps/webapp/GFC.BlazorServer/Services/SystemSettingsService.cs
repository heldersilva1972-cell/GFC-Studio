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
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;
    private readonly ILogger<SystemSettingsService> _logger;

    public SystemSettingsService(IDbContextFactory<GfcDbContext> contextFactory, ILogger<SystemSettingsService> logger)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SystemSettings> GetAsync()
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        var settings = await dbContext.SystemSettings.FindAsync(1);
        
        if (settings == null)
        {
            // Create default settings if none exist
            settings = new SystemSettings
            {
                Id = 1,

                LastUpdatedUtc = null
            };
            
            dbContext.SystemSettings.Add(settings);
            await dbContext.SaveChangesAsync();
            _logger.LogInformation("Created default SystemSettings");
        }
        
        return settings;
    }

    public SystemSettings GetSettings()
    {
        using var dbContext = _contextFactory.CreateDbContext();
        var settings = dbContext.SystemSettings.Find(1);
        
        if (settings == null)
        {
            // Create default settings if none exist
            settings = new SystemSettings
            {
                Id = 1,
                LastUpdatedUtc = null
            };
            
            dbContext.SystemSettings.Add(settings);
            dbContext.SaveChanges();
            _logger.LogInformation("Created default SystemSettings");
        }
        
        return settings;
    }

    public async Task UpdateNvrCredentialsAsync(string nvrIpAddress, int nvrPort, string username, string password)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        var settings = await dbContext.SystemSettings.FindAsync(1);
        if (settings == null) return;
        
        settings.NvrIpAddress = nvrIpAddress;
        settings.NvrPort = nvrPort;
        settings.NvrUsername = username;
        settings.NvrPassword = password; // Note: In production, encrypt this!
        settings.LastUpdatedUtc = DateTime.UtcNow;
        
        await dbContext.SaveChangesAsync();
        _logger.LogInformation("Updated NVR credentials");
    }

    public async Task UpdateAsync(SystemSettings settings)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync();
        var existingSettings = await dbContext.SystemSettings.FindAsync(1);
        if (existingSettings == null) return;

        // Remote Access Configuration
        existingSettings.CloudflareTunnelToken = settings.CloudflareTunnelToken;
        existingSettings.PublicDomain = settings.PublicDomain;
        existingSettings.WireGuardPort = settings.WireGuardPort;
        existingSettings.WireGuardSubnet = settings.WireGuardSubnet;
        existingSettings.LanSubnet = settings.LanSubnet;

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

        // Hosting & Security Framework (Phase 2)
        existingSettings.HostingEnvironment = settings.HostingEnvironment;
        existingSettings.DeviceTrustDurationDays = settings.DeviceTrustDurationDays;
        existingSettings.MagicLinkEnabled = settings.MagicLinkEnabled;
        existingSettings.EnforceVpn = settings.EnforceVpn;

        existingSettings.LastUpdatedUtc = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
        _logger.LogInformation("Updated system settings");
    }
}

