-- Manual Migration: Add Hall Rental Pricing Settings to WebsiteSettings Table
-- Run this script on your ClubMembership database

-- Check if columns exist before adding them
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'MemberRate')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings]
    ADD [MemberRate] decimal(18,2) NULL;
    PRINT 'Added MemberRate column';
END
ELSE
    PRINT 'MemberRate column already exists';
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'NonMemberRate')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings]
    ADD [NonMemberRate] decimal(18,2) NULL;
    PRINT 'Added NonMemberRate column';
END
ELSE
    PRINT 'NonMemberRate column already exists';
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'NonProfitRate')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings]
    ADD [NonProfitRate] decimal(18,2) NULL;
    PRINT 'Added NonProfitRate column';
END
ELSE
    PRINT 'NonProfitRate column already exists';
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'KitchenFee')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings]
    ADD [KitchenFee] decimal(18,2) NULL;
    PRINT 'Added KitchenFee column';
END
ELSE
    PRINT 'KitchenFee column already exists';
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'AvEquipmentFee')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings]
    ADD [AvEquipmentFee] decimal(18,2) NULL;
    PRINT 'Added AvEquipmentFee column';
END
ELSE
    PRINT 'AvEquipmentFee column already exists';
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SecurityDepositAmount')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings]
    ADD [SecurityDepositAmount] decimal(18,2) NULL;
    PRINT 'Added SecurityDepositAmount column';
END
ELSE
    PRINT 'SecurityDepositAmount column already exists';
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'BaseFunctionHours')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings]
    ADD [BaseFunctionHours] int NULL;
    PRINT 'Added BaseFunctionHours column';
END
ELSE
    PRINT 'BaseFunctionHours column already exists';
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'AdditionalHourRate')
BEGIN
    ALTER TABLE [dbo].[WebsiteSettings]
    ADD [AdditionalHourRate] decimal(18,2) NULL;
    PRINT 'Added AdditionalHourRate column';
END
ELSE
    PRINT 'AdditionalHourRate column already exists';
GO

-- Set default values for new columns if WebsiteSettings row exists
-- This is in a separate batch (after GO) so the columns are recognized
IF EXISTS (SELECT 1 FROM [dbo].[WebsiteSettings])
BEGIN
    UPDATE [dbo].[WebsiteSettings]
    SET 
        [MemberRate] = ISNULL([MemberRate], 300),
        [NonMemberRate] = ISNULL([NonMemberRate], 400),
        [NonProfitRate] = ISNULL([NonProfitRate], 200),
        [KitchenFee] = ISNULL([KitchenFee], 100),
        [AvEquipmentFee] = ISNULL([AvEquipmentFee], 50),
        [SecurityDepositAmount] = ISNULL([SecurityDepositAmount], 200),
        [BaseFunctionHours] = ISNULL([BaseFunctionHours], 5),
        [AdditionalHourRate] = ISNULL([AdditionalHourRate], 50);
    
    PRINT 'Updated WebsiteSettings with default pricing values';
END
ELSE
BEGIN
    PRINT 'No WebsiteSettings row exists yet - defaults will be set when row is created';
END

PRINT 'Migration completed successfully!';
GO
