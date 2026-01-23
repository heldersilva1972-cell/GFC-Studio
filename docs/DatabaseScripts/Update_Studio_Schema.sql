-- Script to update GFC Database Schema for Studio V2
-- Run this against your database (e.g., ClubMembership)

-- 1. Add Styles column to StudioSections for Visual Styling
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioSections]') AND name = 'Styles')
BEGIN
    PRINT 'Adding Styles column to StudioSections table...'
    ALTER TABLE [StudioSections] ADD [Styles] NVARCHAR(MAX) NULL DEFAULT '{}';
END
ELSE
BEGIN
    PRINT 'Styles column already exists in StudioSections.'
END
GO

-- 2. Add AnimationSettingsJson column to StudioSections (if missing)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioSections]') AND name = 'AnimationSettingsJson')
BEGIN
    PRINT 'Adding AnimationSettingsJson column to StudioSections table...'
    ALTER TABLE [StudioSections] ADD [AnimationSettingsJson] NVARCHAR(MAX) NULL;
END
GO

-- 3. Ensure existing records have valid JSON for Styles
UPDATE [StudioSections] 
SET [Styles] = '{}' 
WHERE [Styles] IS NULL;

PRINT 'Schema update completed successfully.';
