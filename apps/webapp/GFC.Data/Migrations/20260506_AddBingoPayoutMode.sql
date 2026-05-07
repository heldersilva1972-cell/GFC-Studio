-- Migration: Add PayoutMode and IsAdmissionOnly to BingoSheetDefinitions
-- Description: Adds columns to support prize logic modes and the 'Base Program' Admission-Only sheets.

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[BingoSheetDefinitions]') AND name = 'PayoutMode')
BEGIN
    -- 0 = Fixed (Tiered)
    -- 1 = 50/50 Split (Sequential)
    -- 2 = Gross Percentage (Additive)
    ALTER TABLE [dbo].[BingoSheetDefinitions] ADD [PayoutMode] INT NOT NULL DEFAULT 0;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[BingoSheetDefinitions]') AND name = 'IsAdmissionOnly')
BEGIN
    ALTER TABLE [dbo].[BingoSheetDefinitions] ADD [IsAdmissionOnly] BIT NOT NULL DEFAULT 0;
END
GO

-- Migrate existing IsFiftyFifty flags to PayoutMode 1 (50/50 Split)
-- (Note: IsFiftyFifty column is legacy and can be removed later after verification)
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[BingoSheetDefinitions]') AND name = 'IsFiftyFifty')
BEGIN
    EXEC sp_executesql N'UPDATE [dbo].[BingoSheetDefinitions] SET [PayoutMode] = 1 WHERE [IsFiftyFifty] = 1 AND [PayoutMode] = 0';
END
GO
