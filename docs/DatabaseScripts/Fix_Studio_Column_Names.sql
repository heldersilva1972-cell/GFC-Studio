
-- RENAME PageId to StudioPageId in StudioSections
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioSections]') AND name = 'PageId')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioSections]') AND name = 'StudioPageId')
    BEGIN
        EXEC sp_rename 'StudioSections.PageId', 'StudioPageId', 'COLUMN';
    END
END
GO

-- RENAME PageId to StudioPageId in StudioDrafts
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioDrafts]') AND name = 'PageId')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioDrafts]') AND name = 'StudioPageId')
    BEGIN
        EXEC sp_rename 'StudioDrafts.PageId', 'StudioPageId', 'COLUMN';
    END
END
GO

-- Ensure Foreign Keys match the new name (if we just renamed, the old FK might be broken or named confusingly, but SQL Server handles column renames usually).
-- However, we should double check constraints.

-- ADD StudioPageId if missing (fallback if rename didn't happen because PageId didn't exist but table was empty)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioSections]') AND name = 'StudioPageId')
BEGIN
    ALTER TABLE [StudioSections] ADD [StudioPageId] int NOT NULL DEFAULT 0;
    -- Note: If table has data, this might fail without a default, but we added DEFAULT 0
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[StudioDrafts]') AND name = 'StudioPageId')
BEGIN
    ALTER TABLE [StudioDrafts] ADD [StudioPageId] int NOT NULL DEFAULT 0;
END
GO

-- Verify Columns
SELECT TOP 1 * FROM [StudioSections];
SELECT TOP 1 * FROM [StudioDrafts];
