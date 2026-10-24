USE [ClubMembership]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==============================================================================
-- 1. Table: LiquorVendorQuotes
-- Tracks quotes, regular prices, and active deals per product from multiple vendors
-- ==============================================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LiquorVendorQuotes]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[LiquorVendorQuotes](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [LiquorItemId] [int] NOT NULL,
        [VendorId] [int] NOT NULL,
        [BottlePrice] [decimal](18, 2) NULL,
        [CasePrice] [decimal](18, 2) NULL,
        [IsCaseOnly] [bit] NOT NULL CONSTRAINT [DF_LiquorVendorQuotes_IsCaseOnly] DEFAULT (0),
        [PackSize] [int] NOT NULL CONSTRAINT [DF_LiquorVendorQuotes_PackSize] DEFAULT (1),
        [IsPreferredVendor] [bit] NOT NULL CONSTRAINT [DF_LiquorVendorQuotes_IsPreferredVendor] DEFAULT (0),
        
        -- Deal / Promotion details
        [HasDeal] [bit] NOT NULL CONSTRAINT [DF_LiquorVendorQuotes_HasDeal] DEFAULT (0),
        [DealType] [varchar](50) NULL, -- 'TieredQty', 'BuyXGetY', 'PostOffFlat', 'PercentOff', 'FamilyMix'
        [DealDescription] [nvarchar](255) NULL,
        [DealMinQty] [int] NULL, -- Minimum cases/units to qualify (e.g. 3)
        [DealFreeQty] [int] NULL, -- Free cases/units (for Buy X Get Y, e.g. 1)
        [DealPrice] [decimal](18, 2) NULL, -- Discounted price when deal applies
        [DealStartDate] [datetime2](7) NULL,
        [DealEndDate] [datetime2](7) NULL,
        
        -- Fee details
        [SplitCaseFee] [decimal](18, 2) NULL,
        [IncludesDeposit] [bit] NOT NULL CONSTRAINT [DF_LiquorVendorQuotes_IncludesDeposit] DEFAULT (0),
        [DepositAmount] [decimal](18, 2) NULL,
        
        [Notes] [nvarchar](500) NULL,
        [LastPriceChangeDate] [datetime2](7) NULL,
        [CreatedAt] [datetime2](7) NOT NULL CONSTRAINT [DF_LiquorVendorQuotes_CreatedAt] DEFAULT (SYSUTCDATETIME()),
        [UpdatedAt] [datetime2](7) NOT NULL CONSTRAINT [DF_LiquorVendorQuotes_UpdatedAt] DEFAULT (SYSUTCDATETIME()),
        
        CONSTRAINT [PK_LiquorVendorQuotes] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_LiquorVendorQuotes_LiquorItems] FOREIGN KEY([LiquorItemId]) REFERENCES [dbo].[LiquorItems] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_LiquorVendorQuotes_LiquorVendors] FOREIGN KEY([VendorId]) REFERENCES [dbo].[LiquorVendors] ([Id]) ON DELETE CASCADE
    );

    CREATE NONCLUSTERED INDEX [IX_LiquorVendorQuotes_LiquorItemId] ON [dbo].[LiquorVendorQuotes] ([LiquorItemId]);
    CREATE NONCLUSTERED INDEX [IX_LiquorVendorQuotes_VendorId] ON [dbo].[LiquorVendorQuotes] ([LiquorItemId]);
    CREATE UNIQUE NONCLUSTERED INDEX [IX_LiquorVendorQuotes_Item_Vendor] ON [dbo].[LiquorVendorQuotes] ([LiquorItemId], [VendorId]);

    PRINT 'Table [dbo].[LiquorVendorQuotes] created successfully.';
END
ELSE
BEGIN
    PRINT 'Table [dbo].[LiquorVendorQuotes] already exists.';
END
GO

-- ==============================================================================
-- 2. Table: LiquorPriceHistories
-- Audit trail to track price increases/decreases, percentage changes, and dates
-- ==============================================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LiquorPriceHistories]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[LiquorPriceHistories](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [LiquorVendorQuoteId] [int] NOT NULL,
        [LiquorItemId] [int] NOT NULL,
        [VendorId] [int] NOT NULL,
        [OldBottlePrice] [decimal](18, 2) NULL,
        [NewBottlePrice] [decimal](18, 2) NULL,
        [OldCasePrice] [decimal](18, 2) NULL,
        [NewCasePrice] [decimal](18, 2) NULL,
        [ChangeType] [varchar](50) NOT NULL, -- 'StandardChange', 'PromoStart', 'PromoEnd'
        [ChangeAmount] [decimal](18, 2) NULL,
        [ChangePercentage] [decimal](9, 4) NULL,
        [ChangeReason] [nvarchar](255) NULL,
        [EffectiveDate] [datetime2](7) NOT NULL CONSTRAINT [DF_LiquorPriceHistories_EffectiveDate] DEFAULT (SYSUTCDATETIME()),
        [RecordedByUserId] [nvarchar](100) NULL,
        
        CONSTRAINT [PK_LiquorPriceHistories] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_LiquorPriceHistories_LiquorVendorQuotes] FOREIGN KEY([LiquorVendorQuoteId]) REFERENCES [dbo].[LiquorVendorQuotes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_LiquorPriceHistories_LiquorItems] FOREIGN KEY([LiquorItemId]) REFERENCES [dbo].[LiquorItems] ([Id]),
        CONSTRAINT [FK_LiquorPriceHistories_LiquorVendors] FOREIGN KEY([VendorId]) REFERENCES [dbo].[LiquorVendors] ([Id])
    );

    CREATE NONCLUSTERED INDEX [IX_LiquorPriceHistories_QuoteId] ON [dbo].[LiquorPriceHistories] ([LiquorVendorQuoteId]);
    CREATE NONCLUSTERED INDEX [IX_LiquorPriceHistories_ItemId] ON [dbo].[LiquorPriceHistories] ([LiquorItemId]);
    CREATE NONCLUSTERED INDEX [IX_LiquorPriceHistories_EffectiveDate] ON [dbo].[LiquorPriceHistories] ([EffectiveDate] DESC);

    PRINT 'Table [dbo].[LiquorPriceHistories] created successfully.';
