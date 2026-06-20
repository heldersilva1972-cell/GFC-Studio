USE [ClubMembership]
GO

IF NOT EXISTS (
    SELECT * 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.LiquorItems') 
      AND name = 'CasePrice'
)
BEGIN
    PRINT 'Adding CasePrice column to dbo.LiquorItems...'
    ALTER TABLE [dbo].[LiquorItems]
    ADD [CasePrice] DECIMAL(18,2) NULL;
    PRINT 'CasePrice column added successfully!'
END
ELSE
BEGIN
    PRINT 'CasePrice column already exists in dbo.LiquorItems.'
END
GO
