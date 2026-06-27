-- Database Migration: Add Lottery Settlements and Settings
-- Date: 2026-06-26

-- 1. Add columns to SystemSettings if they do not exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'LotteryBillCreationEnabled')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [LotteryBillCreationEnabled] BIT NOT NULL DEFAULT 0;
    PRINT 'Added LotteryBillCreationEnabled column to SystemSettings';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'LotteryRecordPaidHistoryEnabled')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [LotteryRecordPaidHistoryEnabled] BIT NOT NULL DEFAULT 0;
    PRINT 'Added LotteryRecordPaidHistoryEnabled column to SystemSettings';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'LotteryDefaultCategoryId')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [LotteryDefaultCategoryId] INT NULL;
    PRINT 'Added LotteryDefaultCategoryId column to SystemSettings';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = N'LotterySettingsLastEnabledUtc')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [LotterySettingsLastEnabledUtc] DATETIME NULL;
    PRINT 'Added LotterySettingsLastEnabledUtc column to SystemSettings';
END

-- 2. Create LotteryWeeklySettlements table if it does not exist
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LotteryWeeklySettlements]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[LotteryWeeklySettlements] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [WeekStartDate] DATETIME NOT NULL,
        [WeekEndDate] DATETIME NOT NULL,
        [NetDueAmount] DECIMAL(18, 2) NOT NULL,
        [Status] NVARCHAR(50) NOT NULL CONSTRAINT [DF_LotteryWeeklySettlements_Status] DEFAULT 'Pending',
        [SettleDate] DATETIME NULL,
        [SettleBy] NVARCHAR(256) NULL,
        [ReferenceNumber] NVARCHAR(256) NULL,
        [LinkedBillId] INT NULL,
        CONSTRAINT [PK_LotteryWeeklySettlements] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    
    PRINT 'Created LotteryWeeklySettlements table';
END

-- 3. Add foreign key constraint for LinkedBillId if it does not exist
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = N'FK_LotteryWeeklySettlements_FinanceBills_LinkedBillId')
    AND EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LotteryWeeklySettlements]') AND type in (N'U'))
    AND EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FinanceBills]') AND type in (N'U'))
BEGIN
    ALTER TABLE [dbo].[LotteryWeeklySettlements]  WITH CHECK ADD  CONSTRAINT [FK_LotteryWeeklySettlements_FinanceBills_LinkedBillId] FOREIGN KEY([LinkedBillId])
    REFERENCES [dbo].[FinanceBills] ([Id])
    ON DELETE SET NULL;
    
    ALTER TABLE [dbo].[LotteryWeeklySettlements] CHECK CONSTRAINT [FK_LotteryWeeklySettlements_FinanceBills_LinkedBillId];
    PRINT 'Added foreign key constraint FK_LotteryWeeklySettlements_FinanceBills_LinkedBillId';
END

-- 4. Add foreign key constraint for LotteryDefaultCategoryId in SystemSettings if it does not exist
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = N'FK_SystemSettings_FinanceCategories_LotteryDefaultCategoryId')
    AND EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND type in (N'U'))
    AND EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FinanceCategories]') AND type in (N'U'))
BEGIN
    ALTER TABLE [dbo].[SystemSettings]  WITH CHECK ADD  CONSTRAINT [FK_SystemSettings_FinanceCategories_LotteryDefaultCategoryId] FOREIGN KEY([LotteryDefaultCategoryId])
    REFERENCES [dbo].[FinanceCategories] ([Id])
    ON DELETE SET NULL;
    
    ALTER TABLE [dbo].[SystemSettings] CHECK CONSTRAINT [FK_SystemSettings_FinanceCategories_LotteryDefaultCategoryId];
    PRINT 'Added foreign key constraint FK_SystemSettings_FinanceCategories_LotteryDefaultCategoryId';
END
