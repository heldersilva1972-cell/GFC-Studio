-- ============================================================
-- Migration: Add MonthlyPaymentAmount, PaymentDueDay, and SkippedMonths to FinanceLoans
-- Run this script ONCE against the GFC production/development database
-- ============================================================

IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = 'dbo'
      AND TABLE_NAME   = 'FinanceLoans'
      AND COLUMN_NAME  = 'MonthlyPaymentAmount'
)
BEGIN
    ALTER TABLE [dbo].[FinanceLoans]
        ADD [MonthlyPaymentAmount] DECIMAL(18,2) NULL,
            [PaymentDueDay] INT NULL,
            [SkippedMonths] NVARCHAR(MAX) NULL;

    PRINT 'Added columns MonthlyPaymentAmount, PaymentDueDay, and SkippedMonths to FinanceLoans';
END
ELSE
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_SCHEMA = 'dbo'
          AND TABLE_NAME   = 'FinanceLoans'
          AND COLUMN_NAME  = 'SkippedMonths'
    )
    BEGIN
        ALTER TABLE [dbo].[FinanceLoans]
            ADD [SkippedMonths] NVARCHAR(MAX) NULL;
        PRINT 'Added SkippedMonths column to FinanceLoans';
    END
    ELSE
    BEGIN
        PRINT 'Columns already exist on FinanceLoans – skipped';
    END
END
GO

PRINT '=== Migration complete ===';
GO
