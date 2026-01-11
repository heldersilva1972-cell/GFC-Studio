IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[BarSaleEntries]') 
    AND name = 'Shift'
)
BEGIN
    ALTER TABLE [dbo].[BarSaleEntries]
    ADD [Shift] nvarchar(max) NOT NULL DEFAULT N'Day';
END
GO