END
ELSE
BEGIN
    PRINT 'Table [dbo].[LiquorPriceHistories] already exists.';
END
GO

-- ==============================================================================
-- 3. Enhance dbo.LiquorItems for Market Watchlist / Research Items
-- Allows tracking products not yet stocked in bar inventory/POS
-- ==============================================================================
IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.LiquorItems') 
      AND name = 'IsWatchlistOnly'
)
BEGIN
    ALTER TABLE [dbo].[LiquorItems]
    ADD [IsWatchlistOnly] BIT NOT NULL CONSTRAINT [DF_LiquorItems_IsWatchlistOnly] DEFAULT (0);
    PRINT 'Added [IsWatchlistOnly] column to dbo.LiquorItems.';
END
GO

-- ==============================================================================
-- 4. Enhance dbo.LiquorVendors for Order Deadlines & Delivery Minimums
-- ==============================================================================
IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.LiquorVendors') 
      AND name = 'OrderCutoffTime'
)
BEGIN
    ALTER TABLE [dbo].[LiquorVendors]
    ADD [OrderCutoffTime] [nvarchar](100) NULL; -- e.g. 'Tuesdays 4:00 PM'
    PRINT 'Added [OrderCutoffTime] column to dbo.LiquorVendors.';
END
GO

IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.LiquorVendors') 
      AND name = 'DeliveryDays'
)
BEGIN
    ALTER TABLE [dbo].[LiquorVendors]
    ADD [DeliveryDays] [nvarchar](100) NULL; -- e.g. 'Thursday'
    PRINT 'Added [DeliveryDays] column to dbo.LiquorVendors.';
END
GO

IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.LiquorVendors') 
      AND name = 'SalesRepName'
)
BEGIN
    ALTER TABLE [dbo].[LiquorVendors]
    ADD [SalesRepName] [nvarchar](150) NULL;
    PRINT 'Added [SalesRepName] column to dbo.LiquorVendors.';
END
GO

IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.LiquorVendors') 
      AND name = 'SalesRepPhone'
)
BEGIN
    ALTER TABLE [dbo].[LiquorVendors]
    ADD [SalesRepPhone] [nvarchar](50) NULL;
    PRINT 'Added [SalesRepPhone] column to dbo.LiquorVendors.';
END
GO

-- ==============================================================================
-- 5. Backfill Initial Quotes from Existing LiquorItems
-- Populates LiquorVendorQuotes so existing data is immediately visible
-- ==============================================================================
INSERT INTO [dbo].[LiquorVendorQuotes] (
    [LiquorItemId],
    [VendorId],
    [BottlePrice],
    [CasePrice],
    [IsCaseOnly],
    [PackSize],
    [IsPreferredVendor],
    [HasDeal],
    [DealType],
    [DealMinQty],
    [DealPrice],
    [CreatedAt],
    [UpdatedAt]
)
SELECT 
    i.[Id] AS [LiquorItemId],
    i.[VendorId],
    CASE WHEN i.[OrderByCaseOnly] = 1 THEN NULL ELSE i.[CurrentPrice] END AS [BottlePrice],
    i.[CasePrice],
    i.[OrderByCaseOnly] AS [IsCaseOnly],
    ISNULL(i.[PackSize], 1) AS [PackSize],
    1 AS [IsPreferredVendor],
    CASE WHEN i.[BulkDiscountThreshold] IS NOT NULL AND i.[BulkDiscountPrice] IS NOT NULL THEN 1 ELSE 0 END AS [HasDeal],
    CASE WHEN i.[BulkDiscountThreshold] IS NOT NULL AND i.[BulkDiscountPrice] IS NOT NULL THEN 'TieredQty' ELSE NULL END AS [DealType],
    i.[BulkDiscountThreshold] AS [DealMinQty],
    i.[BulkDiscountPrice] AS [DealPrice],
    SYSUTCDATETIME(),
    SYSUTCDATETIME()
FROM [dbo].[LiquorItems] i
WHERE i.[VendorId] IS NOT NULL
  AND NOT EXISTS (
      SELECT 1 FROM [dbo].[LiquorVendorQuotes] q 
      WHERE q.[LiquorItemId] = i.[Id] AND q.[VendorId] = i.[VendorId]
  );

PRINT 'Backfilled existing LiquorItems into LiquorVendorQuotes.';
GO
