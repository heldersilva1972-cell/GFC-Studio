-- Update NVR credentials in SystemSettings table
-- This fixes the verification to use port 80 instead of 8000

USE ClubMembership;
GO

UPDATE SystemSettings 
SET 
    NvrIpAddress = '192.168.1.64',
    NvrPort = 80,
    NvrUsername = 'admin',
    NvrPassword = 'Gcfclub1923'
WHERE Id = 1;
GO

-- Verify the update
SELECT NvrIpAddress, NvrPort, NvrUsername, 'Password is set' AS PasswordStatus
FROM SystemSettings 
WHERE Id = 1;
GO

PRINT 'NVR credentials updated successfully!';
