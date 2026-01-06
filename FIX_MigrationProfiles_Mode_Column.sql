-- Fix MigrationProfiles Table - Mode Column Type
USE ClubMembership;
GO

PRINT 'Fixing MigrationProfiles table...';
PRINT '';

-- Drop the table and recreate with correct types
DROP TABLE IF EXISTS [dbo].[MigrationProfiles];
GO

-- Create with Mode as INT (for enum)
CREATE TABLE [dbo].[MigrationProfiles] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(200) NOT NULL,
    [Mode] INT NOT NULL DEFAULT 0,  -- Changed from NVARCHAR to INT for enum
    [CreatedAtUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [IsCompleted] BIT NOT NULL DEFAULT 0,
    [CompletedAtUtc] DATETIME2 NULL,
    [GatesStatusJson] NVARCHAR(MAX) NULL,
    [ReportContentTxt] NVARCHAR(MAX) NULL,
    [ReportGeneratedAtUtc] DATETIME2 NULL
);
GO

PRINT '✓ Table recreated with Mode as INT!';
PRINT '';

-- Verify structure
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'MigrationProfiles'
ORDER BY ORDINAL_POSITION;
GO

PRINT '';
PRINT '✓ FIXED! Mode column is now INT type.';
PRINT 'Restart your app and create a new migration profile.';
GO
