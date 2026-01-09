using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Service for managing system-wide settings.
/// </summary>
public class SystemSettingsService : IBlazorSystemSettingsService, GFC.Core.Interfaces.ISystemSettingsService
{
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;
    private readonly ILogger<SystemSettingsService> _logger;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "SystemSettings";
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);

    public SystemSettingsService(
        IDbContextFactory<GfcDbContext> contextFactory, 
        ILogger<SystemSettingsService> logger,
        IMemoryCache cache)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<SystemSettings> GetAsync()
    {
        // Try to get from cache first
        if (_cache.TryGetValue(CacheKey, out SystemSettings? cachedSettings) && cachedSettings != null)
        {
            return cachedSettings;
        }

        try
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
            
            // Cache the settings
            _cache.Set(CacheKey, settings, CacheExpiration);
            
            return settings;
        }
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 208) // Invalid object name
        {
            _logger.LogWarning(ex, "SystemSettings table missing. Returning default settings.");
            var defaultSettings = new SystemSettings { Id = 1 };
            // Cache default settings for shorter duration
            _cache.Set(CacheKey, defaultSettings, TimeSpan.FromMinutes(1));
            return defaultSettings;
        }
        catch (Exception ex)
        {
             _logger.LogError(ex, "Error retrieving SystemSettings");
             var defaultSettings = new SystemSettings { Id = 1 };
             // Cache default settings for shorter duration
             _cache.Set(CacheKey, defaultSettings, TimeSpan.FromMinutes(1));
             return defaultSettings;
        }
    }

    public async Task<SystemSettings> GetSystemSettingsAsync() => await GetAsync();

    public async Task<int> GetTrustedDeviceDurationDaysAsync()
    {
        var settings = await GetAsync();
        return settings.TrustedDeviceDurationDays;
    }

    public async Task<bool> GetSafeModeEnabledAsync()
    {
        var settings = await GetAsync();
        return settings.SafeModeEnabled;
    }

    public async Task<bool> GetEnableTwoFactorAuthAsync()
    {
        var settings = await GetAsync();
        return settings.EnableTwoFactorAuth;
    }

    public async Task<string?> GetTwilioAccountSidAsync()
    {
        var settings = await GetAsync();
        return settings.TwilioAccountSid;
    }

    public async Task<string?> GetTwilioAuthTokenAsync()
    {
        var settings = await GetAsync();
        return settings.TwilioAuthToken;
    }

    public async Task<string?> GetTwilioFromNumberAsync()
    {
        var settings = await GetAsync();
        return settings.TwilioFromNumber;
    }

    public async Task<string> GetPreferredMfaMethodAsync()
    {
        var settings = await GetAsync();
        return settings.PreferredMagicLinkMethod ?? "Email";
    }

    public SystemSettings GetSettings()
    {
        // Try to get from cache first
        if (_cache.TryGetValue(CacheKey, out SystemSettings? cachedSettings) && cachedSettings != null)
        {
            return cachedSettings;
        }

        try
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
            
            // Cache the settings
            _cache.Set(CacheKey, settings, CacheExpiration);
            
            return settings;
        }
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 208) // Invalid object name
        {
            _logger.LogWarning(ex, "SystemSettings table missing. Returning default settings.");
            var defaultSettings = new SystemSettings { Id = 1 };
            _cache.Set(CacheKey, defaultSettings, TimeSpan.FromMinutes(1));
            return defaultSettings;
        }
        catch (Exception ex)
        {
             _logger.LogError(ex, "Error retrieving SystemSettings");
             var defaultSettings = new SystemSettings { Id = 1 };
             _cache.Set(CacheKey, defaultSettings, TimeSpan.FromMinutes(1));
             return defaultSettings;
        }
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
        existingSettings.PrimaryDomain = settings.PrimaryDomain;
        existingSettings.AllowedDomains = settings.AllowedDomains;
        existingSettings.DomainSwitchPending = settings.DomainSwitchPending;
        existingSettings.DomainSwitchExpiryUtc = settings.DomainSwitchExpiryUtc;
        existingSettings.LastConfirmedDomain = settings.LastConfirmedDomain;
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
        existingSettings.TrustedDeviceDurationDays = settings.TrustedDeviceDurationDays;
        existingSettings.MagicLinkEnabled = settings.MagicLinkEnabled;
        existingSettings.EnforceVpn = settings.EnforceVpn;
        existingSettings.AccessMode = settings.AccessMode;
        existingSettings.IdleTimeoutMinutes = settings.IdleTimeoutMinutes;
        existingSettings.AbsoluteSessionMaxMinutes = settings.AbsoluteSessionMaxMinutes;
        existingSettings.SafeModeEnabled = settings.SafeModeEnabled;
        existingSettings.EnableOnboarding = settings.EnableOnboarding;

        // SMS & Twilio Settings
        existingSettings.TwilioAccountSid = settings.TwilioAccountSid;
        existingSettings.TwilioAuthToken = settings.TwilioAuthToken;
        existingSettings.TwilioFromNumber = settings.TwilioFromNumber;
        existingSettings.PreferredMagicLinkMethod = settings.PreferredMagicLinkMethod;

        existingSettings.LastUpdatedUtc = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
        
        // Invalidate cache so next request gets fresh data
        _cache.Remove(CacheKey);
        
        _logger.LogInformation("Updated system settings");
    }
}

