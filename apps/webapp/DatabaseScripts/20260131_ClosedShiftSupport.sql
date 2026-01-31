/* 
   Shift Management - Closed Status Support
   - Makes StaffMemberId nullable to allow for shifts marked as CLOSED
*/

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StaffShifts') AND name = 'StaffMemberId')
BEGIN
    ALTER TABLE StaffShifts ALTER COLUMN StaffMemberId INT NULL;
END
GO
