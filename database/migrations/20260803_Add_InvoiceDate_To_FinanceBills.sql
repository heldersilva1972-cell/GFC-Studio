USE [ClubMembership]
GO

IF NOT EXISTS (
    SELECT * 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.FinanceBills') 
      AND name = 'InvoiceDate'
)
BEGIN
    PRINT 'Adding InvoiceDate column to dbo.FinanceBills...'
    ALTER TABLE [dbo].[FinanceBills]
    ADD [InvoiceDate] DATETIME2 NULL;
    PRINT 'InvoiceDate column added successfully!'
END
ELSE
BEGIN
    PRINT 'InvoiceDate column already exists in dbo.FinanceBills.'
END
GO
