-- ============================================================
-- Migration: Add MonthlyPaymentAmount and PaymentDueDay to FinanceLoans
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
            [PaymentDueDay] INT NULL;

    PRINT 'Added columns MonthlyPaymentAmount and PaymentDueDay to FinanceLoans';
END
ELSE
BEGIN
    PRINT 'Columns already exist on FinanceLoans – skipped';
END
GO

PRINT '=== Migration complete ===';
GO
