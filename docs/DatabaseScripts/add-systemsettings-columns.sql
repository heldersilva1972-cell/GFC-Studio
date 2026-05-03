-- 1. SystemSettings Table Repair & Initialization
PRINT 'Starting SystemSettings repair...';

-- Step A: Add columns as NULLable first to avoid insertion errors
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'CloudflareTunnelToken')
    ALTER TABLE [dbo].[SystemSettings] ADD [CloudflareTunnelToken] NVARCHAR(500) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MaxSimultaneousViewers')
    ALTER TABLE [dbo].[SystemSettings] ADD [MaxSimultaneousViewers] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'PublicDomain')
    ALTER TABLE [dbo].[SystemSettings] ADD [PublicDomain] NVARCHAR(255) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardAllowedIPs')
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardAllowedIPs] NVARCHAR(500) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardPort')
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardPort] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardServerPublicKey')
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardServerPublicKey] NVARCHAR(500) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardSubnet')
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardSubnet] NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LanSubnet')
    ALTER TABLE [dbo].[SystemSettings] ADD [LanSubnet] NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NvrIpAddress')
    ALTER TABLE [dbo].[SystemSettings] ADD [NvrIpAddress] NVARCHAR(255) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NvrPort')
    ALTER TABLE [dbo].[SystemSettings] ADD [NvrPort] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NvrUsername')
    ALTER TABLE [dbo].[SystemSettings] ADD [NvrUsername] NVARCHAR(255) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NvrPassword')
    ALTER TABLE [dbo].[SystemSettings] ADD [NvrPassword] NVARCHAR(500) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'ScannerControllerId')
    ALTER TABLE [dbo].[SystemSettings] ADD [ScannerControllerId] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LastUpdatedUtc')
    ALTER TABLE [dbo].[SystemSettings] ADD [LastUpdatedUtc] DATETIME2 NULL;
GO

-- Step B: Ensure the row with Id=1 exists
IF NOT EXISTS (SELECT * FROM [dbo].[SystemSettings] WHERE Id = 1)
BEGIN
    SET IDENTITY_INSERT [dbo].[SystemSettings] ON;
    INSERT INTO [dbo].[SystemSettings] (Id) VALUES (1);
    SET IDENTITY_INSERT [dbo].[SystemSettings] OFF;
    PRINT 'Created initial SystemSettings row (Id=1)';
END
GO

