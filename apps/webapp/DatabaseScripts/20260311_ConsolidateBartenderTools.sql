-- Fix duplication and categorization of Bartender tools
-- Desktop Tools
UPDATE [dbo].[AppPages] 
SET [Category] = 'BARTENDERS', [PageName] = 'End of Shift Sales' 
WHERE [PageRoute] = '/operations/end-of-shift';

UPDATE [dbo].[AppPages] 
SET [Category] = 'BARTENDERS', [PageName] = 'Bartender Schedule' 
WHERE [PageRoute] = '/admin/staff-shifts';

-- Mobile Tools: Move to MOBILE category to avoid clutter in the main group
UPDATE [dbo].[AppPages] 
SET [Category] = 'MOBILE', [PageName] = 'Mobile: Bartender Schedule' 
WHERE [PageRoute] = '/mobile/schedule';

UPDATE [dbo].[AppPages] 
SET [Category] = 'MOBILE', [PageName] = 'Mobile: Schedule Manager' 
WHERE [PageRoute] = '/mobile/manage-schedule';

UPDATE [dbo].[AppPages] 
SET [Category] = 'MOBILE', [PageName] = 'Mobile: Shift Report' 
WHERE [PageRoute] = '/mobile/shift-report';

-- Fix Liquor tools category
UPDATE [dbo].[AppPages]
SET [Category] = 'MOBILE'
WHERE [PageRoute] LIKE '/mobile/liquor/%';

GO
