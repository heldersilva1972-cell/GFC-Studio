-- Create VpnProfiles table with ALL required columns
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'VpnProfiles')
BEGIN
    CREATE TABLE [dbo].[VpnProfiles] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [UserId] INT NOT NULL,
        [DeviceName] NVARCHAR(255) NULL,
        [DeviceType] NVARCHAR(100) NULL,
        [AssignedIP] NVARCHAR(50) NULL,
        [ConfigFileContent] NVARCHAR(MAX) NULL,
        [PublicKey] NVARCHAR(500) NULL,
        [PrivateKey] NVARCHAR(500) NULL,
        [AllowedIPs] NVARCHAR(500) NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [LastUsedAt] DATETIME2 NULL,
        [RevokedAt] DATETIME2 NULL,
        [RevokedBy] INT NULL,
        [RevokedReason] NVARCHAR(500) NULL
    );
    PRINT 'Created VpnProfiles table with all columns';
END
ELSE
BEGIN
    PRINT 'VpnProfiles table already exists - checking for missing columns...';
    
    -- Add missing columns if table exists
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[VpnProfiles]') AND name = 'DeviceName')
        ALTER TABLE [dbo].[VpnProfiles] ADD [DeviceName] NVARCHAR(255) NULL;
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[VpnProfiles]') AND name = 'DeviceType')
        ALTER TABLE [dbo].[VpnProfiles] ADD [DeviceType] NVARCHAR(100) NULL;
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[VpnProfiles]') AND name = 'AssignedIP')
        ALTER TABLE [dbo].[VpnProfiles] ADD [AssignedIP] NVARCHAR(50) NULL;
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[VpnProfiles]') AND name = 'PrivateKey')
        ALTER TABLE [dbo].[VpnProfiles] ADD [PrivateKey] NVARCHAR(500) NULL;
    
    PRINT 'Added missing columns to VpnProfiles table';
END
GO

-- ENSURE AppUsers table has Identity and MFA columns REQUIRED by the model
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'AppUsers')
BEGIN
    PRINT 'Ensuring AppUsers table has required columns (Email, MFA)...';
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND name = 'Email')
        ALTER TABLE [dbo].[AppUsers] ADD [Email] NVARCHAR(255) NULL;
        
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND name = 'MfaEnabled')
        ALTER TABLE [dbo].[AppUsers] ADD [MfaEnabled] BIT NOT NULL DEFAULT 0;
        
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND name = 'MfaSecretKey')
        ALTER TABLE [dbo].[AppUsers] ADD [MfaSecretKey] NVARCHAR(500) NULL;
        
    PRINT 'AppUsers table updated successfully.';
END
ELSE
BEGIN
    PRINT 'WARNING: AppUsers table does not exist. Please run CreateAuthTables.sql first.';
END
GO

-- Create AuthorizedUsers table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AuthorizedUsers')
BEGIN
    CREATE TABLE [dbo].[AuthorizedUsers] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [UserId] INT NOT NULL,
        [AccessLevel] NVARCHAR(50) NOT NULL DEFAULT 'View Only',
        [ExpiresAt] DATETIME2 NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
    PRINT 'Created AuthorizedUsers table';
END
ELSE
BEGIN
    PRINT 'AuthorizedUsers table already exists';
END
GO

PRINT 'All VPN-related tables and columns have been created successfully!';
