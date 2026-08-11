-- Add Donated Beer, Beer Tally & Proceeds Tracking Columns to EventTemplates and ActiveEvents
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventTemplates]') AND type in (N'U'))
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[EventTemplates]') AND name = N'EnableBeerTally')
        ALTER TABLE [dbo].[EventTemplates] ADD [EnableBeerTally] BIT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[EventTemplates]') AND name = N'ItemsOverrideJson')
        ALTER TABLE [dbo].[EventTemplates] ADD [ItemsOverrideJson] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[EventTemplates]') AND name = N'ClubDonatedCasesCap')
        ALTER TABLE [dbo].[EventTemplates] ADD [ClubDonatedCasesCap] INT NOT NULL DEFAULT 3;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[EventTemplates]') AND name = N'DonatedItemIdsJson')
        ALTER TABLE [dbo].[EventTemplates] ADD [DonatedItemIdsJson] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[EventTemplates]') AND name = N'Enable100PercentDonatedProceeds')
        ALTER TABLE [dbo].[EventTemplates] ADD [Enable100PercentDonatedProceeds] BIT NOT NULL DEFAULT 1;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[EventTemplates]') AND name = N'IsRecurring')
        ALTER TABLE [dbo].[EventTemplates] ADD [IsRecurring] BIT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[EventTemplates]') AND name = N'RecipientEventName')
        ALTER TABLE [dbo].[EventTemplates] ADD [RecipientEventName] NVARCHAR(100) NULL;
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActiveEvents]') AND type in (N'U'))
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ActiveEvents]') AND name = N'EnableBeerTally')
        ALTER TABLE [dbo].[ActiveEvents] ADD [EnableBeerTally] BIT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ActiveEvents]') AND name = N'BeerTalliesJson')
        ALTER TABLE [dbo].[ActiveEvents] ADD [BeerTalliesJson] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ActiveEvents]') AND name = N'ClubDonatedCasesCap')
        ALTER TABLE [dbo].[ActiveEvents] ADD [ClubDonatedCasesCap] INT NOT NULL DEFAULT 3;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ActiveEvents]') AND name = N'DonatedItemIdsJson')
        ALTER TABLE [dbo].[ActiveEvents] ADD [DonatedItemIdsJson] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ActiveEvents]') AND name = N'DonatedItemTalliesJson')
        ALTER TABLE [dbo].[ActiveEvents] ADD [DonatedItemTalliesJson] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ActiveEvents]') AND name = N'DonatedBeerClaimedCount')
        ALTER TABLE [dbo].[ActiveEvents] ADD [DonatedBeerClaimedCount] INT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ActiveEvents]') AND name = N'DonatedBeerReDonatedCount')
        ALTER TABLE [dbo].[ActiveEvents] ADD [DonatedBeerReDonatedCount] INT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ActiveEvents]') AND name = N'DonatedBeerSoldCount')
        ALTER TABLE [dbo].[ActiveEvents] ADD [DonatedBeerSoldCount] INT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ActiveEvents]') AND name = N'Enable100PercentDonatedProceeds')
        ALTER TABLE [dbo].[ActiveEvents] ADD [Enable100PercentDonatedProceeds] BIT NOT NULL DEFAULT 1;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ActiveEvents]') AND name = N'IsRecurring')
        ALTER TABLE [dbo].[ActiveEvents] ADD [IsRecurring] BIT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ActiveEvents]') AND name = N'ActiveGrantJson')
        ALTER TABLE [dbo].[ActiveEvents] ADD [ActiveGrantJson] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ActiveEvents]') AND name = N'RecipientEventName')
        ALTER TABLE [dbo].[ActiveEvents] ADD [RecipientEventName] NVARCHAR(100) NULL;
END
GO