-- Step C: Now add remaining missing columns
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'DirectorAccessExpiryDate')
    ALTER TABLE [dbo].[SystemSettings] ADD [DirectorAccessExpiryDate] DATETIME2 NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableConnectionQualityAlerts')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableConnectionQualityAlerts] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableFailedLoginProtection')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableFailedLoginProtection] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableGeofencing')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableGeofencing] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableIPFiltering')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableIPFiltering] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableSessionTimeout')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableSessionTimeout] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableTwoFactorAuth')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableTwoFactorAuth] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableWatermarking')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableWatermarking] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'IPFilterMode')
    ALTER TABLE [dbo].[SystemSettings] ADD [IPFilterMode] NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LocalQualityMaxBitrate')
    ALTER TABLE [dbo].[SystemSettings] ADD [LocalQualityMaxBitrate] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LoginLockDurationMinutes')
    ALTER TABLE [dbo].[SystemSettings] ADD [LoginLockDurationMinutes] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MaxFailedLoginAttempts')
    ALTER TABLE [dbo].[SystemSettings] ADD [MaxFailedLoginAttempts] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MinimumBandwidthMbps')
    ALTER TABLE [dbo].[SystemSettings] ADD [MinimumBandwidthMbps] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'RemoteQualityMaxBitrate')
    ALTER TABLE [dbo].[SystemSettings] ADD [RemoteQualityMaxBitrate] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SessionTimeoutMinutes')
    ALTER TABLE [dbo].[SystemSettings] ADD [SessionTimeoutMinutes] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WatermarkPosition')
    ALTER TABLE [dbo].[SystemSettings] ADD [WatermarkPosition] NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'AbsoluteSessionMaxMinutes')
    ALTER TABLE [dbo].[SystemSettings] ADD [AbsoluteSessionMaxMinutes] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'AccessMode')
    ALTER TABLE [dbo].[SystemSettings] ADD [AccessMode] NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'AllowedDomains')
    ALTER TABLE [dbo].[SystemSettings] ADD [AllowedDomains] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'BackupFrequencyHours')
    ALTER TABLE [dbo].[SystemSettings] ADD [BackupFrequencyHours] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'BackupMethod')
    ALTER TABLE [dbo].[SystemSettings] ADD [BackupMethod] NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'DomainSwitchExpiryUtc')
    ALTER TABLE [dbo].[SystemSettings] ADD [DomainSwitchExpiryUtc] DATETIME2 NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'DomainSwitchPending')
    ALTER TABLE [dbo].[SystemSettings] ADD [DomainSwitchPending] NVARCHAR(255) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableOnboarding')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableOnboarding] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnforceVpn')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnforceVpn] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'HostingEnvironment')
    ALTER TABLE [dbo].[SystemSettings] ADD [HostingEnvironment] NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'IdleTimeoutMinutes')
    ALTER TABLE [dbo].[SystemSettings] ADD [IdleTimeoutMinutes] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LastConfirmedDomain')
    ALTER TABLE [dbo].[SystemSettings] ADD [LastConfirmedDomain] NVARCHAR(255) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LastRestoreTestUtc')
    ALTER TABLE [dbo].[SystemSettings] ADD [LastRestoreTestUtc] DATETIME2 NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LastSuccessfulBackupUtc')
    ALTER TABLE [dbo].[SystemSettings] ADD [LastSuccessfulBackupUtc] DATETIME2 NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MagicLinkEnabled')
    ALTER TABLE [dbo].[SystemSettings] ADD [MagicLinkEnabled] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'PrimaryDomain')
    ALTER TABLE [dbo].[SystemSettings] ADD [PrimaryDomain] NVARCHAR(255) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SafeModeEnabled')
    ALTER TABLE [dbo].[SystemSettings] ADD [SafeModeEnabled] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SystemTimeZoneId')
    ALTER TABLE [dbo].[SystemSettings] ADD [SystemTimeZoneId] NVARCHAR(100) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'TrustedDeviceDurationDays')
    ALTER TABLE [dbo].[SystemSettings] ADD [TrustedDeviceDurationDays] INT NULL;

-- Backup System Enhancements
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'BackupStoragePath')
    ALTER TABLE [dbo].[SystemSettings] ADD [BackupStoragePath] NVARCHAR(500) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'BackupRetentionCount')
    ALTER TABLE [dbo].[SystemSettings] ADD [BackupRetentionCount] INT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'AllowServerRestoreOperations')
    ALTER TABLE [dbo].[SystemSettings] ADD [AllowServerRestoreOperations] BIT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MaintenanceModeEnabled')
    ALTER TABLE [dbo].[SystemSettings] ADD [MaintenanceModeEnabled] BIT NULL;
GO

-- 2. Communication & Push Notification Columns
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EmailEnabled')
    ALTER TABLE [dbo].[SystemSettings] ADD [EmailEnabled] BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SmsEnabled')
    ALTER TABLE [dbo].[SystemSettings] ADD [SmsEnabled] BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'PushEnabled')
    ALTER TABLE [dbo].[SystemSettings] ADD [PushEnabled] BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'VapidPublicKey')
    ALTER TABLE [dbo].[SystemSettings] ADD [VapidPublicKey] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'VapidPrivateKey')
    ALTER TABLE [dbo].[SystemSettings] ADD [VapidPrivateKey] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'VapidSubject')
    ALTER TABLE [dbo].[SystemSettings] ADD [VapidSubject] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'PreferredMagicLinkMethod')
    ALTER TABLE [dbo].[SystemSettings] ADD [PreferredMagicLinkMethod] NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'TwilioAccountSid')
    ALTER TABLE [dbo].[SystemSettings] ADD [TwilioAccountSid] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'TwilioAuthToken')
    ALTER TABLE [dbo].[SystemSettings] ADD [TwilioAuthToken] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'TwilioFromNumber')
    ALTER TABLE [dbo].[SystemSettings] ADD [TwilioFromNumber] NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EmailProvider')
    ALTER TABLE [dbo].[SystemSettings] ADD [EmailProvider] INT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'ResendApiKey')
    ALTER TABLE [dbo].[SystemSettings] ADD [ResendApiKey] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SmtpHost')
    ALTER TABLE [dbo].[SystemSettings] ADD [SmtpHost] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SmtpPort')
    ALTER TABLE [dbo].[SystemSettings] ADD [SmtpPort] INT NULL;

