/*
 * GFC POS Sales Tracking Table
 * Created: 2026-04-08
 * Description: Stores individual transaction records for all POS terminals.
 */

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PosSales]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[PosSales] (
        [Id]            UNIQUEIDENTIFIER NOT NULL DEFAULT (NEWID()),
        [Timestamp]     DATETIME         NOT NULL DEFAULT (GETDATE()),
        [TerminalName]  NVARCHAR(100)    NOT NULL,
        [BartenderName] NVARCHAR(100)    NOT NULL,
        [TotalAmount]   DECIMAL(18, 2)   NOT NULL,
        [PaymentType]   NVARCHAR(50)     NOT NULL,
        [ItemsJson]     NVARCHAR(MAX)    NOT NULL,
        [IsSynced]      BIT              NOT NULL DEFAULT (1), -- Default to 1 since sync happens at save
        
        CONSTRAINT [PK_PosSales] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    -- Indexes for reporting performance
    CREATE INDEX [IX_PosSales_Timestamp] ON [dbo].[PosSales] ([Timestamp] DESC);
    CREATE INDEX [IX_PosSales_TerminalName] ON [dbo].[PosSales] ([TerminalName] ASC);
    CREATE INDEX [IX_PosSales_BartenderName] ON [dbo].[PosSales] ([BartenderName] ASC);

    PRINT 'Table [dbo].[PosSales] created successfully.';
END
ELSE
BEGIN
    PRINT 'Table [dbo].[PosSales] already exists.';
END
GO
