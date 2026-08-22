-- Migration: Create LotteryVendingCollections Table
-- Created: 2026-08-22
-- Description: Table for tracking cash collected from Scratch Ticket Vending Machines (ITVM)

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LotteryVendingCollections')
BEGIN
    CREATE TABLE [dbo].[LotteryVendingCollections] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [CollectionDate] DATETIME2 NOT NULL,
        [PeriodStartDate] DATETIME2 NOT NULL,
        [PeriodEndDate] DATETIME2 NOT NULL,
        [AmountCollected] DECIMAL(18,2) NOT NULL,
        [EnteredBy] NVARCHAR(100) NULL,
        [Notes] NVARCHAR(MAX) NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT (GETDATE()),
        [UpdatedAt] DATETIME2 NULL
    );

    CREATE INDEX [IX_LotteryVendingCollections_CollectionDate] ON [dbo].[LotteryVendingCollections] ([CollectionDate]);

    PRINT 'LotteryVendingCollections table created successfully.';
END
ELSE
BEGIN
    PRINT 'LotteryVendingCollections table already exists.';
END
GO
