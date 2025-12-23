-- =============================================
-- Add MemberId Column to StaffMembers Table
-- Created: 2025-12-23
-- Description: Adds the MemberId column if it doesn't exist
-- =============================================

-- Check if MemberId column exists, if not add it
IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[StaffMembers]') 
    AND name = 'MemberId'
)
BEGIN
    ALTER TABLE [dbo].[StaffMembers]
    ADD [MemberId] INT NULL;
    
    PRINT 'Added MemberId column to StaffMembers table';
END
ELSE
BEGIN
    PRINT 'MemberId column already exists in StaffMembers table';
END
GO

-- Verify the column was added
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'StaffMembers'
ORDER BY ORDINAL_POSITION;
GO

PRINT 'Column verification complete!';
GO
