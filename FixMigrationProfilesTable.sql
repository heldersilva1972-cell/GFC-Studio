-- Check and Fix MigrationProfiles Table Structure
USE ClubMembership;
GO

-- Check current table structure
PRINT 'Current MigrationProfiles table structure:';
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'MigrationProfiles'
ORDER BY ORDINAL_POSITION;
GO

-- Check if Id column is the wrong type
IF EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'MigrationProfiles' 
    AND COLUMN_NAME = 'Id' 
    AND DATA_TYPE != 'int'
)
BEGIN
    PRINT '';
    PRINT '❌ ERROR: Id column is not INT type!';
    PRINT 'Dropping and recreating table...';
    
    -- Drop the table
    DROP TABLE [dbo].[MigrationProfiles];
    
    -- Recreate with correct structure
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
    
    PRINT '✓ Table recreated with correct structure!';
END
ELSE
BEGIN
    PRINT '';
    PRINT '✓ Id column is correct INT type';
END
GO

-- Show final structure
PRINT '';
PRINT 'Final table structure:';
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'MigrationProfiles'
ORDER BY ORDINAL_POSITION;
GO
