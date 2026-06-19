-- ============================================================
-- Migration: Add GroupName column to FinanceLoans
-- Run this script ONCE against the GFC production/development database
-- ============================================================

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = 'dbo'
      AND TABLE_NAME   = 'FinanceLoans'
      AND COLUMN_NAME  = 'GroupName'
)
BEGIN
    ALTER TABLE [dbo].[FinanceLoans]
        ADD [GroupName] NVARCHAR(100) NULL;

    PRINT 'Added column GroupName to FinanceLoans';
END
ELSE
BEGIN
    PRINT 'Column GroupName already exists on FinanceLoans – skipped';
END
GO

PRINT '=== Migration complete ===';
GO
