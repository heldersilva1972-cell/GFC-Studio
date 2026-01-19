-- Migration: Add Mobile Support and Closing Reports
-- Added by Antigravity

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AppUsers') AND name = 'IsMobileUser')
BEGIN
    ALTER TABLE AppUsers ADD IsMobileUser BIT NOT NULL DEFAULT 0;
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ClosingReports')
BEGIN
    CREATE TABLE ClosingReports (
        Id INT PRIMARY KEY IDENTITY(1,1),
        UserId INT NOT NULL,
        ShiftType NVARCHAR(50) NOT NULL,
        BarSales DECIMAL(18, 2) NOT NULL,
        LotterySales DECIMAL(18, 2) NOT NULL DEFAULT 0,
        TotalSales DECIMAL(18, 2) NOT NULL,
        Notes NVARCHAR(MAX),
        ReportDate DATETIME NOT NULL DEFAULT GETDATE(),
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME,
        LastUpdatedByUserId INT,
        IsCorrected BIT NOT NULL DEFAULT 0,
        CorrectionReason NVARCHAR(MAX),
        CONSTRAINT FK_ClosingReports_User FOREIGN KEY (UserId) REFERENCES AppUsers(UserId)
    );
END
GO

-- Ensure Mobile page category and initial pages exist
IF NOT EXISTS (SELECT * FROM AppPages WHERE PageRoute = '/mobile')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Mobile Hub', '/mobile', 'Mobile Central Launchpad', 'Mobile', 0, 1, 100);
END

IF NOT EXISTS (SELECT * FROM AppPages WHERE PageRoute = '/mobile/shift-report')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Shift Report', '/mobile/shift-report', 'End of shift sales reporting', 'Mobile', 0, 1, 110);
END
GO
