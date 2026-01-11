USE [ClubMembership]
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ControllerEvents]') AND name = 'ControllerEventTime')
BEGIN
    ALTER TABLE [dbo].[ControllerEvents] ADD [ControllerEventTime] DATETIME2 NULL;
    PRINT 'Added ControllerEventTime column to ControllerEvents table.';
END
GO

-- Populate existing rows with TimestampUtc value as a fallback
UPDATE [dbo].[ControllerEvents]
SET [ControllerEventTime] = [TimestampUtc]
WHERE [ControllerEventTime] IS NULL;
GO

-- Make it NOT NULL if desired, but NULL is safer for now if we want to allow updates
-- ALTER TABLE [dbo].[ControllerEvents] ALTER COLUMN [ControllerEventTime] DATETIME2 NOT NULL;
GO
