-- Check if HSilva is an admin
SELECT 
    UserId,
    Username,
    IsAdmin,
    IsActive,
    PasswordChangeRequired
FROM AppUsers
WHERE Username = 'HSilva';
