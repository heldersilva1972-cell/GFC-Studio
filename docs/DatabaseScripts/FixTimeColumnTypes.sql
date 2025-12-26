-- Fix: Convert StartTime and EndTime columns from INT to NVARCHAR
-- The columns were created as INT but the model expects strings

USE ClubMembership;
GO

-- First, update any existing numeric values to formatted strings
UPDATE HallRentalRequests 
SET StartTime = CAST(StartTime AS NVARCHAR(50)) + ':00 PM'
WHERE StartTime IS NOT NULL AND ISNUMERIC(StartTime) = 1;

UPDATE HallRentalRequests 
SET EndTime = CAST(EndTime AS NVARCHAR(50)) + ':00 PM'
WHERE EndTime IS NOT NULL AND ISNUMERIC(EndTime) = 1;

-- Now alter the column types
ALTER TABLE [dbo].[HallRentalRequests] 
ALTER COLUMN [StartTime] NVARCHAR(50) NULL;

ALTER TABLE [dbo].[HallRentalRequests] 
ALTER COLUMN [EndTime] NVARCHAR(50) NULL;

PRINT 'Column types fixed! StartTime and EndTime are now NVARCHAR(50)';
GO
