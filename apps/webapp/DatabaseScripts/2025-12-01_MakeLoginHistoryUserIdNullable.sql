-- =============================================
-- Make LoginHistory.UserId nullable to allow
-- logging failed login attempts for non-existent users
-- =============================================
-- Run this script once against the ClubMembership database

-- Make UserId nullable if it is currently NOT NULL
IF EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE Name = N'UserId'
      AND Object_ID = Object_ID(N'dbo.LoginHistory')
      AND is_nullable = 0
)
BEGIN
    -- Drop the existing FK constraint
    IF EXISTS (
        SELECT 1
        FROM sys.foreign_keys
        WHERE name = 'FK_LoginHistory_AppUsers'
          AND parent_object_id = OBJECT_ID(N'dbo.LoginHistory')
    )
    BEGIN
        ALTER TABLE dbo.LoginHistory
        DROP CONSTRAINT FK_LoginHistory_AppUsers;
    END

    -- Make UserId nullable
    ALTER TABLE dbo.LoginHistory
    ALTER COLUMN UserId INT NULL;

    -- Re-add the FK constraint (allows NULL values)
    ALTER TABLE dbo.LoginHistory
    ADD CONSTRAINT FK_LoginHistory_AppUsers
        FOREIGN KEY (UserId)
        REFERENCES dbo.AppUsers(UserId);
        -- no cascade; NULL means "no linked user"
END
GO

PRINT 'LoginHistory.UserId is now nullable. Failed login attempts can be logged without FK violations.';
