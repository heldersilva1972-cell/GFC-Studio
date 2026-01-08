IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VpnOnboardingTokens]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[VpnOnboardingTokens](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [UserId] [int] NOT NULL,
        [Token] [nvarchar](256) NOT NULL,
        [CreatedAtUtc] [datetime2](7) NOT NULL,
        [ExpiresAtUtc] [datetime2](7) NOT NULL,
        [IsUsed] [bit] NOT NULL,
        [DeviceInfo] [nvarchar](500) NULL,
     CONSTRAINT [PK_VpnOnboardingTokens] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )
    )
END
