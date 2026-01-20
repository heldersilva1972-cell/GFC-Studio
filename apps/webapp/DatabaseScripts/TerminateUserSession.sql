-- Script to terminate a user's active session
-- This will force them to log in again

-- Option 1: Revoke all device trust tokens for a specific user
-- Replace @UserId with the actual user ID
DECLARE @UserId INT = 1; -- Change this to the target user ID

-- Delete all trusted devices for this user
DELETE FROM TrustedDevices WHERE UserId = @UserId;
PRINT 'All device tokens revoked for UserId: ' + CAST(@UserId AS NVARCHAR(10));

-- Option 2: Revoke all passkeys (biometric logins) for a user
-- Uncomment if you also want to revoke their biometric access
-- DELETE FROM UserPasskeys WHERE UserId = @UserId;
-- PRINT 'All passkeys revoked for UserId: ' + CAST(@UserId AS NVARCHAR(10));

-- Option 3: Force password change on next login
UPDATE AppUsers 
SET PasswordChangeRequired = 1 
WHERE UserId = @UserId;
PRINT 'Password change required flag set for UserId: ' + CAST(@UserId AS NVARCHAR(10));

-- Option 4: Deactivate the user account entirely
-- UPDATE AppUsers SET IsActive = 0 WHERE UserId = @UserId;
-- PRINT 'User account deactivated for UserId: ' + CAST(@UserId AS NVARCHAR(10));

-- Verify the changes
SELECT 
    UserId,
    Username,
    IsActive,
    PasswordChangeRequired,
    LastLoginDate
FROM AppUsers
WHERE UserId = @UserId;
