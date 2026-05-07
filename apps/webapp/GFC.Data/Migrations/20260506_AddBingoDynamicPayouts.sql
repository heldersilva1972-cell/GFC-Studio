-- Migration: Add Bingo Dynamic Payouts
-- Description: Adds Payout Tiers and Variable Payout support.

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BingoPayoutTiers' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[BingoPayoutTiers] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [GlobalId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        [IsDeleted] BIT NOT NULL DEFAULT 0,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedAt] DATETIME2 NULL,
        [CreatedBy] NVARCHAR(MAX) NULL,
        [ModifiedBy] NVARCHAR(MAX) NULL,
        [SyncDate] DATETIME2 NULL,
        [RowVersion] ROWVERSION NULL,
        [MinAdmissions] INT NOT NULL,
        [PayoutPercentage] DECIMAL(18,2) NOT NULL DEFAULT 100.00,
        CONSTRAINT [PK_BingoPayoutTiers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[BingoGameDefinitions]') AND name = 'IsVariablePayout')
BEGIN
    ALTER TABLE [dbo].[BingoGameDefinitions] ADD [IsVariablePayout] BIT NOT NULL DEFAULT 0;
END
GO
