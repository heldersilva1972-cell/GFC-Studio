/* 
  Studio Folder Organization Support
  Adds the 'Folder' column to StudioPages if it doesn't exist.
*/

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'StudioPages')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[StudioPages]') AND name = 'Folder')
    BEGIN
        ALTER TABLE [dbo].[StudioPages] ADD [Folder] NVARCHAR(500) NULL;
        
        -- Set default to ROOT for existing pages
        EXEC('UPDATE [dbo].[StudioPages] SET [Folder] = ''/'' WHERE [Folder] IS NULL');
        
        PRINT 'Added [Folder] column to [StudioPages] table.';
    END
    ELSE
    BEGIN
        PRINT '[Folder] column already exists in [StudioPages] table.';
    END
END
ELSE
BEGIN
    PRINT 'StudioPages table not found. It will be created by the master fix script.';
END
GO
