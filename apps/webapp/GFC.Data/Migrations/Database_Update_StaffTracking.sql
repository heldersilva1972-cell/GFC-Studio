-- DATABASE UPDATE: Employee Records & Payroll Integration
-- This script ensures the AppUsers table has the HourlyRate column for payroll tracking.
-- Run this to allow setting pay rates directly in the Employee Hours report.

-- 1. Ensure AppUsers has HourlyRate (decima)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND name = 'HourlyRate')
BEGIN
    ALTER TABLE [dbo].[AppUsers] ADD [HourlyRate] DECIMAL(18,2) NULL;
    PRINT 'Added HourlyRate column to AppUsers';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND name = 'IsTrackedEmployee')
BEGIN
    ALTER TABLE [dbo].[AppUsers] ADD [IsTrackedEmployee] BIT NOT NULL DEFAULT 0;
    PRINT 'Added IsTrackedEmployee column to AppUsers';
END

GO

-- Verify updated schema
PRINT '--- Current User Payroll Schema Verify ---';
SELECT 
    'AppUsers' as TableName,
    c.name AS ColumnName,
    t.name AS DataType
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE c.object_id = OBJECT_ID(N'[dbo].[AppUsers]')
AND c.name IN ('UserId', 'Username', 'HourlyRate', 'IsTrackedEmployee', 'IsAdmin')
ORDER BY ColumnName;
