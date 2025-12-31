-- Check current IP address
SELECT Id, Name, SerialNumber, IpAddress, DoorCount FROM Controllers WHERE SerialNumber = 223213880;

-- If IP is wrong, fix it back to 192.168.0.196
UPDATE Controllers 
SET IpAddress = '192.168.0.196' 
WHERE SerialNumber = 223213880 AND IpAddress != '192.168.0.196';

-- Verify the fix
SELECT Id, Name, SerialNumber, IpAddress, DoorCount FROM Controllers WHERE SerialNumber = 223213880;
