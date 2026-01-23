-- Add KeyType column to KeyHistory table for tracking Card vs Fob assignments
-- Run this script on the ClubMembership database

USE ClubMembership;
GO

-- Add KeyType column to KeyHistory table
IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.KeyHistory') 
    AND name = 'KeyType'
)
BEGIN
    ALTER TABLE dbo.KeyHistory 
    ADD KeyType NVARCHAR(50) NULL;
    
    PRINT 'KeyType column added to KeyHistory table successfully.';
END
ELSE
BEGIN
    PRINT 'KeyType column already exists in KeyHistory table.';
END
GO

-- Update existing records to default to 'Card' if null
UPDATE dbo.KeyHistory
SET KeyType = 'Card'
WHERE KeyType IS NULL;
GO

PRINT 'Database schema update completed successfully.';
GO
