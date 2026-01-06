-- =============================================
-- Database Maintenance Feature Schema
-- =============================================
-- This script creates the necessary tables and columns for the Database Maintenance feature.
-- Run this script manually on your database before using the Database Maintenance page.

USE ClubMembership;
GO

PRINT 'Starting Database Maintenance schema setup...';
GO

-- =============================================
-- 1. Create DatabaseBackups table
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DatabaseBackups')
BEGIN
    PRINT 'Creating DatabaseBackups table...';
    
    CREATE TABLE DatabaseBackups (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        FileName NVARCHAR(255) NOT NULL,
        FilePath NVARCHAR(500) NOT NULL,
        FileSizeBytes BIGINT NOT NULL,
        CreatedAtUtc DATETIME2 NOT NULL,
        CreatedByUserId INT NOT NULL,
        BackupType NVARCHAR(50) NOT NULL DEFAULT 'Manual',
        FileHash NVARCHAR(64) NOT NULL,
        IsDeleted BIT NOT NULL DEFAULT 0,
        Notes NVARCHAR(500) NULL,
        CONSTRAINT FK_DatabaseBackups_AppUsers FOREIGN KEY (CreatedByUserId) 
            REFERENCES AppUsers(UserId) ON DELETE NO ACTION
    );
    
    CREATE INDEX IX_DatabaseBackups_CreatedAtUtc ON DatabaseBackups(CreatedAtUtc DESC);
    CREATE INDEX IX_DatabaseBackups_IsDeleted ON DatabaseBackups(IsDeleted) WHERE IsDeleted = 0;
    
    PRINT 'DatabaseBackups table created successfully.';
END
ELSE
BEGIN
    PRINT 'DatabaseBackups table already exists.';
END
GO

-- =============================================
-- 2. Create DatabaseOperations table
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DatabaseOperations')
BEGIN
    PRINT 'Creating DatabaseOperations table...';
    
    CREATE TABLE DatabaseOperations (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        OperationType NVARCHAR(50) NOT NULL,
        Status NVARCHAR(50) NOT NULL DEFAULT 'Running',
        StartedAtUtc DATETIME2 NOT NULL,
        CompletedAtUtc DATETIME2 NULL,
        StartedByUserId INT NOT NULL,
        ErrorMessage NVARCHAR(MAX) NULL,
        ProgressLog NVARCHAR(MAX) NULL,
        RelatedBackupId INT NULL,
        CONSTRAINT FK_DatabaseOperations_AppUsers FOREIGN KEY (StartedByUserId) 
            REFERENCES AppUsers(UserId) ON DELETE NO ACTION,
        CONSTRAINT FK_DatabaseOperations_DatabaseBackups FOREIGN KEY (RelatedBackupId) 
            REFERENCES DatabaseBackups(Id) ON DELETE NO ACTION
    );
    
    CREATE INDEX IX_DatabaseOperations_Status ON DatabaseOperations(Status) WHERE Status = 'Running';
    CREATE INDEX IX_DatabaseOperations_StartedAtUtc ON DatabaseOperations(StartedAtUtc DESC);
    
    PRINT 'DatabaseOperations table created successfully.';
END
ELSE
BEGIN
    PRINT 'DatabaseOperations table already exists.';
END
GO

-- =============================================
-- 3. Add columns to SystemSettings table
-- =============================================
PRINT 'Updating SystemSettings table...';

-- BackupStoragePath
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('SystemSettings') AND name = 'BackupStoragePath')
BEGIN
    PRINT 'Adding BackupStoragePath column...';
    ALTER TABLE SystemSettings ADD BackupStoragePath NVARCHAR(500) NOT NULL DEFAULT 'C:\GFC_Backups\Sql\';
END

-- BackupRetentionCount
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('SystemSettings') AND name = 'BackupRetentionCount')
BEGIN
    PRINT 'Adding BackupRetentionCount column...';
    ALTER TABLE SystemSettings ADD BackupRetentionCount INT NOT NULL DEFAULT 10;
    ALTER TABLE SystemSettings ADD CONSTRAINT CK_SystemSettings_BackupRetentionCount CHECK (BackupRetentionCount >= 1 AND BackupRetentionCount <= 100);
END

-- AllowServerRestoreOperations
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('SystemSettings') AND name = 'AllowServerRestoreOperations')
BEGIN
    PRINT 'Adding AllowServerRestoreOperations column...';
    ALTER TABLE SystemSettings ADD AllowServerRestoreOperations BIT NOT NULL DEFAULT 0;
END

-- MaintenanceModeEnabled
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('SystemSettings') AND name = 'MaintenanceModeEnabled')
BEGIN
    PRINT 'Adding MaintenanceModeEnabled column...';
    ALTER TABLE SystemSettings ADD MaintenanceModeEnabled BIT NOT NULL DEFAULT 0;
END

PRINT 'SystemSettings table updated successfully.';
GO

-- =============================================
-- 4. Ensure backup storage directory exists
-- =============================================
PRINT 'Database Maintenance schema setup completed successfully!';
PRINT '';
PRINT 'IMPORTANT: Please ensure the backup storage directory exists:';
PRINT '  Default: C:\GFC_Backups\Sql\';
PRINT '  The SQL Server service account must have read/write permissions to this directory.';
PRINT '';
PRINT 'You can change the backup path in the Database Maintenance page settings.';
GO
