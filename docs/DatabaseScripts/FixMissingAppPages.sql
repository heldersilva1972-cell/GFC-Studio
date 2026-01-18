-- Manually add missing pages to AppPages table
-- These should have been discovered by PageDiscoveryService but adding manually to be sure

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/finance/insights')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Financial Insights', '/finance/insights', 'In-depth financial performance analysis', 'Finance', 0, 1, 36);
    PRINT 'Added Financial Insights page.';
END
ELSE
BEGIN
    UPDATE AppPages SET IsActive = 1 WHERE PageRoute = '/finance/insights';
    PRINT 'Reactivated Financial Insights page.';
END

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/admin/system/communications')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Text & Email Setup', '/admin/system/communications', 'Configure outbound communication channels', 'System', 1, 1, 72);
    PRINT 'Added Text & Email Setup page.';
END
ELSE
BEGIN
    UPDATE AppPages SET IsActive = 1 WHERE PageRoute = '/admin/system/communications';
    PRINT 'Reactivated Text & Email Setup page.';
END

GO
