
-- Fix missing columns in StudioDrafts
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioDrafts]') AND name = 'ChangeDescription')
BEGIN
    ALTER TABLE [StudioDrafts] ADD [ChangeDescription] nvarchar(500) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioDrafts]') AND name = 'PublishedAt')
BEGIN
    ALTER TABLE [StudioDrafts] ADD [PublishedAt] datetime2 NULL;
END
GO

-- Fix missing columns in StudioSections
-- Based on StudioSection.cs model
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioSections]') AND name = 'AnimationSettingsJson')
BEGIN
    ALTER TABLE [StudioSections] ADD [AnimationSettingsJson] nvarchar(max) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioSections]') AND name = 'VisibleOnDesktop')
BEGIN
    ALTER TABLE [StudioSections] ADD [VisibleOnDesktop] bit NOT NULL DEFAULT 1;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioSections]') AND name = 'VisibleOnTablet')
BEGIN
    ALTER TABLE [StudioSections] ADD [VisibleOnTablet] bit NOT NULL DEFAULT 1;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioSections]') AND name = 'VisibleOnMobile')
BEGIN
    ALTER TABLE [StudioSections] ADD [VisibleOnMobile] bit NOT NULL DEFAULT 1;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioSections]') AND name = 'IsVisible')
BEGIN
    ALTER TABLE [StudioSections] ADD [IsVisible] bit NOT NULL DEFAULT 1;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioSections]') AND name = 'OrderIndex')
BEGIN
    ALTER TABLE [StudioSections] ADD [OrderIndex] int NOT NULL DEFAULT 0;
END
GO

-- Audit columns for StudioSections
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioSections]') AND name = 'CreatedAt')
BEGIN
    ALTER TABLE [StudioSections] ADD [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE();
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioSections]') AND name = 'CreatedBy')
BEGIN
    ALTER TABLE [StudioSections] ADD [CreatedBy] nvarchar(100) NOT NULL DEFAULT 'System';
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioSections]') AND name = 'UpdatedAt')
BEGIN
    ALTER TABLE [StudioSections] ADD [UpdatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE();
END
GO
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioSections]') AND name = 'UpdatedBy')
BEGIN
    ALTER TABLE [StudioSections] ADD [UpdatedBy] nvarchar(100) NOT NULL DEFAULT 'System';
END
GO
