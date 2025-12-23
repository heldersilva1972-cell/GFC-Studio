-- Database Fix Script for WebsiteSettings
-- Run this script to create the WebsiteSettings table if existing migration failed

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'WebsiteSettings')
BEGIN
    CREATE TABLE [dbo].[WebsiteSettings] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [ClubPhone] NVARCHAR(MAX) NULL,
        [ClubAddress] NVARCHAR(MAX) NULL,
        [MasterEmailKillSwitch] BIT NOT NULL DEFAULT 0,
        [MemberRate] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [NonMemberRate] DECIMAL(18,2) NOT NULL DEFAULT 0,
        CONSTRAINT [PK_WebsiteSettings] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    -- Insert default row
    INSERT INTO [dbo].[WebsiteSettings] (ClubPhone, ClubAddress, MasterEmailKillSwitch, MemberRate, NonMemberRate)
    VALUES ('', '', 0, 0, 0);
END
GO
