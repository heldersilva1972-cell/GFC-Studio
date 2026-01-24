USE [ClubMembership];
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserPageUsage]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[UserPageUsage](
        [Id] INT IDENTITY(1,1) NOT NULL,
        [UserId] INT NOT NULL,
        [PageIdentifier] NVARCHAR(100) NOT NULL,
        [UsageCount] INT NOT NULL DEFAULT 1,
        [LastUsedUtc] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_UserPageUsage] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    -- Foreign Key to AppUsers
    ALTER TABLE [dbo].[UserPageUsage]  WITH CHECK ADD  CONSTRAINT [FK_UserPageUsage_AppUsers] FOREIGN KEY([UserId])
    REFERENCES [dbo].[AppUsers] ([UserId])
    ON DELETE CASCADE;

    ALTER TABLE [dbo].[UserPageUsage] CHECK CONSTRAINT [FK_UserPageUsage_AppUsers];

    -- Unique constraint for Upsert logic
    CREATE UNIQUE NONCLUSTERED INDEX [IX_UserPageUsage_User_Page] ON [dbo].[UserPageUsage]
    (
        [UserId] ASC,
        [PageIdentifier] ASC
    );

    PRINT '✓ Created UserPageUsage table';
END
ELSE
BEGIN
    PRINT 'UserPageUsage table already exists';
END
GO
