IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[BarSaleEntries]') AND name = 'IsAudited')
BEGIN
    ALTER TABLE [dbo].[BarSaleEntries] ADD [IsAudited] bit NOT NULL DEFAULT 0;
END
GO
