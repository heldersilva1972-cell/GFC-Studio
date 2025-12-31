UPDATE Controllers 
SET IpAddress = '192.168.0.196' 
WHERE SerialNumber = 223213880;

UPDATE ControllerNetworkConfigs 
SET IpAddress = '192.168.0.196', Gateway = '192.168.0.1', SubnetMask = '255.255.255.0'
WHERE ControllerId = (SELECT Id FROM Controllers WHERE SerialNumber = 223213880);

SELECT * FROM Controllers WHERE SerialNumber = 223213880;
