IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MigrationProfiles]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[MigrationProfiles](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](max) NOT NULL,
        [Mode] [int] NOT NULL,
        [CreatedAtUtc] [datetime2](7) NOT NULL,
        [IsCompleted] [bit] NOT NULL,
        [GatesStatusJson] [nvarchar](max) NULL,
        [ReportContentTxt] [nvarchar](max) NULL,
        [ReportGeneratedAtUtc] [datetime2](7) NULL,
     CONSTRAINT [PK_MigrationProfiles] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

-- Idempotent column additions for existing tables
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MigrationProfiles]') AND name = 'ReportContentTxt')
BEGIN
    ALTER TABLE [dbo].[MigrationProfiles] ADD [ReportContentTxt] [nvarchar](max) NULL;
    ALTER TABLE [dbo].[MigrationProfiles] ADD [ReportGeneratedAtUtc] [datetime2](7) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SystemSettings]') AND name = 'HostingEnvironment')
BEGIN
    ALTER TABLE [SystemSettings] ADD [HostingEnvironment] nvarchar(MAX) NOT NULL DEFAULT N'Development';
END
GO
