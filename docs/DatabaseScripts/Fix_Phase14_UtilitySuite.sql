-- Comprehensive Fix for Phase 14 Utility Suite Table and Columns
-- Ensures every table and column in GfcDbContext matches the physical database

-- 1. MediaAssets Table Repair
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MediaAssets')
BEGIN
    CREATE TABLE [dbo].[MediaAssets] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [FileName] NVARCHAR(255) NOT NULL,
        [StoredFileName] NVARCHAR(255) NOT NULL,
        [ContentType] NVARCHAR(100) NOT NULL,
        [FileSize] BIGINT NOT NULL DEFAULT 0,
        [UploadedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

-- Add missing MediaAssets columns (Base and Metadata)
DECLARE @MediaAssetSQL NVARCHAR(MAX) = '';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'FileName')
    SET @MediaAssetSQL += 'ALTER TABLE [dbo].[MediaAssets] ADD [FileName] NVARCHAR(255) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'StoredFileName')
    SET @MediaAssetSQL += 'ALTER TABLE [dbo].[MediaAssets] ADD [StoredFileName] NVARCHAR(255) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'ContentType')
    SET @MediaAssetSQL += 'ALTER TABLE [dbo].[MediaAssets] ADD [ContentType] NVARCHAR(100) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'FileSize')
    SET @MediaAssetSQL += 'ALTER TABLE [dbo].[MediaAssets] ADD [FileSize] BIGINT NOT NULL DEFAULT 0;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'UploadedAt')
    SET @MediaAssetSQL += 'ALTER TABLE [dbo].[MediaAssets] ADD [UploadedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE();';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'Tag')
    SET @MediaAssetSQL += 'ALTER TABLE [dbo].[MediaAssets] ADD [Tag] NVARCHAR(100) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'UploadedBy')
    SET @MediaAssetSQL += 'ALTER TABLE [dbo].[MediaAssets] ADD [UploadedBy] NVARCHAR(MAX) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'Usage')
    SET @MediaAssetSQL += 'ALTER TABLE [dbo].[MediaAssets] ADD [Usage] NVARCHAR(MAX) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MediaAssets]') AND name = 'RequiredRole')
    SET @MediaAssetSQL += 'ALTER TABLE [dbo].[MediaAssets] ADD [RequiredRole] NVARCHAR(100) NULL;';

IF @MediaAssetSQL <> '' EXEC sp_executesql @MediaAssetSQL;
GO

-- 2. WebsiteSettings Table Repair
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'WebsiteSettings')
BEGIN
    CREATE TABLE [dbo].[WebsiteSettings] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [ClubPhone] NVARCHAR(MAX) NULL,
        [ClubAddress] NVARCHAR(MAX) NULL,
        [MasterEmailKillSwitch] BIT NULL
    );
END
GO

-- Add missing WebsiteSettings columns
DECLARE @WebSettingsSQL NVARCHAR(MAX) = '';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'MemberRate')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [MemberRate] DECIMAL(18,2) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'NonMemberRate')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [NonMemberRate] DECIMAL(18,2) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'NonProfitRate')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [NonProfitRate] DECIMAL(18,2) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'KitchenFee')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [KitchenFee] DECIMAL(18,2) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'AvEquipmentFee')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [AvEquipmentFee] DECIMAL(18,2) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SecurityDepositAmount')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [SecurityDepositAmount] DECIMAL(18,2) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'BaseFunctionHours')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [BaseFunctionHours] INT NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'AdditionalHourRate')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [AdditionalHourRate] DECIMAL(18,2) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'MaxHallRentalDurationHours')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [MaxHallRentalDurationHours] INT NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'EnableOnlineRentalsPayment')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [EnableOnlineRentalsPayment] BIT NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'PaymentGatewayUrl')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [PaymentGatewayUrl] NVARCHAR(MAX) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'PaymentGatewayApiKey')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [PaymentGatewayApiKey] NVARCHAR(MAX) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'PrimaryColor')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [PrimaryColor] NVARCHAR(20) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SecondaryColor')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [SecondaryColor] NVARCHAR(20) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'HeadingFont')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [HeadingFont] NVARCHAR(100) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'BodyFont')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [BodyFont] NVARCHAR(100) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'HighAccessibilityMode')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [HighAccessibilityMode] BIT NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'IsClubOpen')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [IsClubOpen] BIT NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SeoTitle')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [SeoTitle] NVARCHAR(MAX) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SeoDescription')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [SeoDescription] NVARCHAR(MAX) NULL;';
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SeoKeywords')
    SET @WebSettingsSQL += 'ALTER TABLE [dbo].[WebsiteSettings] ADD [SeoKeywords] NVARCHAR(MAX) NULL;';

IF @WebSettingsSQL <> '' EXEC sp_executesql @WebSettingsSQL;
GO

-- 3. DynamicForms Table Creation
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DynamicForms')
BEGIN
    CREATE TABLE [dbo].[DynamicForms] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR(100) NOT NULL,
        [SchemaJson] NVARCHAR(MAX) NOT NULL
    );
END
GO

-- 4. Maintenance / Data Sanitization
-- Now that all columns are guaranteed to exist, we can safely update them
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'MediaAssets')
BEGIN
    UPDATE [dbo].[MediaAssets] SET [UploadedAt] = GETUTCDATE() WHERE [UploadedAt] IS NULL;
    UPDATE [dbo].[MediaAssets] SET [FileSize] = 0 WHERE [FileSize] IS NULL;
    -- Set defaults for required strings if any are NULL
    UPDATE [dbo].[MediaAssets] SET [FileName] = 'unknown' WHERE [FileName] IS NULL;
    UPDATE [dbo].[MediaAssets] SET [StoredFileName] = 'unknown' WHERE [StoredFileName] IS NULL;
    UPDATE [dbo].[MediaAssets] SET [ContentType] = 'application/octet-stream' WHERE [ContentType] IS NULL;
END
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'SystemSettings')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'LanSubnet')
    BEGIN
        ALTER TABLE [dbo].[SystemSettings] ADD [LanSubnet] NVARCHAR(50) NULL;
    END
    UPDATE [dbo].[SystemSettings] SET [LanSubnet] = '192.168.1.0/24' WHERE [LanSubnet] IS NULL;
END
GO

IF EXISTS (SELECT * FROM [dbo].[WebsiteSettings])
BEGIN
    UPDATE [dbo].[WebsiteSettings] SET [PrimaryColor] = '#0D1B2A' WHERE [PrimaryColor] IS NULL;
    UPDATE [dbo].[WebsiteSettings] SET [IsClubOpen] = 1 WHERE [IsClubOpen] IS NULL;
    UPDATE [dbo].[WebsiteSettings] SET [MemberRate] = 0 WHERE [MemberRate] IS NULL;
    UPDATE [dbo].[WebsiteSettings] SET [NonMemberRate] = 0 WHERE [NonMemberRate] IS NULL;
END
ELSE
BEGIN
    INSERT INTO [dbo].[WebsiteSettings] (ClubPhone, PrimaryColor, IsClubOpen, MemberRate, NonMemberRate) 
    VALUES ('978-283-0507', '#0D1B2A', 1, 0, 0);
END
GO

PRINT '✓ Phase 14 Utility Suite Database Refresh Completed Successfully';
GO
