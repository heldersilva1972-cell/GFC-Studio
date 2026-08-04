USE ClubMembership;
GO

IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/lottery/imported-reports')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Imported Reports', '/lottery/imported-reports', 'Upload and compare Daily vs. Weekly Lottery CSV reports', 'LOTTERY', 0, 1, 110);
    PRINT 'Inserted: Lottery Imported Reports page';
END
ELSE
BEGIN
    UPDATE AppPages 
    SET IsActive = 1, Category = 'LOTTERY'
    WHERE PageRoute = '/lottery/imported-reports';
    PRINT 'Verified: Lottery Imported Reports page';
END
GO
