IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[BarSaleEntries]') 
    AND name = 'AdjustedSaleDate'
)
BEGIN
    ALTER TABLE [dbo].[BarSaleEntries]
    ADD [AdjustedSaleDate] datetime2 NULL;
END
GO
