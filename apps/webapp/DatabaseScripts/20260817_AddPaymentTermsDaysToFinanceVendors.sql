-- Add PaymentTermsDays column to FinanceVendors table
IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[FinanceVendors]') 
      AND name = N'PaymentTermsDays'
)
BEGIN
    ALTER TABLE [dbo].[FinanceVendors]
    ADD [PaymentTermsDays] INT NULL;
END
GO
