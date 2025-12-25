-- Create VpnProfiles table (without foreign key for now)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'VpnProfiles')
BEGIN
    CREATE TABLE [dbo].[VpnProfiles] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [UserId] INT NOT NULL,
        [ConfigFileContent] NVARCHAR(MAX) NULL,
        [PublicKey] NVARCHAR(500) NULL,
        [AllowedIPs] NVARCHAR(500) NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [LastUsedAt] DATETIME2 NULL,
        [RevokedAt] DATETIME2 NULL,
        [RevokedBy] INT NULL,
        [RevokedReason] NVARCHAR(500) NULL
    );
    PRINT 'Created VpnProfiles table';
END
ELSE
BEGIN
    PRINT 'VpnProfiles table already exists';
END
GO

-- Create AuthorizedUsers table (without foreign key for now)
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

PRINT 'All VPN-related tables have been created successfully!';
PRINT 'Note: Foreign keys omitted - add them manually if needed';
