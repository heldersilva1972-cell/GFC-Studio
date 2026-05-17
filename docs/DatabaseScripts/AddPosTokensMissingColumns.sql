-- =============================================
-- ADD MISSING COLUMNS TO POSTOKENS TABLE
-- Description: Ensures the required fields for smart upgrades and liability tracking exist in the PosTokens table
-- Run this on the PRODUCTION database.
-- =============================================

PRINT '-----------------------------------------';
PRINT 'Starting PosTokens Database Column Fix';
PRINT '-----------------------------------------';

-- 1. Check/Add AllowCreditUpgrade
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PosTokens]') AND name = 'AllowCreditUpgrade')
BEGIN
    ALTER TABLE [dbo].[PosTokens] ADD [AllowCreditUpgrade] BIT NULL DEFAULT 0;
    PRINT '✓ Added AllowCreditUpgrade column';
END
ELSE
BEGIN
    PRINT '✓ AllowCreditUpgrade column already exists';
END

-- 2. Check/Add CreditValue
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PosTokens]') AND name = 'CreditValue')
BEGIN
    ALTER TABLE [dbo].[PosTokens] ADD [CreditValue] DECIMAL(18,2) NULL;
    PRINT '✓ Added CreditValue column';
END
ELSE
BEGIN
    PRINT '✓ CreditValue column already exists';
END

-- 3. Check/Add CreditEligibleCategories
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PosTokens]') AND name = 'CreditEligibleCategories')
BEGIN
    ALTER TABLE [dbo].[PosTokens] ADD [CreditEligibleCategories] NVARCHAR(MAX) NULL;
    PRINT '✓ Added CreditEligibleCategories column';
END
ELSE
BEGIN
    PRINT '✓ CreditEligibleCategories column already exists';
END

-- 4. Check/Add StartingLiabilityBalance
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PosTokens]') AND name = 'StartingLiabilityBalance')
BEGIN
    ALTER TABLE [dbo].[PosTokens] ADD [StartingLiabilityBalance] INT NOT NULL DEFAULT 0;
    PRINT '✓ Added StartingLiabilityBalance column';
END
ELSE
BEGIN
    PRINT '✓ StartingLiabilityBalance column already exists';
END

PRINT '-----------------------------------------';
PRINT 'PosTokens database fix completed!';
PRINT '-----------------------------------------';
GO
