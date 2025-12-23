-- Database Fix Script for EventPromotions
-- Run this script if the Event Promotions page is failing to load

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'EventPromotions')
BEGIN
    CREATE TABLE [dbo].[EventPromotions] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Title] NVARCHAR(MAX) NOT NULL,
        [EventDate] DATETIME2(7) NOT NULL,
        [ImageUrl] NVARCHAR(MAX) NULL,
        [Skin] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_EventPromotions] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO
