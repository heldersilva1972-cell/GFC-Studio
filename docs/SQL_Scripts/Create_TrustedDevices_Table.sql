-- Create TrustedDevices table for device trust tokens
-- Run this on the ClubMembership database

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TrustedDevices')
BEGIN
    CREATE TABLE [dbo].[TrustedDevices] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [UserId] INT NOT NULL,
        [DeviceToken] NVARCHAR(128) NOT NULL,
        [UserAgent] NVARCHAR(256) NULL,
        [IpAddress] NVARCHAR(45) NULL,
        [LastUsedUtc] DATETIME2 NOT NULL,
        [ExpiresAtUtc] DATETIME2 NOT NULL,
        [IsRevoked] BIT NOT NULL DEFAULT 0,
        
        CONSTRAINT FK_TrustedDevices_AppUsers FOREIGN KEY ([UserId]) 
            REFERENCES [dbo].[AppUsers]([UserId]) ON DELETE CASCADE
    );

    -- Create unique index on DeviceToken for fast lookups
    CREATE UNIQUE INDEX IX_TrustedDevices_DeviceToken 
        ON [dbo].[TrustedDevices]([DeviceToken]);
    
    -- Create index on UserId for user device queries
    CREATE INDEX IX_TrustedDevices_UserId 
        ON [dbo].[TrustedDevices]([UserId]);

    PRINT 'TrustedDevices table created successfully';
END
ELSE
BEGIN
    PRINT 'TrustedDevices table already exists';
END
GO
