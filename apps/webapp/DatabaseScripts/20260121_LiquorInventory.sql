-- [NEW] LIQUOR INVENTORY TABLES
-- Migration for Liquor Inventory Tracking System

-- 1. LiquorItems Catalog
CREATE TABLE [dbo].[LiquorItems] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](200) NOT NULL,
    [Description] [nvarchar](500) NULL,
    [UpcCode] [nvarchar](100) NULL,
    [BottleSize] [nvarchar](50) NULL, -- e.g. "750ml", "1.75L"
    [Category] [nvarchar](100) NULL, -- e.g. "Vodka", "Whiskey"
    [ImageUrl] [nvarchar](max) NULL,
    [CurrentStock] [int] NOT NULL DEFAULT 0,
    [MinStockLimit] [int] NOT NULL DEFAULT 2,
    [IsActive] [bit] NOT NULL DEFAULT 1,
    [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [PK_LiquorItems] PRIMARY KEY CLUSTERED ([Id] ASC)
);

-- Index for fast UPC scanning
CREATE UNIQUE INDEX [IX_LiquorItems_UpcCode] ON [dbo].[LiquorItems] ([UpcCode]) WHERE [UpcCode] IS NOT NULL;

-- 2. LiquorTransactions Audit Log
CREATE TABLE [dbo].[LiquorTransactions] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [ItemId] [int] NOT NULL,
    [UserId] [int] NOT NULL,
    [ChangeAmount] [int] NOT NULL, -- e.g. -1 for checkout
    [TransactionType] [nvarchar](50) NOT NULL, -- e.g. "Checkout", "Restock", "Adjustment"
    [Timestamp] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
    [Notes] [nvarchar](500) NULL,
    CONSTRAINT [PK_LiquorTransactions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LiquorTransactions_LiquorItems] FOREIGN KEY([ItemId]) REFERENCES [dbo].[LiquorItems] ([Id]) ON DELETE CASCADE
);

-- 3. LiquorNotificationRules
CREATE TABLE [dbo].[LiquorNotificationRules] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NOT NULL,
    [NotifyOnLowStock] [bit] NOT NULL DEFAULT 1,
    [NotifyOnEmpty] [bit] NOT NULL DEFAULT 1,
    [ReceivePush] [bit] NOT NULL DEFAULT 1,
    [ReceiveSms] [bit] NOT NULL DEFAULT 0,
    [ReceiveEmail] [bit] NOT NULL DEFAULT 0,
    CONSTRAINT [PK_LiquorNotificationRules] PRIMARY KEY CLUSTERED ([Id] ASC)
);

-- 4. Register new pages in AppPages for permissions
IF NOT EXISTS (SELECT 1 FROM [dbo].[AppPages] WHERE [PageRoute] = '/mobile/liquor/checkout')
BEGIN
    INSERT INTO [dbo].[AppPages] ([PageName], [PageRoute], [Description], [IsActive], [Category], [RequiresAdmin])
    VALUES ('Liquor Checkout', '/mobile/liquor/checkout', 'Bartender bottle checkout scanner', 1, 'Mobile', 0);
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[AppPages] WHERE [PageRoute] = '/mobile/liquor/manage')
BEGIN
    INSERT INTO [dbo].[AppPages] ([PageName], [PageRoute], [Description], [IsActive], [Category], [RequiresAdmin])
    VALUES ('Liquor Inventory Manager', '/mobile/liquor/manage', 'Manage stock and notification rules', 1, 'Inventory', 1);
END
