-- [NEW]
-- =============================================
-- Page Permissions Tables
-- =============================================

-- AppPages table: Stores all pages in the application that can be secured
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppPages]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AppPages] (
        [PageId] INT IDENTITY(1,1) PRIMARY KEY,
        [PageName] NVARCHAR(100) NOT NULL,
        [PageRoute] NVARCHAR(255) NOT NULL,
        [Description] NVARCHAR(500) NULL,
        [Category] NVARCHAR(100) NULL,
        [RequiresAdmin] BIT NOT NULL DEFAULT 0,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [DisplayOrder] INT NOT NULL DEFAULT 0
    );
END
GO

-- PagePermissions table: Links users to the pages they can access
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PagePermissions]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[PagePermissions] (
        [PermissionId] INT IDENTITY(1,1) PRIMARY KEY,
        [UserId] INT NOT NULL,
        [PageId] INT NOT NULL,
        CONSTRAINT [FK_PagePermissions_AppUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AppUsers]([UserId]),
        CONSTRAINT [FK_PagePermissions_AppPages] FOREIGN KEY ([PageId]) REFERENCES [dbo].[AppPages]([PageId])
    );

    CREATE UNIQUE INDEX [IX_PagePermissions_UserPage] ON [dbo].[PagePermissions]([UserId], [PageId]);
END
GO

-- Insert the "Remote Camera Access" permission
IF NOT EXISTS (SELECT * FROM [dbo].[AppPages] WHERE [PageRoute] = '/cameras/view')
BEGIN
    INSERT INTO [dbo].[AppPages] ([PageName], [PageRoute], [Description], [Category], [RequiresAdmin], [IsActive], [DisplayOrder])
    VALUES ('Remote Camera Access', '/cameras/view', 'Allows remote access to camera feeds via VPN.', 'Camera & Remote Access', 0, 1, 100);
END
GO

PRINT 'Page permissions tables created successfully!';
