-- Migration: Add PhysicalTokensJson and ShiftDrinkJson to PosZReports
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('PosZReports') AND name = 'PhysicalTokensJson')
BEGIN
    ALTER TABLE PosZReports ADD PhysicalTokensJson NVARCHAR(MAX) NULL;
    PRINT 'Added PhysicalTokensJson column to PosZReports.';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('PosZReports') AND name = 'ShiftDrinkJson')
BEGIN
    ALTER TABLE PosZReports ADD ShiftDrinkJson NVARCHAR(MAX) NULL;
    PRINT 'Added ShiftDrinkJson column to PosZReports.';
END
