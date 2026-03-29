USE ClubMembership;
GO

-- 1. Ensure the 'Sign-in Draw Print' page exists in AppPages
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/print-membership-draw')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Sign-in Draw Print', '/print-membership-draw', 'Generate and print the eligible member list for the periodic draw.', 'REPORTS', 0, 1, 100);
    PRINT 'Inserted: Sign-in Draw Print page';
END
ELSE
BEGIN
    UPDATE AppPages 
    SET IsActive = 1, Category = 'REPORTS'
    WHERE PageRoute = '/print-membership-draw';
    PRINT 'Verified: Sign-in Draw Print page';
END
GO
