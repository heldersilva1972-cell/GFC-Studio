-- Adds the Google Calendar event id to hall rental requests so the app can
-- update/delete the exact Google event instead of guessing by date + name.
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'GoogleEventId')
BEGIN
    ALTER TABLE [dbo].[HallRentalRequests] ADD [GoogleEventId] NVARCHAR(256) NULL;
    PRINT 'Added [GoogleEventId] to [dbo].[HallRentalRequests].';
END
GO
