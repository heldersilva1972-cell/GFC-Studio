using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GFC.Core.Enums;

namespace GFC.Core.Models;

/// <summary>
/// System-wide settings. Only one row should exist (Id = 1).
/// Shared across Server and Mobile Standalone.
/// </summary>
[Table("SystemSettings")]
public class SystemSettings
{
    [Key]
    public int Id { get; set; } = 1;

    public int? ScannerControllerId { get; set; }
    public DateTime? LastUpdatedUtc { get; set; }
    public string? NvrIpAddress { get; set; }
    public int? NvrPort { get; set; }
    public string? NvrUsername { get; set; }
    public string? NvrPassword { get; set; }
    public string? CloudflareTunnelToken { get; set; }
    public string? PrimaryDomain { get; set; }
    public string? AllowedDomains { get; set; }
    public string? DomainSwitchPending { get; set; }
    public DateTime? DomainSwitchExpiryUtc { get; set; }
    public string? LastConfirmedDomain { get; set; }
    public int WireGuardPort { get; set; } = 51820;
    public string WireGuardSubnet { get; set; } = "10.20.0.0/24";
    public string? WireGuardServerPublicKey { get; set; }
    public string WireGuardAllowedIPs { get; set; } = "10.20.0.0/24";
    public int MaxSimultaneousViewers { get; set; } = 10;
    public bool EnableTwoFactorAuth { get; set; } = false;
    public bool EnableIPFiltering { get; set; } = false;
    public int MinimumBandwidthMbps { get; set; } = 5;
    public bool EnableSessionTimeout { get; set; } = true;
    public int SessionTimeoutMinutes { get; set; } = 30;
    public int IdleTimeoutMinutes { get; set; } = 20;
    public int AbsoluteSessionMaxMinutes { get; set; } = 1440;
    public bool EnableFailedLoginProtection { get; set; } = true;
    public int MaxFailedLoginAttempts { get; set; } = 5;
    public string IPFilterMode { get; set; } = "Whitelist";
    public int LoginLockDurationMinutes { get; set; } = 30;
    public string WatermarkPosition { get; set; } = "BottomRight";
    public bool EnableWatermarking { get; set; } = false;
    public int LocalQualityMaxBitrate { get; set; } = 8000;
    public int RemoteQualityMaxBitrate { get; set; } = 2000;
    public bool EnableGeofencing { get; set; } = false;
    public bool EnableConnectionQualityAlerts { get; set; } = true;
    public DateTime? DirectorAccessExpiryDate { get; set; }
    public string? LanSubnet { get; set; } = "192.168.0.0/16";

    [StringLength(20)]
    public string HostingEnvironment { get; set; } = "Dev";

    [Range(1, 365)]
    public int TrustedDeviceDurationDays { get; set; } = 30;

    public bool EnforceVpn { get; set; } = false;
    public GFC.Core.Enums.AccessMode AccessMode { get; set; } = GFC.Core.Enums.AccessMode.Open;
    public bool EnableOnboarding { get; set; } = false;
    public bool SafeModeEnabled { get; set; } = false;
    public bool MagicLinkEnabled { get; set; } = true;
    public string PreferredMagicLinkMethod { get; set; } = "Email";
    public string SystemTimeZoneId { get; set; } = "Eastern Standard Time";
    public string BackupMethod { get; set; } = "External USB";
    public DateTime? LastSuccessfulBackupUtc { get; set; }
    public DateTime? LastRestoreTestUtc { get; set; }
    public int BackupFrequencyHours { get; set; } = 24;

    [StringLength(500)]
    public string BackupStoragePath { get; set; } = "C:\\GFC_Backups\\Sql\\";
    
    [Range(1, 100)]
    public int BackupRetentionCount { get; set; } = 10;
    
    public bool AllowServerRestoreOperations { get; set; } = false;
    public bool MaintenanceModeEnabled { get; set; } = false;
    public bool SmsEnabled { get; set; } = false;
    public bool EmailEnabled { get; set; } = false;
    public EmailProvider EmailProvider { get; set; } = EmailProvider.SMTP;
    public string? ResendApiKey { get; set; }
    public string? TwilioAccountSid { get; set; }
    public string? TwilioAuthToken { get; set; }
    public string? TwilioFromNumber { get; set; }
    public string? SmtpHost { get; set; }
    public int SmtpPort { get; set; } = 587;
    [Column("SmtpUser")]
    public string? SmtpUsername { get; set; }
    
    [Column("SmtpPass")]
    public string? SmtpPassword { get; set; }
    
    [Column("SmtpUseSsl")]
    public bool SmtpEnableSsl { get; set; } = true;
    public string? SmtpFromAddress { get; set; }
    public string? SmtpFromName { get; set; } = "GFC System";
    public bool PushEnabled { get; set; } = false;
    public string? VapidPublicKey { get; set; }
    public string? VapidPrivateKey { get; set; }
    public string? VapidSubject { get; set; }
    public TimeSpan DayShiftStartTime { get; set; } = new TimeSpan(9, 0, 0);
    public TimeSpan DayShiftEndTime { get; set; } = new TimeSpan(17, 0, 0);
    public TimeSpan NightShiftStartTime { get; set; } = new TimeSpan(18, 0, 0);
    public TimeSpan NightShiftEndTime { get; set; } = new TimeSpan(2, 0, 0);
    public bool LiquorEmailEnabled { get; set; } = false;
    public string? LiquorEmailSignature { get; set; }
    public string? LiquorEmailFooter { get; set; }
    public string? LiquorEmailCc { get; set; }
    public DateTime? LastSignInDrawExportUtc { get; set; }
    public decimal MaStateTaxRate { get; set; } = 5.0m;
    public decimal PfmlEmployeeRate { get; set; } = 0.35m;
    public decimal PfmlEmployerRate { get; set; } = 0.53m;
    public decimal FicaEmployeeRate { get; set; } = 7.65m;
    public decimal FicaEmployerRate { get; set; } = 7.65m;
    public decimal MaUnemploymentRate { get; set; } = 2.42m;
    public string? MaEmployerAccountNumber { get; set; }
    public string? FederalEmployerIdNumber { get; set; }
    public decimal GlobalLiquorPourSize { get; set; } = 1.5m;

    public decimal BingoBaseAdmissionPrice { get; set; } = 15.00m;
    public decimal BingoAdditionalCardPrice { get; set; } = 3.00m;
}
