USE [ClubMembership];
GO

-- 1. Ensure IsEnabled exists on Controllers
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Controllers]') AND name = 'IsEnabled')
BEGIN
    ALTER TABLE [dbo].[Controllers] ADD [IsEnabled] BIT NOT NULL DEFAULT 1;
END
GO

-- 2. ControllerNetworkConfigs
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ControllerNetworkConfigs]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ControllerNetworkConfigs](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [ControllerId] [int] NOT NULL,
        [IpAddress] [nvarchar](45) NULL,
        [SubnetMask] [nvarchar](45) NULL,
        [Gateway] [nvarchar](45) NULL,
        [Port] [int] NULL,
        [DhcpEnabled] [bit] NOT NULL,
        [AllowedPcIp] [nvarchar](45) NULL,
        [CommPasswordMasked] [nvarchar](50) NULL,
        [LastReadUtc] [datetime2](7) NULL,
        [LastSyncUtc] [datetime2](7) NULL,
        [CreatedUtc] [datetime2](7) NOT NULL,
        [UpdatedUtc] [datetime2](7) NULL,
        CONSTRAINT [PK_ControllerNetworkConfigs] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    CREATE UNIQUE INDEX [IX_ControllerNetworkConfigs_ControllerId] ON [dbo].[ControllerNetworkConfigs] ([ControllerId]);
    
    ALTER TABLE [dbo].[ControllerNetworkConfigs]  WITH CHECK ADD  CONSTRAINT [FK_ControllerNetworkConfigs_Controllers_ControllerId] FOREIGN KEY([ControllerId])
    REFERENCES [dbo].[Controllers] ([Id])
    ON DELETE CASCADE;
END
GO

-- 3. ControllerCommandLogs
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ControllerCommandLogs]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ControllerCommandLogs](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [ControllerId] [int] NOT NULL,
        [Action] [nvarchar](100) NOT NULL,
        [Success] [bit] NOT NULL,
        [Error] [nvarchar](2000) NULL,
        [LatencyMs] [int] NULL,
        [TimestampUtc] [datetime2](7) NOT NULL,
        CONSTRAINT [PK_ControllerCommandLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    CREATE INDEX [IX_ControllerCommandLogs_ControllerId] ON [dbo].[ControllerCommandLogs] ([ControllerId]);
    CREATE INDEX [IX_ControllerCommandLogs_TimestampUtc] ON [dbo].[ControllerCommandLogs] ([TimestampUtc]);

    ALTER TABLE [dbo].[ControllerCommandLogs]  WITH CHECK ADD  CONSTRAINT [FK_ControllerCommandLogs_Controllers_ControllerId] FOREIGN KEY([ControllerId])
    REFERENCES [dbo].[Controllers] ([Id])
    ON DELETE CASCADE;
END
GO

-- 4. ControllerBehaviorOptions
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ControllerBehaviorOptions]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ControllerBehaviorOptions](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [ControllerId] [int] NOT NULL,
        [ValidSwipeGapSeconds] [int] NOT NULL,
        [CreatedUtc] [datetime2](7) NOT NULL,
        [UpdatedUtc] [datetime2](7) NULL,
        CONSTRAINT [PK_ControllerBehaviorOptions] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    CREATE UNIQUE INDEX [IX_ControllerBehaviorOptions_ControllerId] ON [dbo].[ControllerBehaviorOptions] ([ControllerId]);

    ALTER TABLE [dbo].[ControllerBehaviorOptions]  WITH CHECK ADD  CONSTRAINT [FK_ControllerBehaviorOptions_Controllers_ControllerId] FOREIGN KEY([ControllerId])
    REFERENCES [dbo].[Controllers] ([Id])
    ON DELETE CASCADE;
END
GO

-- 5. DoorBehaviorOptions
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DoorBehaviorOptions]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DoorBehaviorOptions](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [DoorId] [int] NOT NULL,
        [FirstCardOpenEnabled] [bit] NOT NULL,
        [DoorAsSwitchEnabled] [bit] NOT NULL,
        [OpenTooLongWarnEnabled] [bit] NOT NULL,
        [Invalid3CardsWarnEnabled] [bit] NOT NULL,
        [CreatedUtc] [datetime2](7) NOT NULL,
        [UpdatedUtc] [datetime2](7) NULL,
        CONSTRAINT [PK_DoorBehaviorOptions] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    CREATE UNIQUE INDEX [IX_DoorBehaviorOptions_DoorId] ON [dbo].[DoorBehaviorOptions] ([DoorId]);

    ALTER TABLE [dbo].[DoorBehaviorOptions]  WITH CHECK ADD  CONSTRAINT [FK_DoorBehaviorOptions_Doors_DoorId] FOREIGN KEY([DoorId])
    REFERENCES [dbo].[Doors] ([Id])
    ON DELETE CASCADE;
END
GO

-- 6. DoorConfigs
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DoorConfigs]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DoorConfigs](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [DoorId] [int] NOT NULL,
        [OpenTimeSeconds] [int] NOT NULL,
        [LockDelaySeconds] [int] NOT NULL,
        [AlarmEnabled] [bit] NOT NULL,
        [CreatedUtc] [datetime2](7) NOT NULL,
        [UpdatedUtc] [datetime2](7) NULL,
        CONSTRAINT [PK_DoorConfigs] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    CREATE UNIQUE INDEX [IX_DoorConfigs_DoorId] ON [dbo].[DoorConfigs] ([DoorId]);

    ALTER TABLE [dbo].[DoorConfigs]  WITH CHECK ADD  CONSTRAINT [FK_DoorConfigs_Doors_DoorId] FOREIGN KEY([DoorId])
    REFERENCES [dbo].[Doors] ([Id])
    ON DELETE CASCADE;
END
GO

-- 7. DoorAutoOpenSchedules
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DoorAutoOpenSchedules]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DoorAutoOpenSchedules](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [DoorId] [int] NOT NULL,
        [TimeProfileId] [int] NULL,
        [IsActive] [bit] NOT NULL,
        [Description] [nvarchar](256) NULL,
        [ControllerProfileIndex] [int] NULL,
        [CreatedUtc] [datetime2](7) NOT NULL,
        [UpdatedUtc] [datetime2](7) NULL,
        CONSTRAINT [PK_DoorAutoOpenSchedules] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    CREATE UNIQUE INDEX [IX_DoorAutoOpenSchedules_DoorId] ON [dbo].[DoorAutoOpenSchedules] ([DoorId]);

    ALTER TABLE [dbo].[DoorAutoOpenSchedules]  WITH CHECK ADD  CONSTRAINT [FK_DoorAutoOpenSchedules_Doors_DoorId] FOREIGN KEY([DoorId])
    REFERENCES [dbo].[Doors] ([Id])
    ON DELETE CASCADE;
END
GO

-- 8. Sync ControllerCommandInfos Data
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ControllerCommandInfos]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ControllerCommandInfos](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Key] [nvarchar](100) NOT NULL,
        [DisplayName] [nvarchar](200) NOT NULL,
        [Category] [nvarchar](100) NULL,
        [Phase] [nvarchar](50) NULL,
        [ShortDescription] [nvarchar](500) NULL,
        [LongDescription] [nvarchar](max) NULL,
        [RiskLevel] [int] NOT NULL,
        [IsReadOperation] [bit] NOT NULL,
        [IsWriteOperation] [bit] NOT NULL,
        [ProtocolInfo] [nvarchar](500) NULL,
        [Enabled] [bit] NOT NULL,
        CONSTRAINT [PK_ControllerCommandInfos] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    CREATE UNIQUE INDEX [IX_ControllerCommandInfos_Key] ON [dbo].[ControllerCommandInfos] ([Key]);
