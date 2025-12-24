-- Database Fix Script for NavMenuEntries
-- Run this script if the Nav Menu Editor page is failing

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'NavMenuEntries')
BEGIN
    CREATE TABLE [dbo].[NavMenuEntries] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Label] NVARCHAR(MAX) NOT NULL,
        [Url] NVARCHAR(MAX) NOT NULL,
        [ParentId] INT NULL,
        CONSTRAINT [PK_NavMenuEntries] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO
