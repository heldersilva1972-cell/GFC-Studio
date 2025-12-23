-- =============================================
-- COMPLETE FIX - Bartender Schedule Database
-- Created: 2025-12-23
-- Description: Ensures all required columns and tables exist
-- RUN THIS SCRIPT TO FIX THE ERROR
-- =============================================

PRINT '========================================';
PRINT 'Starting Bartender Schedule Database Fix';
PRINT '========================================';
PRINT '';

-- Step 1: Add MemberId column to StaffMembers if missing
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StaffMembers]') AND type in (N'U'))
BEGIN
    IF NOT EXISTS (
        SELECT * FROM sys.columns 
        WHERE object_id = OBJECT_ID(N'[dbo].[StaffMembers]') 
        AND name = 'MemberId'
    )
    BEGIN
        ALTER TABLE [dbo].[StaffMembers]
        ADD [MemberId] INT NULL;
        
        PRINT '✓ Added MemberId column to StaffMembers table';
    END
    ELSE
    BEGIN
        PRINT '✓ MemberId column already exists';
    END
END
ELSE
BEGIN
    PRINT '✗ StaffMembers table does not exist - run RUN_THIS_DATABASE_SCRIPT.sql first';
END
GO

-- Step 2: Verify all required columns exist in StaffMembers
PRINT '';
PRINT 'Verifying StaffMembers table structure:';
PRINT '----------------------------------------';

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StaffMembers]') AND type in (N'U'))
BEGIN
    SELECT 
        COLUMN_NAME as ColumnName,
        DATA_TYPE as DataType,
        CHARACTER_MAXIMUM_LENGTH as MaxLength,
        IS_NULLABLE as Nullable
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'StaffMembers'
    ORDER BY ORDINAL_POSITION;
END
GO

-- Step 3: Verify StaffShifts table
PRINT '';
PRINT 'Verifying StaffShifts table structure:';
PRINT '----------------------------------------';

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StaffShifts]') AND type in (N'U'))
BEGIN
    SELECT 
        COLUMN_NAME as ColumnName,
        DATA_TYPE as DataType,
        IS_NULLABLE as Nullable
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'StaffShifts'
    ORDER BY ORDINAL_POSITION;
END
ELSE
BEGIN
    PRINT '✗ StaffShifts table does not exist - run RUN_THIS_DATABASE_SCRIPT.sql first';
END
GO

-- Step 4: Check record counts
PRINT '';
PRINT 'Current record counts:';
PRINT '----------------------------------------';

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StaffMembers]') AND type in (N'U'))
BEGIN
    SELECT 
        'StaffMembers' AS TableName,
        COUNT(*) AS RecordCount
    FROM [dbo].[StaffMembers];
END

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StaffShifts]') AND type in (N'U'))
BEGIN
    SELECT 
        'StaffShifts' AS TableName,
        COUNT(*) AS RecordCount
    FROM [dbo].[StaffShifts];
END
GO

PRINT '';
PRINT '========================================';
PRINT 'Database fix completed!';
PRINT 'You can now refresh the Bartender Schedule page';
PRINT '========================================';
GO
