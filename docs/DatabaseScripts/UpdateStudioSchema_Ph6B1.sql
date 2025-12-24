USE [ClubMembership];
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = N'Slug')
BEGIN
    ALTER TABLE [StudioPages] ADD [Slug] NVARCHAR(255) NOT NULL DEFAULT '';
END
GO

-- Create an index for faster slug lookups
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[StudioPages]') AND name = N'IX_StudioPages_Slug')
BEGIN
    CREATE UNIQUE INDEX [IX_StudioPages_Slug] ON [StudioPages] ([Slug]);
END
GO
