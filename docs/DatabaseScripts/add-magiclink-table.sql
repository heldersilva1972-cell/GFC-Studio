-- [NEW]
-- This script creates the MagicLinkTokens table for the Magic Link Authentication feature.

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MagicLinkTokens]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MagicLinkTokens](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Token] [nvarchar](128) NOT NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[ExpiresAtUtc] [datetime2](7) NOT NULL,
	[IsUsed] [bit] NOT NULL,
	[IpAddress] [nvarchar](45) NULL,
 CONSTRAINT [PK_MagicLinkTokens] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[MagicLinkTokens]') AND name = N'IX_MagicLinkTokens_Token')
CREATE NONCLUSTERED INDEX [IX_MagicLinkTokens_Token] ON [dbo].[MagicLinkTokens]
(
	[Token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MagicLinkTokens]  WITH CHECK ADD  CONSTRAINT [FK_MagicLinkTokens_AppUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AppUsers] ([UserId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[MagicLinkTokens] CHECK CONSTRAINT [FK_MagicLinkTokens_AppUsers_UserId]
GO
