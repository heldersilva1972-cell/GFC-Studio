using GFC.BlazorServer.Data;
using GFC.Core.Models;
using GFC.Core.Interfaces;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using GFC.Core.Enums;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net.Http.Json;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Service for managing system-wide settings.
/// </summary>
public class SystemSettingsService : IBlazorSystemSettingsService, GFC.Core.Interfaces.ISystemSettingsService
{
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;
    private readonly ILogger<SystemSettingsService> _logger;
    private readonly IMemoryCache _cache;
    private readonly IServiceProvider _serviceProvider;
    private readonly IEncryptionService _encryptionService;
    private const string CacheKey = "SystemSettings";
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);

    public SystemSettingsService(
        IDbContextFactory<GfcDbContext> contextFactory, 
        ILogger<SystemSettingsService> logger,
        IMemoryCache cache,
        IServiceProvider serviceProvider,
        IEncryptionService encryptionService)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
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
                    LastUpdatedUtc = null,
                    IdleTimeoutMinutes = 60,
                    AbsoluteSessionMaxMinutes = 1440,
                    TrustedDeviceDurationDays = 30,
                    AccessMode = AccessMode.Open,
                    HostingEnvironment = "Production"
                };
                
                dbContext.SystemSettings.Add(settings);
                await dbContext.SaveChangesAsync();
                _logger.LogInformation("Created default SystemSettings with fixed defaults");
            }
            
            if (settings != null)
            {
                if (!string.IsNullOrEmpty(settings.ResendApiKey))
                    settings.ResendApiKey = _encryptionService.Decrypt(settings.ResendApiKey);
                
                if (!string.IsNullOrEmpty(settings.SmtpPassword))
                    settings.SmtpPassword = _encryptionService.Decrypt(settings.SmtpPassword);
            }
            
            // Cache the settings
            _cache.Set(CacheKey, settings, CacheExpiration);
            
            // Sync ShiftDefaults
            GFC.Core.Models.ShiftDefaults.SetDefaults(
                settings.DayShiftStartTime, 
                settings.DayShiftEndTime, 
                settings.NightShiftStartTime, 
                settings.NightShiftEndTime
            );
            
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
    public async Task<SystemSettings> GetSettingsAsync() => await GetAsync();
    public async Task SaveSettingsAsync(SystemSettings settings) => await UpdateAsync(settings);

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
        // MFA is currently disabled globally. If enabled, we default to SMS if available, then Email.
        return "SMS"; 
    }

    public async Task<bool> GetSmsEnabledAsync()
    {
        var settings = await GetAsync();
        return settings.SmsEnabled;
    }

    public async Task<bool> GetEmailEnabledAsync()
    {
        var settings = await GetAsync();
        return settings.EmailEnabled;
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
                    LastUpdatedUtc = null,
                    IdleTimeoutMinutes = 60,
                    AbsoluteSessionMaxMinutes = 1440,
                    TrustedDeviceDurationDays = 30,
                    AccessMode = AccessMode.Open,
                    HostingEnvironment = "Production"
                };
                
                dbContext.SystemSettings.Add(settings);
                dbContext.SaveChanges();
                _logger.LogInformation("Created default SystemSettings with fixed defaults (sync)");
            }
            
            if (settings != null)
            {
                if (!string.IsNullOrEmpty(settings.ResendApiKey))
                    settings.ResendApiKey = _encryptionService.Decrypt(settings.ResendApiKey);
                
                if (!string.IsNullOrEmpty(settings.SmtpPassword))
                    settings.SmtpPassword = _encryptionService.Decrypt(settings.SmtpPassword);
            }

            // Cache the settings
            _cache.Set(CacheKey, settings, CacheExpiration);
            
            // Sync ShiftDefaults
            GFC.Core.Models.ShiftDefaults.SetDefaults(
                settings.DayShiftStartTime, 
                settings.DayShiftEndTime, 
                settings.NightShiftStartTime, 
                settings.NightShiftEndTime
            );
            
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
        existingSettings.EnforceVpn = settings.EnforceVpn;
        existingSettings.AccessMode = settings.AccessMode;
        existingSettings.IdleTimeoutMinutes = settings.IdleTimeoutMinutes;
        existingSettings.AbsoluteSessionMaxMinutes = settings.AbsoluteSessionMaxMinutes;
        existingSettings.SafeModeEnabled = settings.SafeModeEnabled;
        existingSettings.EnableOnboarding = settings.EnableOnboarding;
        existingSettings.SystemTimeZoneId = settings.SystemTimeZoneId;

        // SMS & Twilio Settings
        existingSettings.SmsEnabled = settings.SmsEnabled;
        existingSettings.TwilioAccountSid = settings.TwilioAccountSid;
        existingSettings.TwilioAuthToken = settings.TwilioAuthToken;
        existingSettings.TwilioFromNumber = settings.TwilioFromNumber;
        
        // Email & Gateway Settings
        existingSettings.EmailEnabled = settings.EmailEnabled;
        existingSettings.EmailProvider = settings.EmailProvider;
        
        // Encrypt Resend API Key before saving
        if (!string.IsNullOrEmpty(settings.ResendApiKey))
        {
            existingSettings.ResendApiKey = _encryptionService.Encrypt(settings.ResendApiKey);
        }
        else
        {
            existingSettings.ResendApiKey = settings.ResendApiKey;
        }

        existingSettings.SmtpHost = settings.SmtpHost;
        existingSettings.SmtpPort = settings.SmtpPort;
        existingSettings.SmtpUsername = settings.SmtpUsername;
        
        // Encrypt SMTP Password before saving
        if (!string.IsNullOrEmpty(settings.SmtpPassword))
        {
            existingSettings.SmtpPassword = _encryptionService.Encrypt(settings.SmtpPassword);
        }
        else
        {
            existingSettings.SmtpPassword = settings.SmtpPassword;
        }

        existingSettings.SmtpEnableSsl = settings.SmtpEnableSsl;
        existingSettings.SmtpFromAddress = settings.SmtpFromAddress;
        existingSettings.SmtpFromName = settings.SmtpFromName;



        // Web Push Settings (Phase 4)
        existingSettings.PushEnabled = settings.PushEnabled;
        existingSettings.VapidPublicKey = settings.VapidPublicKey;
        existingSettings.VapidPrivateKey = settings.VapidPrivateKey;
        existingSettings.VapidSubject = settings.VapidSubject;

        // Maintenance & NVR (Phase 3)
        existingSettings.LastSuccessfulBackupUtc = settings.LastSuccessfulBackupUtc;
        existingSettings.LastRestoreTestUtc = settings.LastRestoreTestUtc;
        existingSettings.BackupStoragePath = settings.BackupStoragePath;
        existingSettings.BackupRetentionCount = settings.BackupRetentionCount;
        existingSettings.BackupFrequencyHours = settings.BackupFrequencyHours;
        existingSettings.BackupMethod = settings.BackupMethod;
        existingSettings.AllowServerRestoreOperations = settings.AllowServerRestoreOperations;
        existingSettings.MaintenanceModeEnabled = settings.MaintenanceModeEnabled;

        // Standard Shift Times
        existingSettings.DayShiftStartTime = settings.DayShiftStartTime;
        existingSettings.DayShiftEndTime = settings.DayShiftEndTime;
        existingSettings.NightShiftStartTime = settings.NightShiftStartTime;
        existingSettings.NightShiftEndTime = settings.NightShiftEndTime;

        existingSettings.LastSignInDrawExportUtc = settings.LastSignInDrawExportUtc;
        
        // Liquor Specific Settings
        existingSettings.LiquorEmailEnabled = settings.LiquorEmailEnabled;
        existingSettings.LiquorEmailSignature = settings.LiquorEmailSignature;
        existingSettings.LiquorEmailFooter = settings.LiquorEmailFooter;
        existingSettings.LiquorEmailCc = settings.LiquorEmailCc;
        existingSettings.GlobalLiquorPourSize = settings.GlobalLiquorPourSize;


        existingSettings.LastUpdatedUtc = DateTime.UtcNow;

        // Sync ShiftDefaults immediately
        GFC.Core.Models.ShiftDefaults.SetDefaults(
            settings.DayShiftStartTime, 
            settings.DayShiftEndTime, 
            settings.NightShiftStartTime, 
            settings.NightShiftEndTime
        );

        await dbContext.SaveChangesAsync();
        
        // Invalidate main settings cache
        _cache.Remove(CacheKey);
        
        // Invalidate EmailSettings options cache (for IOptionsMonitor to detect changes immediately)
        try 
        {
            var optionsCache = _serviceProvider.GetService<IOptionsMonitorCache<EmailSettings>>();
            optionsCache?.Clear();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to clear EmailSettings options cache.");
        }
        
        _logger.LogInformation("Updated system settings and invalidated options cache");
    }

    public async Task<(bool Success, string Message)> TestEmailConnectionAsync(SystemSettings settings)
    {
        try
        {
            if (settings.EmailProvider == EmailProvider.Resend)
            {
                if (string.IsNullOrEmpty(settings.ResendApiKey))
                    return (false, "Resend API Key is required.");

                // Test Resend API
                // Since ResendClient requires DI and we're testing unsaved settings, we use a direct HTTP call
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", settings.ResendApiKey);
                
                var testMessage = new
                {
                    from = string.IsNullOrWhiteSpace(settings.SmtpFromName) 
                        ? settings.SmtpFromAddress 
                        : $"\"{settings.SmtpFromName}\" <{settings.SmtpFromAddress}>",
                    to = new[] { settings.SmtpFromAddress },
                    subject = "LiquorHub Connection Test",
                    html = "<strong>Success!</strong> Your Resend API connection is working correctly."
                };

                var response = await httpClient.PostAsJsonAsync("https://api.resend.com/emails", testMessage);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Resend API connection verified.");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return (false, $"Resend API Error: {response.StatusCode} - {error}");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(settings.SmtpHost))
                    return (false, "SMTP Host is required.");

                // Test SMTP Connection using MailKit
                using var client = new MailKit.Net.Smtp.SmtpClient();
                await client.ConnectAsync(settings.SmtpHost, settings.SmtpPort, settings.SmtpEnableSsl ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.None);
                
                if (!string.IsNullOrEmpty(settings.SmtpUsername))
                {
                    await client.AuthenticateAsync(settings.SmtpUsername, settings.SmtpPassword);
                }
                
                await client.DisconnectAsync(true);
                return (true, "SMTP connection verified.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Email connection test failed");
            return (false, ex.Message);
        }
    }
}