-- Correction: SmtpUsername/Password/EnableSsl/FromName to match model
-- Legacy migration cleanup FIRST to avoid conflicts
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SmtpUser')
   AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SmtpUsername')
    EXEC sp_rename 'SystemSettings.SmtpUser', 'SmtpUsername', 'COLUMN';

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SmtpPass')
   AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SmtpPassword')
    EXEC sp_rename 'SystemSettings.SmtpPass', 'SmtpPassword', 'COLUMN';

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SmtpUseSsl')
   AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SmtpEnableSsl')
    EXEC sp_rename 'SystemSettings.SmtpUseSsl', 'SmtpEnableSsl', 'COLUMN';

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SmtpFromDisplayName')
   AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SmtpFromName')
    EXEC sp_rename 'SystemSettings.SmtpFromDisplayName', 'SmtpFromName', 'COLUMN';

-- Add columns if still missing (either rename didn't happen or they weren't there)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SmtpUsername')
    ALTER TABLE [dbo].[SystemSettings] ADD [SmtpUsername] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SmtpPassword')
    ALTER TABLE [dbo].[SystemSettings] ADD [SmtpPassword] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SmtpFromAddress')
    ALTER TABLE [dbo].[SystemSettings] ADD [SmtpFromAddress] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SmtpFromName')
    ALTER TABLE [dbo].[SystemSettings] ADD [SmtpFromName] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SmtpEnableSsl')
    ALTER TABLE [dbo].[SystemSettings] ADD [SmtpEnableSsl] BIT NOT NULL DEFAULT 1;
GO

-- 3. Shift and Payroll Tracking Columns
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'DayShiftStartTime')
    ALTER TABLE [dbo].[SystemSettings] ADD [DayShiftStartTime] TIME NOT NULL DEFAULT '09:00:00';

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'DayShiftEndTime')
    ALTER TABLE [dbo].[SystemSettings] ADD [DayShiftEndTime] TIME NOT NULL DEFAULT '17:00:00';

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NightShiftStartTime')
    ALTER TABLE [dbo].[SystemSettings] ADD [NightShiftStartTime] TIME NOT NULL DEFAULT '18:00:00';

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NightShiftEndTime')
    ALTER TABLE [dbo].[SystemSettings] ADD [NightShiftEndTime] TIME NOT NULL DEFAULT '02:00:00';

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LiquorEmailEnabled')
    ALTER TABLE [dbo].[SystemSettings] ADD [LiquorEmailEnabled] BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LiquorEmailSignature')
    ALTER TABLE [dbo].[SystemSettings] ADD [LiquorEmailSignature] NVARCHAR(MAX) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LastSignInDrawExportUtc')
    ALTER TABLE [dbo].[SystemSettings] ADD [LastSignInDrawExportUtc] DATETIME2 NULL;

