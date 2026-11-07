USE [ClubMembership];
GO

-- Migration: Add Drink Comps tracking columns to PosZReports
-- Date: 2026-11-07

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.PosZReports') AND name = 'BirthdayCompsCount')
BEGIN
    ALTER TABLE dbo.PosZReports ADD BirthdayCompsCount INT NOT NULL CONSTRAINT DF_PosZReports_BirthdayCompsCount DEFAULT (0);
    PRINT 'Added BirthdayCompsCount column to PosZReports.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.PosZReports') AND name = 'BirthdayCompsValue')
BEGIN
    ALTER TABLE dbo.PosZReports ADD BirthdayCompsValue DECIMAL(18,2) NOT NULL CONSTRAINT DF_PosZReports_BirthdayCompsValue DEFAULT (0.00);
    PRINT 'Added BirthdayCompsValue column to PosZReports.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.PosZReports') AND name = 'GeneralCompsCount')
BEGIN
    ALTER TABLE dbo.PosZReports ADD GeneralCompsCount INT NOT NULL CONSTRAINT DF_PosZReports_GeneralCompsCount DEFAULT (0);
    PRINT 'Added GeneralCompsCount column to PosZReports.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.PosZReports') AND name = 'GeneralCompsValue')
BEGIN
    ALTER TABLE dbo.PosZReports ADD GeneralCompsValue DECIMAL(18,2) NOT NULL CONSTRAINT DF_PosZReports_GeneralCompsValue DEFAULT (0.00);
    PRINT 'Added GeneralCompsValue column to PosZReports.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.PosZReports') AND name = 'CompsSummaryJson')
BEGIN
    ALTER TABLE dbo.PosZReports ADD CompsSummaryJson NVARCHAR(MAX) NULL;
    PRINT 'Added CompsSummaryJson column to PosZReports.';
END
GO

