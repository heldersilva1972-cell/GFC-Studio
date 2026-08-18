-- Add MinimumPayout column to BingoGameDefinitions table
IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[BingoGameDefinitions]') 
      AND name = N'MinimumPayout'
)
BEGIN
    ALTER TABLE [dbo].[BingoGameDefinitions]
    ADD [MinimumPayout] DECIMAL(18, 2) NOT NULL DEFAULT 0.00;
END
GO
