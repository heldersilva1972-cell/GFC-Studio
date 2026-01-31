-- ============================================================
-- Migration: 20260130_ShiftSalesEnhancements.sql
-- Description: Adds "Shift Sales Alert" permission and audit fields
-- ============================================================

USE ClubMembership;
GO

-- 1. Add "Shift Sales Alert" to AppPages
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/widgets/shift-sales-alert')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, DisplayOrder)
    VALUES ('Shift Sales Alert', '/widgets/shift-sales-alert', 'Visibility for missing shift sales alerts on dashboard', 'MOBILE', 0, 100);
    PRINT 'Added "Shift Sales Alert" permission to AppPages.';
END
ELSE
BEGIN
    PRINT '"Shift Sales Alert" permission already exists.';
END
GO

-- 2. Add audit fields to BarSaleEntries
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('BarSaleEntries') AND name = 'OriginalTotalSales')
BEGIN
    ALTER TABLE BarSaleEntries ADD OriginalTotalSales DECIMAL(18,2) NULL;
    ALTER TABLE BarSaleEntries ADD ModifiedBy NVARCHAR(100) NULL;
    ALTER TABLE BarSaleEntries ADD ModifiedDate DATETIME2 NULL;
    PRINT 'Added audit fields to BarSaleEntries.';
END
GO

-- 3. Add audit fields to LotteryShifts
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('LotteryShifts') AND name = 'OriginalTotalSales')
BEGIN
    ALTER TABLE LotteryShifts ADD OriginalTotalSales DECIMAL(18,2) NULL;
    PRINT 'Added OriginalTotalSales to LotteryShifts.';
END
GO