-- Tax and Payroll Rates
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MaStateTaxRate')
    ALTER TABLE [dbo].[SystemSettings] ADD [MaStateTaxRate] DECIMAL(18,2) NOT NULL DEFAULT 5.0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'PfmlEmployeeRate')
    ALTER TABLE [dbo].[SystemSettings] ADD [PfmlEmployeeRate] DECIMAL(18,2) NOT NULL DEFAULT 0.35;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'PfmlEmployerRate')
    ALTER TABLE [dbo].[SystemSettings] ADD [PfmlEmployerRate] DECIMAL(18,2) NOT NULL DEFAULT 0.53;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'FicaEmployeeRate')
    ALTER TABLE [dbo].[SystemSettings] ADD [FicaEmployeeRate] DECIMAL(18,2) NOT NULL DEFAULT 7.65;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'FicaEmployerRate')
    ALTER TABLE [dbo].[SystemSettings] ADD [FicaEmployerRate] DECIMAL(18,2) NOT NULL DEFAULT 7.65;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MaUnemploymentRate')
    ALTER TABLE [dbo].[SystemSettings] ADD [MaUnemploymentRate] DECIMAL(18,2) NOT NULL DEFAULT 2.42;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MaEmployerAccountNumber')
    ALTER TABLE [dbo].[SystemSettings] ADD [MaEmployerAccountNumber] NVARCHAR(255) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'FederalEmployerIdNumber')
    ALTER TABLE [dbo].[SystemSettings] ADD [FederalEmployerIdNumber] NVARCHAR(255) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'GlobalLiquorPourSize')
    ALTER TABLE [dbo].[SystemSettings] ADD [GlobalLiquorPourSize] DECIMAL(18,2) NOT NULL DEFAULT 1.5;
GO

-- Step D: Populate existing NULLs with defaults
PRINT 'Populating defaults...';
UPDATE [dbo].[SystemSettings] SET
    [MaxSimultaneousViewers] = ISNULL([MaxSimultaneousViewers], 10),
    [WireGuardAllowedIPs] = ISNULL([WireGuardAllowedIPs], '10.8.0.0/24, 192.168.1.0/24'),
    [WireGuardPort] = ISNULL([WireGuardPort], 51820),
    [WireGuardSubnet] = ISNULL([WireGuardSubnet], '10.8.0.0/24'),
    [EnableConnectionQualityAlerts] = ISNULL([EnableConnectionQualityAlerts], 1),
    [EnableFailedLoginProtection] = ISNULL([EnableFailedLoginProtection], 1),
    [EnableGeofencing] = ISNULL([EnableGeofencing], 0),
    [EnableIPFiltering] = ISNULL([EnableIPFiltering], 0),
    [EnableSessionTimeout] = ISNULL([EnableSessionTimeout], 1),
    [EnableTwoFactorAuth] = ISNULL([EnableTwoFactorAuth], 0),
    [EnableWatermarking] = ISNULL([EnableWatermarking], 0),
    [IPFilterMode] = ISNULL([IPFilterMode], 'Whitelist'),
    [LocalQualityMaxBitrate] = ISNULL([LocalQualityMaxBitrate], 8000),
    [LoginLockDurationMinutes] = ISNULL([LoginLockDurationMinutes], 30),
    [MaxFailedLoginAttempts] = ISNULL([MaxFailedLoginAttempts], 5),
    [MinimumBandwidthMbps] = ISNULL([MinimumBandwidthMbps], 5),
    [RemoteQualityMaxBitrate] = ISNULL([RemoteQualityMaxBitrate], 2000),
    [SessionTimeoutMinutes] = ISNULL([SessionTimeoutMinutes], 30),
    [WatermarkPosition] = ISNULL([WatermarkPosition], 'BottomRight'),
    [LanSubnet] = ISNULL([LanSubnet], '192.168.1.0/24'),
    
    -- New Columns Defaults
    [AbsoluteSessionMaxMinutes] = ISNULL([AbsoluteSessionMaxMinutes], 1440),
    [AccessMode] = ISNULL([AccessMode], 'Open'),
    [AllowedDomains] = ISNULL([AllowedDomains], ''),
    [BackupFrequencyHours] = ISNULL([BackupFrequencyHours], 24),
    [BackupMethod] = ISNULL([BackupMethod], 'Native'),
    [EnableOnboarding] = ISNULL([EnableOnboarding], 0),
    [EnforceVpn] = ISNULL([EnforceVpn], 0),
    [HostingEnvironment] = ISNULL([HostingEnvironment], 'Production'),
    [IdleTimeoutMinutes] = ISNULL([IdleTimeoutMinutes], 60),
    [MagicLinkEnabled] = ISNULL([MagicLinkEnabled], 1),
    [SafeModeEnabled] = ISNULL([SafeModeEnabled], 0),
    [SystemTimeZoneId] = ISNULL([SystemTimeZoneId], 'Eastern Standard Time'),
    [TrustedDeviceDurationDays] = ISNULL([TrustedDeviceDurationDays], 30),
    
    -- Communication Defaults
    [EmailEnabled] = ISNULL([EmailEnabled], 0),
    [SmsEnabled] = ISNULL([SmsEnabled], 0),
    [PushEnabled] = ISNULL([PushEnabled], 0),
    [PreferredMagicLinkMethod] = ISNULL([PreferredMagicLinkMethod], 'Email'),
    [SmtpPort] = ISNULL([SmtpPort], 587),
    [SmtpEnableSsl] = ISNULL([SmtpEnableSsl], 1),
    [BackupRetentionCount] = ISNULL([BackupRetentionCount], 10),
    [AllowServerRestoreOperations] = ISNULL([AllowServerRestoreOperations], 0),
    [MaintenanceModeEnabled] = ISNULL([MaintenanceModeEnabled], 0)
