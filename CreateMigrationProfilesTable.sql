-- =============================================
-- Create MigrationProfiles Table Directly
-- =============================================
-- Run this in SQL Server Management Studio or any SQL client

USE ClubMembership;
GO

-- Check if table already exists
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MigrationProfiles')
BEGIN
    PRINT 'Creating MigrationProfiles table...';
    
    CREATE TABLE [dbo].[MigrationProfiles] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(200) NOT NULL,
        [Mode] NVARCHAR(50) NOT NULL,
        [CreatedAtUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [CreatedByUserId] INT NULL,
        [IsCompleted] BIT NOT NULL DEFAULT 0,
        [CompletedAtUtc] DATETIME2 NULL,
        [GatesStatusJson] NVARCHAR(MAX) NULL,
        [ReportContentTxt] NVARCHAR(MAX) NULL,
        [ReportGeneratedAtUtc] DATETIME2 NULL
    );
    
    PRINT '✓ MigrationProfiles table created successfully!';
END
ELSE
BEGIN
    PRINT '✓ MigrationProfiles table already exists.';
END
GO

-- Verify the table was created
SELECT 
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'MigrationProfiles'
ORDER BY ORDINAL_POSITION;
GO

PRINT '';
PRINT '========================================';
PRINT 'Migration Complete!';
PRINT '========================================';
PRINT 'You can now use the Migration Wizard.';
GO
