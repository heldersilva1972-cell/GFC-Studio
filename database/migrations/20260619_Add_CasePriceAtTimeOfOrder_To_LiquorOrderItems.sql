USE [ClubMembership]
GO

IF NOT EXISTS (
    SELECT * 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.LiquorOrderItems') 
      AND name = 'CasePriceAtTimeOfOrder'
)
BEGIN
    PRINT 'Adding CasePriceAtTimeOfOrder column to dbo.LiquorOrderItems...'
    ALTER TABLE [dbo].[LiquorOrderItems]
    ADD [CasePriceAtTimeOfOrder] DECIMAL(18,2) NULL;
    PRINT 'CasePriceAtTimeOfOrder column added successfully!'
END
ELSE
BEGIN
    PRINT 'CasePriceAtTimeOfOrder column already exists in dbo.LiquorOrderItems.'
END
GO
