-- Add ItemsOverrideJson column to EventTemplates table if it does not already exist
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventTemplates]') AND type in (N'U'))
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[EventTemplates]') AND name = N'ItemsOverrideJson')
    BEGIN
        ALTER TABLE [dbo].[EventTemplates] ADD [ItemsOverrideJson] NVARCHAR(MAX) NULL;
        PRINT 'Added ItemsOverrideJson column to EventTemplates.';
    END
    ELSE
    BEGIN
        PRINT 'ItemsOverrideJson column already exists in EventTemplates.';
    END
END
ELSE
BEGIN
    PRINT 'Error: EventTemplates table does not exist.';
END
GO
