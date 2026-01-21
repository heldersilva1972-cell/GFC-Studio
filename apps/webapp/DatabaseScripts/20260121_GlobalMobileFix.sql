-- Global fix for Mobile pages: Ensure they are in the 'Mobile' category and accessible to non-admins if granted.
UPDATE [dbo].[AppPages]
SET [Category] = 'Mobile', 
    [RequiresAdmin] = 0
WHERE [PageRoute] LIKE '/mobile/%';

-- Also ensure 'Manage Schedule' which might be the mobile version of staff shifts is unlocked
UPDATE [dbo].[AppPages]
SET [Category] = 'Mobile',
    [RequiresAdmin] = 0
WHERE [PageName] LIKE '%Manage Schedule%' OR [PageName] LIKE '%Schedule Manager%';