END
GO

-- Seed SyncNetworkConfig if missing
IF NOT EXISTS (SELECT 1 FROM [ControllerCommandInfos] WHERE [Key] = 'SyncNetworkConfig')
BEGIN
    INSERT INTO [dbo].[ControllerCommandInfos] ([Key], [DisplayName], [Category], [Phase], [ShortDescription], [LongDescription], [RiskLevel], [IsReadOperation], [IsWriteOperation], [ProtocolInfo], [Enabled])
    VALUES ('SyncNetworkConfig', 'Sync Network Configuration', 'Configuration', 'Phase 3B', 'Updates controller IP, gateway, and security ports.', 'Writes network and password settings to the controller hardware.', 2, 0, 1, 'Agent POST /{sn}/network-config', 1);
END
GO

-- 9. Rename Controller
UPDATE [Controllers] 
SET [Name] = 'Main Controller' 
WHERE [Name] LIKE '%Simulation%';
GO

-- 10. Fix SystemSettings
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableTwoFactorAuth')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableTwoFactorAuth] BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableIPFiltering')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableIPFiltering] BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MinimumBandwidthMbps')
    ALTER TABLE [dbo].[SystemSettings] ADD [MinimumBandwidthMbps] INT NOT NULL DEFAULT 5;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableSessionTimeout')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableSessionTimeout] BIT NOT NULL DEFAULT 1;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'SessionTimeoutMinutes')
    ALTER TABLE [dbo].[SystemSettings] ADD [SessionTimeoutMinutes] INT NOT NULL DEFAULT 30;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableFailedLoginProtection')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableFailedLoginProtection] BIT NOT NULL DEFAULT 1;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MaxFailedLoginAttempts')
    ALTER TABLE [dbo].[SystemSettings] ADD [MaxFailedLoginAttempts] INT NOT NULL DEFAULT 5;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'IPFilterMode')
    ALTER TABLE [dbo].[SystemSettings] ADD [IPFilterMode] NVARCHAR(50) NOT NULL DEFAULT 'Whitelist';

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LoginLockDurationMinutes')
    ALTER TABLE [dbo].[SystemSettings] ADD [LoginLockDurationMinutes] INT NOT NULL DEFAULT 30;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WatermarkPosition')
    ALTER TABLE [dbo].[SystemSettings] ADD [WatermarkPosition] NVARCHAR(50) NOT NULL DEFAULT 'BottomRight';

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableWatermarking')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableWatermarking] BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LocalQualityMaxBitrate')
    ALTER TABLE [dbo].[SystemSettings] ADD [LocalQualityMaxBitrate] INT NOT NULL DEFAULT 8000;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'RemoteQualityMaxBitrate')
    ALTER TABLE [dbo].[SystemSettings] ADD [RemoteQualityMaxBitrate] INT NOT NULL DEFAULT 2000;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableGeofencing')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableGeofencing] BIT NOT NULL DEFAULT 0;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'EnableConnectionQualityAlerts')
    ALTER TABLE [dbo].[SystemSettings] ADD [EnableConnectionQualityAlerts] BIT NOT NULL DEFAULT 1;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'DirectorAccessExpiryDate')
    ALTER TABLE [dbo].[SystemSettings] ADD [DirectorAccessExpiryDate] DATETIME2(7) NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LanSubnet')
    ALTER TABLE [dbo].[SystemSettings] ADD [LanSubnet] NVARCHAR(50) NULL DEFAULT '192.168.1.0/24';
GO

-- Ensure there is at least one row in SystemSettings
IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemSettings])
BEGIN
    INSERT INTO [dbo].[SystemSettings] (Id) VALUES (1);
END
GO

PRINT 'Database sync completed successfully.';
GO