WHERE Id = 1;
GO

-- Step E: Enforce NOT NULL constraints where required
PRINT 'Enforcing NOT NULL constraints...';
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [MaxSimultaneousViewers] INT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [WireGuardAllowedIPs] NVARCHAR(500) NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [WireGuardPort] INT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [WireGuardSubnet] NVARCHAR(50) NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [EnableConnectionQualityAlerts] BIT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [EnableFailedLoginProtection] BIT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [EnableGeofencing] BIT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [EnableIPFiltering] BIT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [EnableSessionTimeout] BIT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [EnableTwoFactorAuth] BIT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [EnableWatermarking] BIT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [IPFilterMode] NVARCHAR(50) NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [LocalQualityMaxBitrate] INT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [LoginLockDurationMinutes] INT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [MaxFailedLoginAttempts] INT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [MinimumBandwidthMbps] INT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [RemoteQualityMaxBitrate] INT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [SessionTimeoutMinutes] INT NOT NULL;
ALTER TABLE [dbo].[SystemSettings] ALTER COLUMN [WatermarkPosition] NVARCHAR(50) NOT NULL;
GO

-- 2. WebsiteSettings Table Repair
PRINT 'Checking WebsiteSettings table...';
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'WebsiteSettings')
BEGIN
    CREATE TABLE [dbo].[WebsiteSettings] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [ClubPhone] NVARCHAR(MAX) NULL,
        [ClubAddress] NVARCHAR(MAX) NULL,
        [MasterEmailKillSwitch] BIT NOT NULL DEFAULT 0,
        [MemberRate] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [NonMemberRate] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [PrimaryColor] NVARCHAR(20) NOT NULL DEFAULT '#0D1B2A',
        [SecondaryColor] NVARCHAR(20) NOT NULL DEFAULT '#FFD700',
        [HeadingFont] NVARCHAR(100) NOT NULL DEFAULT 'Outfit',
        [BodyFont] NVARCHAR(100) NOT NULL DEFAULT 'Inter',
        [HighAccessibilityMode] BIT NOT NULL DEFAULT 0
    );
END
GO

-- [AUTO-FIX] Ensure new columns exist even if table was already created
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'IsClubOpen')
    ALTER TABLE [dbo].[WebsiteSettings] ADD [IsClubOpen] BIT NOT NULL DEFAULT 1;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SeoTitle')
    ALTER TABLE [dbo].[WebsiteSettings] ADD [SeoTitle] NVARCHAR(200) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SeoDescription')
    ALTER TABLE [dbo].[WebsiteSettings] ADD [SeoDescription] NVARCHAR(500) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SeoKeywords')
    ALTER TABLE [dbo].[WebsiteSettings] ADD [SeoKeywords] NVARCHAR(500) NULL;

