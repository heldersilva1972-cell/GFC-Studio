-- =============================================
-- Fix: Add CreatedDate to KeyCards Table
-- Date: 2025-12-30
-- Description: Adds the missing CreatedDate column to KeyCards table to track card issuance date.
-- =============================================

USE ClubMembership;
GO

-- 1. Add CreatedDate column if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.KeyCards') AND name = 'CreatedDate')
BEGIN
    ALTER TABLE dbo.KeyCards
    ADD CreatedDate DATETIME NULL;
    
    PRINT 'Added CreatedDate column to KeyCards table';
END
GO

-- 2. Backfill existing records (Optional - set to a default or leave NULL)
-- We'll leave them as NULL to indicate "Unknown" or backfill with a default date if preferred.
-- Uncomment the below lines if you want to set a default date for existing cards:
-- UPDATE dbo.KeyCards
-- SET CreatedDate = '2024-01-01'
-- WHERE CreatedDate IS NULL;
-- GO

PRINT 'KeyCards table update complete.';
GO
