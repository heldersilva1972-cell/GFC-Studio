-- Check the PasswordChangeRequired value for user HSilva
SELECT 
    UserId,
    Username,
    PasswordChangeRequired,
    CreatedDate,
    LastLoginDate
FROM AppUsers
WHERE Username = 'HSilva';
