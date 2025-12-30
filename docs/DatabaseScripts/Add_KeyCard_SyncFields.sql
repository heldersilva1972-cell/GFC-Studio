/*
    Add synchronization status columns to KeyCards table.
    These columns are required for tracking status confirmation from the physical access controller.
*/

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[KeyCards]') AND name = N'IsControllerSynced')
BEGIN
    ALTER TABLE [dbo].[KeyCards] ADD [IsControllerSynced] BIT NOT NULL DEFAULT 0;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[KeyCards]') AND name = N'LastControllerSyncDate')
BEGIN
    ALTER TABLE [dbo].[KeyCards] ADD [LastControllerSyncDate] DATETIME NULL;
END
GO

-- Update existing active cards to be considered "Synced" if they were created before today
-- (This prevents a massive wave of "Not Confirmed" warnings for old cards)
UPDATE [dbo].[KeyCards]
SET [IsControllerSynced] = 1,
    [LastControllerSyncDate] = GETDATE()
WHERE [IsActive] = 1 AND [IsControllerSynced] = 0 AND [CreatedDate] < CAST(GETDATE() AS DATE);
GO
