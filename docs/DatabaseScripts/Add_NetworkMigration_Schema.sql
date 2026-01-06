-- =============================================
-- Network Migration Wizard - Database Schema
-- =============================================

-- Step 1: Create NetworkMigrations table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'NetworkMigrations')
BEGIN
    CREATE TABLE NetworkMigrations (
        Id INT PRIMARY KEY IDENTITY(1,1),
        ControllerId INT NOT NULL,
        MigrationType NVARCHAR(50) NOT NULL,
        PreviousNetworkType NVARCHAR(50),
        PreviousIpAddress NVARCHAR(50),
        PreviousPort INT,
        PreviousVpnProfileId INT NULL,
        NewNetworkType NVARCHAR(50),
        NewIpAddress NVARCHAR(50),
        NewPort INT,
        NewVpnProfileId INT NULL,
        InitiatedUtc DATETIME NOT NULL,
        CompletedUtc DATETIME NULL,
        Status NVARCHAR(50) NOT NULL,
        InitiatedByUser NVARCHAR(100) NOT NULL,
        TestResultsJson NVARCHAR(MAX),
        ErrorMessage NVARCHAR(MAX),
        BackupExpiresUtc DATETIME NULL,
        CanRollback BIT NOT NULL DEFAULT 1,
        Notes NVARCHAR(MAX),
        CONSTRAINT FK_NetworkMigrations_Controllers FOREIGN KEY (ControllerId) REFERENCES Controllers(Id) ON DELETE CASCADE
    );

    PRINT 'Created NetworkMigrations table';
END
ELSE
BEGIN
    PRINT 'NetworkMigrations table already exists';
END
GO

-- Step 2: Add columns to Controllers table if they don't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Controllers') AND name = 'NetworkType')
BEGIN
    ALTER TABLE Controllers ADD NetworkType NVARCHAR(50) DEFAULT 'LAN';
    PRINT 'Added NetworkType column to Controllers';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Controllers') AND name = 'VpnProfileId')
BEGIN
    ALTER TABLE Controllers ADD VpnProfileId INT NULL;
    PRINT 'Added VpnProfileId column to Controllers';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Controllers') AND name = 'BackupIpAddress')
BEGIN
    ALTER TABLE Controllers ADD BackupIpAddress NVARCHAR(50) NULL;
    PRINT 'Added BackupIpAddress column to Controllers';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Controllers') AND name = 'BackupPort')
BEGIN
    ALTER TABLE Controllers ADD BackupPort INT NULL;
    PRINT 'Added BackupPort column to Controllers';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Controllers') AND name = 'BackupExpiresUtc')
BEGIN
    ALTER TABLE Controllers ADD BackupExpiresUtc DATETIME NULL;
    PRINT 'Added BackupExpiresUtc column to Controllers';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Controllers') AND name = 'LastMigrationUtc')
BEGIN
    ALTER TABLE Controllers ADD LastMigrationUtc DATETIME NULL;
    PRINT 'Added LastMigrationUtc column to Controllers';
END
GO

-- Step 3: Create indexes for performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_NetworkMigrations_ControllerId')
BEGIN
    CREATE INDEX IX_NetworkMigrations_ControllerId ON NetworkMigrations(ControllerId);
    PRINT 'Created index IX_NetworkMigrations_ControllerId';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_NetworkMigrations_Status')
BEGIN
    CREATE INDEX IX_NetworkMigrations_Status ON NetworkMigrations(Status);
    PRINT 'Created index IX_NetworkMigrations_Status';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_NetworkMigrations_InitiatedUtc')
BEGIN
    CREATE INDEX IX_NetworkMigrations_InitiatedUtc ON NetworkMigrations(InitiatedUtc);
    PRINT 'Created index IX_NetworkMigrations_InitiatedUtc';
END
GO

-- Step 4: Update existing controllers to have default NetworkType
UPDATE Controllers 
SET NetworkType = 'LAN' 
WHERE NetworkType IS NULL;
GO

PRINT 'Network Migration schema installation complete!';
