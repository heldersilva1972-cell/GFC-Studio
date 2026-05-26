IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ActiveEvents]') AND name = N'BeerTalliesJson')
BEGIN
    ALTER TABLE [dbo].[ActiveEvents] ADD [BeerTalliesJson] NVARCHAR(MAX) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ActiveEvents]') AND name = N'EnableBeerTally')
BEGIN
    ALTER TABLE [dbo].[ActiveEvents] ADD [EnableBeerTally] BIT NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[EventTemplates]') AND name = N'EnableBeerTally')
BEGIN
    ALTER TABLE [dbo].[EventTemplates] ADD [EnableBeerTally] BIT NOT NULL DEFAULT 0;
END
GO
