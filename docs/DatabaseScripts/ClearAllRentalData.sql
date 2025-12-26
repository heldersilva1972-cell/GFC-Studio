-- Delete all hall rental requests and calendar entries
-- Run this in SQL Server Management Studio or your database tool

USE ClubMembership;
GO

-- Delete all rental requests
DELETE FROM [dbo].[HallRentalRequests];
PRINT 'Deleted all hall rental requests';

-- Delete all calendar availability entries
DELETE FROM [dbo].[AvailabilityCalendars];
PRINT 'Deleted all calendar availability entries';

-- Optional: Reset identity seeds to start IDs from 1 again
DBCC CHECKIDENT ('HallRentalRequests', RESEED, 0);
DBCC CHECKIDENT ('AvailabilityCalendars', RESEED, 0);
PRINT 'Reset identity seeds';

PRINT 'Calendar cleared successfully!';
GO
