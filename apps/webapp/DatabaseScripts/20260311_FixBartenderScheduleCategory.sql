-- Fix the category for Bartender Schedule to ensure it appears in the BARTENDERS section in the permissions modal
UPDATE [dbo].[AppPages] 
SET [Category] = 'BARTENDERS' 
WHERE [PageRoute] = '/admin/staff-shifts' OR [PageRoute] = '/mobile/schedule' OR [PageRoute] = '/mobile/manage-schedule';
GO

-- Verify changes
SELECT PageId, PageName, PageRoute, Category FROM AppPages WHERE PageName LIKE '%Schedule%' OR PageRoute LIKE '%schedule%';
GO
