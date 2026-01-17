-- Manually set PasswordChangeRequired to 0 for HSilva
UPDATE AppUsers
SET PasswordChangeRequired = 0
WHERE Username = 'HSilva';

-- Verify the update
SELECT 
    UserId,
    Username,
    PasswordChangeRequired,
    CreatedDate,
    LastLoginDate
FROM AppUsers
WHERE Username = 'HSilva';
