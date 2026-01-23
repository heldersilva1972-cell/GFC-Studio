-- Recreate the legacy Waivers table with the correct schema
-- The code expects 'Id' as the primary key column name

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Waivers]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[Waivers];
END

CREATE TABLE [dbo].[Waivers](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [MemberId] [int] NOT NULL,
    [Year] [int] NOT NULL,
    [Reason] [nvarchar](500) NULL,
    CONSTRAINT [PK_Waivers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE INDEX [IX_Waivers_MemberId_Year] ON [dbo].[Waivers] ([MemberId], [Year]);

PRINT 'Waivers table recreated successfully with correct ID column.';
GO
