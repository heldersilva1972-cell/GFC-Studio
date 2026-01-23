-- LIQUOR INVENTORY DOCTOR & SCHEMA ALIGNMENT --
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LiquorItems')
BEGIN
    CREATE TABLE [dbo].[LiquorItems] (
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](200) NOT NULL,
        [Description] [nvarchar](500) NULL,
        [UpcCode] [nvarchar](100) NULL,
        [BottleSize] [nvarchar](50) NULL,
        [Category] [nvarchar](100) NULL,
        [ImageUrl] [nvarchar](max) NULL,
        [CurrentStock] [int] NOT NULL DEFAULT 0,
        [MinStockLimit] [int] NOT NULL DEFAULT 2,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_LiquorItems] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LiquorItems_UpcCode')
BEGIN
    CREATE UNIQUE INDEX [IX_LiquorItems_UpcCode] ON [dbo].[LiquorItems] ([UpcCode]) WHERE [UpcCode] IS NOT NULL;
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LiquorTransactions')
BEGIN
    CREATE TABLE [dbo].[LiquorTransactions] (
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [ItemId] [int] NOT NULL,
        [UserId] [int] NOT NULL,
        [ChangeAmount] [int] NOT NULL,
        [TransactionType] [nvarchar](50) NOT NULL,
        [Timestamp] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        [Notes] [nvarchar](500) NULL,
        CONSTRAINT [PK_LiquorTransactions] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_LiquorTransactions_LiquorItems] FOREIGN KEY([ItemId]) REFERENCES [dbo].[LiquorItems] ([Id]) ON DELETE CASCADE
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LiquorNotificationRules')
BEGIN
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
END

-- Ensure the pages are in AppPages
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
GO