-- Hall Rental Payment Settings
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'EnableOnlineRentalsPayment')
    ALTER TABLE [dbo].[WebsiteSettings] ADD [EnableOnlineRentalsPayment] BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'PaymentGatewayUrl')
    ALTER TABLE [dbo].[WebsiteSettings] ADD [PaymentGatewayUrl] NVARCHAR(500) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'PaymentGatewayApiKey')
    ALTER TABLE [dbo].[WebsiteSettings] ADD [PaymentGatewayApiKey] NVARCHAR(500) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'MaxHallRentalDurationHours')
    ALTER TABLE [dbo].[WebsiteSettings] ADD [MaxHallRentalDurationHours] INT NULL DEFAULT 8;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'AdditionalHourRate')
    ALTER TABLE [dbo].[WebsiteSettings] ADD [AdditionalHourRate] DECIMAL(18,2) NOT NULL DEFAULT 0;

PRINT '✓ Verified/Updated WebsiteSettings table';

IF NOT EXISTS (SELECT * FROM [dbo].[WebsiteSettings])
BEGIN
    INSERT INTO [dbo].[WebsiteSettings] (ClubPhone, PrimaryColor) VALUES ('978-283-0507', '#0D1B2A');
END
GO

-- 3. ProtectedDocuments Table Repair
PRINT 'Checking ProtectedDocuments table...';
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProtectedDocuments')
BEGIN
    CREATE TABLE [dbo].[ProtectedDocuments] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [FileName] NVARCHAR(255) NOT NULL,
        [Description] NVARCHAR(500) NULL,
        [ContentType] NVARCHAR(100) NOT NULL,
        [FilePath] NVARCHAR(1024) NOT NULL,
        [Visibility] NVARCHAR(50) NOT NULL,
        [UploadedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

-- 4. AppUsers MFA and Email Columns Fix
PRINT 'Checking AppUsers table for MFA and Email columns...';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND name = 'Email')
BEGIN
    ALTER TABLE [dbo].[AppUsers] ADD [Email] NVARCHAR(255) NULL;
    PRINT 'Added Email column to AppUsers';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND name = 'MfaEnabled')
BEGIN
    ALTER TABLE [dbo].[AppUsers] ADD [MfaEnabled] BIT NOT NULL DEFAULT 0;
    PRINT 'Added MfaEnabled column to AppUsers';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND name = 'MfaSecretKey')
BEGIN
    ALTER TABLE [dbo].[AppUsers] ADD [MfaSecretKey] NVARCHAR(MAX) NULL;
    PRINT 'Added MfaSecretKey column to AppUsers';
END
GO

-- 5. Data Sanitization for WebsiteSettings (Fix 'Data is Null' errors)
PRINT 'Sanitizing WebsiteSettings data...';
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'WebsiteSettings')
BEGIN
    UPDATE [dbo].[WebsiteSettings] SET [MemberRate] = 0 WHERE [MemberRate] IS NULL;
    UPDATE [dbo].[WebsiteSettings] SET [NonMemberRate] = 0 WHERE [NonMemberRate] IS NULL;
    UPDATE [dbo].[WebsiteSettings] SET [IsClubOpen] = 1 WHERE [IsClubOpen] IS NULL;
    UPDATE [dbo].[WebsiteSettings] SET [MasterEmailKillSwitch] = 0 WHERE [MasterEmailKillSwitch] IS NULL;
    UPDATE [dbo].[WebsiteSettings] SET [HighAccessibilityMode] = 0 WHERE [HighAccessibilityMode] IS NULL;
    
    -- Ensure colors have defaults if missing
    UPDATE [dbo].[WebsiteSettings] SET [PrimaryColor] = '#0D1B2A' WHERE [PrimaryColor] IS NULL;
    UPDATE [dbo].[WebsiteSettings] SET [SecondaryColor] = '#FFD700' WHERE [SecondaryColor] IS NULL;
    UPDATE [dbo].[WebsiteSettings] SET [HeadingFont] = 'Outfit' WHERE [HeadingFont] IS NULL;
    UPDATE [dbo].[WebsiteSettings] SET [BodyFont] = 'Inter' WHERE [BodyFont] IS NULL;
END
GO

PRINT 'Database fix successfully completed!';
GO
