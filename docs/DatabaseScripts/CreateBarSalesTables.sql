-- ============================================================
-- Script: CreateBarSalesTables.sql
-- Description: Creates the BarSaleEntries table for tracking
-- bar revenue and item velocity.
-- ============================================================

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BarSaleEntries')
BEGIN
    PRINT 'Creating table BarSaleEntries...';
    
    CREATE TABLE [dbo].[BarSaleEntries] (
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [SaleDate] [datetime2](7) NOT NULL,
        [TotalSales] [decimal](18, 2) NOT NULL,
        [TotalItemsSold] [int] NOT NULL,
        [Notes] [nvarchar](max) NULL,
        [CreatedAt] [datetime2](7) NOT NULL,
        CONSTRAINT [PK_BarSaleEntries] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    -- Create index for performance on history searches
    CREATE INDEX [IX_BarSaleEntries_SaleDate] ON [dbo].[BarSaleEntries] ([SaleDate]);
    
    PRINT 'Table BarSaleEntries created successfully.';
END
ELSE
BEGIN
    PRINT 'Table BarSaleEntries already exists.';
END
GO
