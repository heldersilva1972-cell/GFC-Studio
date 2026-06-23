USE [ClubMembership]
GO

IF NOT EXISTS (
    SELECT * 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.LiquorItems') 
      AND name = 'ExcludeFromPredictions'
)
BEGIN
    PRINT 'Adding ExcludeFromPredictions column to dbo.LiquorItems...'
    ALTER TABLE [dbo].[LiquorItems]
    ADD [ExcludeFromPredictions] BIT NOT NULL DEFAULT 0;
    PRINT 'ExcludeFromPredictions column added successfully!'
END
ELSE
BEGIN
    PRINT 'ExcludeFromPredictions column already exists in dbo.LiquorItems.'
END
GO
