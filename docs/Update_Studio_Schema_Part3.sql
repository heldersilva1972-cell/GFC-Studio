-- Script to update GFC Database Schema - Part 3
-- Adds InteractionJson column to Sections table.

-- 1. Add InteractionJson column to Sections table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Sections]') AND name = 'InteractionJson')
BEGIN
    PRINT 'Adding InteractionJson column to Sections table...'
    ALTER TABLE [Sections] ADD [InteractionJson] NVARCHAR(MAX) NULL DEFAULT '[]';
END
ELSE
BEGIN
    PRINT 'InteractionJson column already exists in Sections.'
END
GO
