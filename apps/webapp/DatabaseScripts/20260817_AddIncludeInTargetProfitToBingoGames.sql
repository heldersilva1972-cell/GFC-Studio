-- Migration: Add IncludeInTargetProfit to BingoGameDefinitions
-- Date: 2026-08-17

IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[BingoGameDefinitions]') 
    AND name = 'IncludeInTargetProfit'
)
BEGIN
    ALTER TABLE [dbo].[BingoGameDefinitions] 
    ADD [IncludeInTargetProfit] BIT NOT NULL DEFAULT 0;
    PRINT 'Added IncludeInTargetProfit column to BingoGameDefinitions table.';
END
ELSE
BEGIN
    PRINT 'IncludeInTargetProfit column already exists in BingoGameDefinitions table.';
END
GO
