-- Manual migration to add ClockInTime and ClockOutTime columns to StaffShifts table
-- Run this if the columns are missing

-- Check if columns exist and add them if they don't
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[StaffShifts]') AND name = 'ClockInTime')
BEGIN
    ALTER TABLE [dbo].[StaffShifts]
    ADD [ClockInTime] datetime2(7) NULL;
    PRINT 'Added ClockInTime column';
END
ELSE
BEGIN
    PRINT 'ClockInTime column already exists';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[StaffShifts]') AND name = 'ClockOutTime')
BEGIN
    ALTER TABLE [dbo].[StaffShifts]
    ADD [ClockOutTime] datetime2(7) NULL;
    PRINT 'Added ClockOutTime column';
END
ELSE
BEGIN
    PRINT 'ClockOutTime column already exists';
END

-- Verify the columns were added
SELECT 
    c.name AS ColumnName,
    t.name AS DataType,
    c.is_nullable AS IsNullable
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE c.object_id = OBJECT_ID(N'[dbo].[StaffShifts]')
ORDER BY c.column_id;
