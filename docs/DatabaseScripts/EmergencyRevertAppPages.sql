-- EMERGENCY REVERT: Restore ALL CAPS categories to match NavMenu.razor expectations
UPDATE AppPages SET Category = 'FINANCE' WHERE Category = 'Finance';
UPDATE AppPages SET Category = 'MEMBERSHIP' WHERE Category = 'Membership';
UPDATE AppPages SET Category = 'CONTROLLERS' WHERE Category = 'Controllers';
UPDATE AppPages SET Category = 'ADMINISTRATION' WHERE Category = 'Administration';
UPDATE AppPages SET Category = 'WEBSITE' WHERE Category = 'Website';
UPDATE AppPages SET Category = 'HALL RENTALS' WHERE Category = 'Hall Rentals';
UPDATE AppPages SET Category = 'SYSTEM' WHERE Category = 'System';
UPDATE AppPages SET Category = 'CAMERA SYSTEM' WHERE Category = 'Camera System';
UPDATE AppPages SET Category = 'GFC STUDIO' WHERE Category = 'GFC Studio';

-- Ensure Financial Insights is present and correctly categorized
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/finance/insights')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Financial Insights', '/finance/insights', 'In-depth financial performance analysis', 'FINANCE', 0, 1, 36);
END
ELSE
BEGIN
    UPDATE AppPages SET Category = 'FINANCE', IsActive = 1 WHERE PageRoute = '/finance/insights';
END

GO
