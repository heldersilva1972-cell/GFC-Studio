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

-- 2. Add Massachusetts Payroll Tax Configuration to SystemSettings (Global)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MaStateTaxRate')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [MaStateTaxRate] DECIMAL(18,4) NOT NULL DEFAULT 5.0;
    ALTER TABLE [dbo].[SystemSettings] ADD [PfmlEmployeeRate] DECIMAL(18,4) NOT NULL DEFAULT 0.35;
    ALTER TABLE [dbo].[SystemSettings] ADD [PfmlEmployerRate] DECIMAL(18,4) NOT NULL DEFAULT 0.53;
    ALTER TABLE [dbo].[SystemSettings] ADD [FicaEmployeeRate] DECIMAL(18,4) NOT NULL DEFAULT 7.65;
    ALTER TABLE [dbo].[SystemSettings] ADD [FicaEmployerRate] DECIMAL(18,4) NOT NULL DEFAULT 7.65;
    ALTER TABLE [dbo].[SystemSettings] ADD [MaUnemploymentRate] DECIMAL(18,4) NOT NULL DEFAULT 2.42;
    PRINT 'Added Payroll Tax configuration columns to SystemSettings';
END

GO

-- Verify updated schema
PRINT '--- Payroll Schema Verify (AppUsers) ---';
SELECT 'AppUsers' TableName, name, type_name(user_type_id) type FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.AppUsers') AND name IN ('HourlyRate', 'IsTrackedEmployee');

PRINT '--- Payroll Schema Verify (SystemSettings) ---';
SELECT 'SystemSettings' TableName, name, type_name(user_type_id) type FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.SystemSettings') AND name LIKE '%Rate%';
