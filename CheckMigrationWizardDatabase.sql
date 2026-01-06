-- =============================================
-- Migration Wizard Database Diagnostic Script
-- =============================================
-- This script checks if the Migration Wizard database schema is correctly set up

PRINT '========================================';
PRINT 'Migration Wizard Database Diagnostic';
PRINT '========================================';
PRINT '';

-- Check if MigrationProfiles table exists
PRINT '1. Checking if MigrationProfiles table exists...';
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'MigrationProfiles')
BEGIN
    PRINT '   ✓ MigrationProfiles table EXISTS';
    
    -- Show table structure
    PRINT '';
    PRINT '2. MigrationProfiles table structure:';
    SELECT 
        COLUMN_NAME,
        DATA_TYPE,
        CHARACTER_MAXIMUM_LENGTH,
        IS_NULLABLE,
        COLUMN_DEFAULT
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'MigrationProfiles'
    ORDER BY ORDINAL_POSITION;
    
    -- Show row count
    DECLARE @rowCount INT;
    SELECT @rowCount = COUNT(*) FROM MigrationProfiles;
    PRINT '';
    PRINT '3. Number of migration profiles: ' + CAST(@rowCount AS VARCHAR(10));
    
    -- Show existing profiles
    IF @rowCount > 0
    BEGIN
        PRINT '';
        PRINT '4. Existing migration profiles:';
        SELECT 
            Id,
            Name,
            Mode,
            CreatedAtUtc,
            IsCompleted,
            CompletedAtUtc
        FROM MigrationProfiles
        ORDER BY CreatedAtUtc DESC;
    END
    ELSE
    BEGIN
        PRINT '';
        PRINT '4. No migration profiles found (table is empty)';
    END
END
ELSE
BEGIN
    PRINT '   ✗ MigrationProfiles table DOES NOT EXIST';
    PRINT '';
    PRINT '   ACTION REQUIRED:';
    PRINT '   Run the migration script:';
    PRINT '   sqlcmd -S .\ClubMembership -d ClubMembership -i "docs\DatabaseScripts\Manual_MigrationWizard_Schema.sql"';
END

PRINT '';
PRINT '========================================';
PRINT 'Diagnostic Complete';
PRINT '========================================';
