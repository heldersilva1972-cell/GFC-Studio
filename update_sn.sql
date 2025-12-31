UPDATE Controllers SET SerialNumber = 223213880, IpAddress = '192.168.1.72' WHERE Id = 1;
GO
UPDATE ControllerNetworkConfigs SET IpAddress = '192.168.1.72' WHERE ControllerId = 1;
GO
SELECT Id, Name, SerialNumber, IpAddress FROM Controllers;
GO
