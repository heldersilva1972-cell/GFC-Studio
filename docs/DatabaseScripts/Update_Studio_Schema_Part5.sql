-- Script to update GFC Database Schema - Part 5
-- Adds DataBindingJson column to Sections table.

-- 1. Add DataBindingJson column to Sections table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Sections]') AND name = 'DataBindingJson')
BEGIN
    PRINT 'Adding DataBindingJson column to Sections table...'
    ALTER TABLE [Sections] ADD [DataBindingJson] NVARCHAR(MAX) NULL DEFAULT '{}';
END
ELSE
BEGIN
    PRINT 'DataBindingJson column already exists in Sections.'
END
GO
