-- Fix categories and permissions for Mobile pages
UPDATE [dbo].[AppPages] 
SET [Category] = 'Mobile', [RequiresAdmin] = 0
WHERE [PageRoute] IN ('/mobile/liquor/manage', '/mobile/manage-schedule');

-- Ensure all other mobile-prefixed pages are also in Mobile category
UPDATE [dbo].[AppPages]
SET [Category] = 'Mobile'
WHERE [PageRoute] LIKE '/mobile/%' AND ([Category] IS NULL OR [Category] <> 'Mobile');
