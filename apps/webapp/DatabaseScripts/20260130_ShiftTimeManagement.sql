/* 
   Shift Time Management Enhancements
   - Adds global default shift times to SystemSettings
   - Adds per-shift override capability to StaffShifts
*/

-- Add standard shift times to SystemSettings
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('SystemSettings') AND name = 'DayShiftStartTime')
BEGIN
    ALTER TABLE SystemSettings ADD DayShiftStartTime TIME NOT NULL DEFAULT '09:00:00';
    ALTER TABLE SystemSettings ADD DayShiftEndTime TIME NOT NULL DEFAULT '17:00:00';
    ALTER TABLE SystemSettings ADD NightShiftStartTime TIME NOT NULL DEFAULT '18:00:00';
    ALTER TABLE SystemSettings ADD NightShiftEndTime TIME NOT NULL DEFAULT '02:00:00';
END
GO

-- Add override columns to StaffShifts
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('StaffShifts') AND name = 'CustomStartTime')
BEGIN
    ALTER TABLE StaffShifts ADD CustomStartTime DATETIME NULL;
    ALTER TABLE StaffShifts ADD CustomEndTime DATETIME NULL;
END
GO
