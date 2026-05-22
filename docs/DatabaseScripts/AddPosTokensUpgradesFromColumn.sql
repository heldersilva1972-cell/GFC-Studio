-- =============================================
-- ADD UpgradesFromTokenId COLUMN TO POSTOKENS
-- Description: Supports linked upgrade tokens by referencing the base token
-- Run this on the local & production database.
-- =============================================

PRINT '-----------------------------------------';
PRINT 'Starting PosTokens database migration';
PRINT '-----------------------------------------';

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PosTokens]') AND name = 'UpgradesFromTokenId')
BEGIN
    ALTER TABLE [dbo].[PosTokens] ADD [UpgradesFromTokenId] INT NULL;
    PRINT '✓ Added UpgradesFromTokenId column';
END
ELSE
BEGIN
    PRINT '✓ UpgradesFromTokenId column already exists';
END

PRINT '-----------------------------------------';
PRINT 'PosTokens database migration completed!';
PRINT '-----------------------------------------';
GO
