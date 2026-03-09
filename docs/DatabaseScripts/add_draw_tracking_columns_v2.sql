USE [ClubMembership]
GO

PRINT 'Repairing SystemSettings schema for Sign-in Draw tracking...';

-- Add tracking column for the Sign-in Draw printable list
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LastSignInDrawExportUtc')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [LastSignInDrawExportUtc] DATETIME2 NULL;
    PRINT 'Added LastSignInDrawExportUtc column.';
END
GO

-- Add Shift logic columns used for dashboard metrics
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'DayShiftStartTime')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [DayShiftStartTime] TIME NOT NULL DEFAULT '09:00:00';
    PRINT 'Added DayShiftStartTime column.';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'DayShiftEndTime')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [DayShiftEndTime] TIME NOT NULL DEFAULT '17:00:00';
    PRINT 'Added DayShiftEndTime column.';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NightShiftStartTime')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [NightShiftStartTime] TIME NOT NULL DEFAULT '18:00:00';
    PRINT 'Added NightShiftStartTime column.';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'NightShiftEndTime')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [NightShiftEndTime] TIME NOT NULL DEFAULT '02:00:00';
    PRINT 'Added NightShiftEndTime column.';
END
GO

-- Populate defaults for existing rows
UPDATE [dbo].[SystemSettings] SET 
    [LastSignInDrawExportUtc] = ISNULL([LastSignInDrawExportUtc], NULL),
    [DayShiftStartTime] = ISNULL([DayShiftStartTime], '09:00:00'),
    [DayShiftEndTime] = ISNULL([DayShiftEndTime], '17:00:00'),
    [NightShiftStartTime] = ISNULL([NightShiftStartTime], '18:00:00'),
    [NightShiftEndTime] = ISNULL([NightShiftEndTime], '02:00:00')
WHERE Id = 1;
GO

PRINT 'SystemSettings schema repair completed.';
