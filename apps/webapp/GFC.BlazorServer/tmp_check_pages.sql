SELECT PageId, PageName, PageRoute, Category FROM AppPages WHERE Category IN ('BARTENDERS', 'MOBILE') OR PageName LIKE '%Bartender%' OR PageRoute LIKE '%mobile%' ORDER BY Category, PageName;
GO
