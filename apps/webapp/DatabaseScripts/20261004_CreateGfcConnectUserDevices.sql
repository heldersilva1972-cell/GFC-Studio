-- =====================================================================
-- Migration: 20261004_CreateGfcConnectUserDevices.sql
-- Purpose: Create UserDevices table for GFC Connect mobile phone tracking,
--          push notification tokens, and register GFC Connect permission pages.
-- =====================================================================

-- 1. Create UserDevices Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserDevices')
BEGIN
    CREATE TABLE UserDevices (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        DeviceToken NVARCHAR(256) NOT NULL,
        FcmDeviceToken NVARCHAR(512) NULL,
        Platform NVARCHAR(50) NOT NULL DEFAULT 'Android',
        DeviceModel NVARCHAR(150) NULL,
        OsVersion NVARCHAR(50) NULL,
        AppVersion NVARCHAR(50) NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        LastActive DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        CONSTRAINT FK_UserDevices_AppUsers FOREIGN KEY (UserId) REFERENCES AppUsers(UserId) ON DELETE CASCADE,
        CONSTRAINT UQ_UserDevices_DeviceToken UNIQUE (DeviceToken)
    );

    CREATE NONCLUSTERED INDEX IX_UserDevices_UserId ON UserDevices(UserId);
    CREATE NONCLUSTERED INDEX IX_UserDevices_FcmToken ON UserDevices(FcmDeviceToken) WHERE FcmDeviceToken IS NOT NULL;
    PRINT 'Created UserDevices table successfully.';
END
ELSE
BEGIN
    PRINT 'UserDevices table already exists.';
END
GO

-- 2. Register GFC Connect Pages in AppPages Table
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/gfc-connect/access')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Category, IsActive, RequiresAdmin, DisplayOrder, Description)
    VALUES ('GFC Connect: Master Access', '/gfc-connect/access', 'GFC CONNECT', 1, 0, 1000, 'Master permission enabling the member to log into the GFC Connect app');
    PRINT 'Registered GFC Connect: Master Access.';
END
GO

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/gfc-connect/hall-rentals')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Category, IsActive, RequiresAdmin, DisplayOrder, Description)
    VALUES ('GFC Connect: Hall Rentals', '/gfc-connect/hall-rentals', 'GFC CONNECT', 1, 0, 1010, 'Access to Hall Rental inquiries, rates, and management inside GFC Connect');
    PRINT 'Registered GFC Connect: Hall Rentals.';
END
GO

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/gfc-connect/door-access')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Category, IsActive, RequiresAdmin, DisplayOrder, Description)
    VALUES ('GFC Connect: Door Access', '/gfc-connect/door-access', 'GFC CONNECT', 1, 0, 1020, 'Access to digital door unlock and scan history inside GFC Connect');
    PRINT 'Registered GFC Connect: Door Access.';
END
GO

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/gfc-connect/shift-reports')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Category, IsActive, RequiresAdmin, DisplayOrder, Description)
    VALUES ('GFC Connect: Shift Reports', '/gfc-connect/shift-reports', 'GFC CONNECT', 1, 0, 1030, 'Access to bartender/staff shift closing reports inside GFC Connect');
    PRINT 'Registered GFC Connect: Shift Reports.';
END
GO

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/gfc-connect/club-news')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Category, IsActive, RequiresAdmin, DisplayOrder, Description)
    VALUES ('GFC Connect: Club News & Events', '/gfc-connect/club-news', 'GFC CONNECT', 1, 0, 1040, 'Access to club announcements, bulletins, and calendar events inside GFC Connect');
    PRINT 'Registered GFC Connect: Club News & Events.';
END
GO

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/gfc-connect/member-dues')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Category, IsActive, RequiresAdmin, DisplayOrder, Description)
    VALUES ('GFC Connect: Member Dues', '/gfc-connect/member-dues', 'GFC CONNECT', 1, 0, 1050, 'Access to membership dues status and renewal inside GFC Connect');
    PRINT 'Registered GFC Connect: Member Dues.';
END
GO
