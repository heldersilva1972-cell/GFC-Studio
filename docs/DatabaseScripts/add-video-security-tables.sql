USE [ClubMembership]
GO

PRINT 'Starting Video Security Tables Check...';

-- 1. VideoAccessAudits Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'VideoAccessAudits')
BEGIN
    PRINT 'Creating VideoAccessAudits table...';
    CREATE TABLE [dbo].[VideoAccessAudits] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [UserId] INT NOT NULL,
        [SessionStart] DATETIME2 NOT NULL,
        [SessionEnd] DATETIME2 NULL,
        [ClientIP] NVARCHAR(50) NOT NULL,
        [AccessType] NVARCHAR(50) NOT NULL, -- e.g., 'Live', 'Playback'
        [CameraName] NVARCHAR(100) NULL,
        [RecordingFile] NVARCHAR(255) NULL,
        [Success] BIT NOT NULL,
        [FailureReason] NVARCHAR(255) NULL,
        [ConnectionType] NVARCHAR(50) NOT NULL -- e.g., 'Local', 'VPN', 'Relay'
    );
    
    -- Add foreign key if AppUsers exists
    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'AppUsers')
    BEGIN
        ALTER TABLE [dbo].[VideoAccessAudits] ADD CONSTRAINT [FK_VideoAccessAudits_AppUsers] 
        FOREIGN KEY ([UserId]) REFERENCES [dbo].[AppUsers] ([UserId]) ON DELETE CASCADE;
    END
END
GO

-- 2. VpnSessions Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'VpnSessions')
BEGIN
    PRINT 'Creating VpnSessions table...';
    CREATE TABLE [dbo].[VpnSessions] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [UserId] INT NOT NULL,
        [VpnProfileId] INT NOT NULL,
        [ClientIP] NVARCHAR(50) NOT NULL,
        [ConnectedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [DisconnectedAt] DATETIME2 NULL,
        [BytesReceived] BIGINT NOT NULL DEFAULT 0,
        [BytesSent] BIGINT NOT NULL DEFAULT 0
    );
END
GO

-- 3. SecurityAlerts Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SecurityAlerts')
BEGIN
    PRINT 'Creating SecurityAlerts table...';
    CREATE TABLE [dbo].[SecurityAlerts] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [UserId] INT NULL, -- Can be null if system wide
        [AlertType] NVARCHAR(50) NOT NULL,
        [Severity] NVARCHAR(20) NOT NULL,
        [Message] NVARCHAR(MAX) NOT NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [IsResolved] BIT NOT NULL DEFAULT 0,
        [ResolvedAt] DATETIME2 NULL,
        [ResolvedBy] INT NULL,
        [Status] NVARCHAR(50) NOT NULL DEFAULT 'New',
        [ReviewedBy] INT NULL,
        [ReviewedAt] DATETIME2 NULL
    );
END
GO

-- 4. AuthorizedUsers Table (for remote access/video permissions)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AuthorizedUsers')
BEGIN
    PRINT 'Creating AuthorizedUsers table...';
    CREATE TABLE [dbo].[AuthorizedUsers] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [UserId] INT NOT NULL,
        [AccessLevel] NVARCHAR(50) NOT NULL DEFAULT 'View Only',
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
        [ExpiresAt] DATETIME2 NULL
    );
END
ELSE
BEGIN
    -- Repair existing table columns
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AuthorizedUsers]') AND name = 'CreatedAt')
        ALTER TABLE [dbo].[AuthorizedUsers] ADD [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE();
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AuthorizedUsers]') AND name = 'AccessLevel')
        ALTER TABLE [dbo].[AuthorizedUsers] ADD [AccessLevel] NVARCHAR(50) NOT NULL DEFAULT 'View Only';
END
GO

-- 5. VpnProfiles Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'VpnProfiles')
BEGIN
    PRINT 'Creating VpnProfiles table...';
    CREATE TABLE [dbo].[VpnProfiles] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [UserId] INT NOT NULL,
        [DeviceName] NVARCHAR(255) NOT NULL,
        [DeviceType] NVARCHAR(50) NULL,
        [PublicKey] NVARCHAR(255) NOT NULL,
        [PrivateKey] NVARCHAR(255) NULL,
        [AssignedIP] NVARCHAR(50) NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [LastUsedAt] DATETIME2 NULL,
        [RevokedAt] DATETIME2 NULL,
        [RevokedBy] INT NULL,
        [RevokedReason] NVARCHAR(500) NULL
    );
END
ELSE
BEGIN
     -- Repair existing table columns
     IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[VpnProfiles]') AND name = 'AssignedIP')
        ALTER TABLE [dbo].[VpnProfiles] ADD [AssignedIP] NVARCHAR(50) NULL;

     IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[VpnProfiles]') AND name = 'DeviceType')
        ALTER TABLE [dbo].[VpnProfiles] ADD [DeviceType] NVARCHAR(50) NULL;

     IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[VpnProfiles]') AND name = 'LastUsedAt')
        ALTER TABLE [dbo].[VpnProfiles] ADD [LastUsedAt] DATETIME2 NULL;
        
     IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[VpnProfiles]') AND name = 'RevokedAt')
        ALTER TABLE [dbo].[VpnProfiles] ADD [RevokedAt] DATETIME2 NULL;
END
GO

PRINT 'Video Security Tables Check Complete.';
GO
