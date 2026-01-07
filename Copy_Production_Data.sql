-- Copy data from production to development
-- Run this on your LAPTOP after connecting to production server

-- Step 1: Create a linked server to production (if not exists)
IF NOT EXISTS (SELECT * FROM sys.servers WHERE name = 'ProductionServer')
BEGIN
    EXEC sp_addlinkedserver 
        @server = 'ProductionServer',
        @srvproduct = '',
        @provider = 'SQLNCLI',
        @datasrc = '192.168.1.72\SQLEXPRESS'
END
GO

-- Step 2: Use the local database
USE [ClubMembership];
GO

-- Step 3: Truncate local tables (CAREFUL - this deletes all local data!)
TRUNCATE TABLE [ControllerEvents];
TRUNCATE TABLE [Doors];
TRUNCATE TABLE [Controllers];
-- Add other tables as needed

-- Step 4: Copy Controllers from production
INSERT INTO [Controllers] (
    [Name], [SerialNumber], [IpAddress], [Port], [IsEnabled], 
    [DoorCount], [NetworkType], [VpnProfileId], [BackupIpAddress], 
    [BackupPort], [BackupExpiresUtc], [LastMigrationUtc]
)
SELECT 
    [Name], [SerialNumber], [IpAddress], [Port], [IsEnabled],
    [DoorCount], [NetworkType], [VpnProfileId], [BackupIpAddress],
    [BackupPort], [BackupExpiresUtc], [LastMigrationUtc]
FROM [ProductionServer].[ClubMembership].[dbo].[Controllers];

PRINT 'Controllers copied: ' + CAST(@@ROWCOUNT AS VARCHAR(10));

-- Step 5: Copy Doors from production
INSERT INTO [Doors] ([ControllerId], [DoorIndex], [Name], [IsEnabled], [Notes])
SELECT [ControllerId], [DoorIndex], [Name], [IsEnabled], [Notes]
FROM [ProductionServer].[ClubMembership].[dbo].[Doors];

PRINT 'Doors copied: ' + CAST(@@ROWCOUNT AS VARCHAR(10));

-- Step 6: Copy Members (if needed)
-- TRUNCATE TABLE [Members];
-- INSERT INTO [Members] SELECT * FROM [ProductionServer].[ClubMembership].[dbo].[Members];

PRINT 'Data copy complete!';
GO

-- Clean up linked server (optional)
-- EXEC sp_dropserver 'ProductionServer', 'droplogins';
