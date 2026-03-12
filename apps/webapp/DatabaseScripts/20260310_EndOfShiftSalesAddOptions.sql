-- ============================================================
-- Migration: 20260310_EndOfShiftSalesAddOptions.sql
-- Description: Adds "TotalHours" to BarSaleEntries and ensures End of Shift Sales page is registered.
-- ============================================================

USE ClubMembership;
GO

-- 1. Add TotalHours to BarSaleEntries if it doesn't exist
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('BarSaleEntries') AND name = 'TotalHours')
BEGIN
    ALTER TABLE BarSaleEntries ADD TotalHours DECIMAL(5,2) NULL;
    PRINT 'Added TotalHours to BarSaleEntries.';
END
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('BarSaleEntries') AND name = 'IsRentalHall')
BEGIN
    ALTER TABLE BarSaleEntries ADD IsRentalHall BIT NOT NULL DEFAULT 0;
    PRINT 'Added IsRentalHall to BarSaleEntries.';
END
GO

-- 2. Register the new End of Shift Sales page
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/operations/end-of-shift')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, DisplayOrder)
    VALUES ('End of Shift Sales', '/operations/end-of-shift', 'Enhanced desktop report for bartenders to record sales and hours', 'BARTENDERS', 0, 110);
    PRINT 'Added "End of Shift Sales" page to AppPages.';
END
ELSE
BEGIN
    -- Update category to BARTENDERS if it exists elsewhere
    UPDATE AppPages SET Category = 'BARTENDERS' WHERE PageRoute = '/operations/end-of-shift';
    PRINT 'Updated category for "End of Shift Sales" page.';
END
GO

-- 3. Ensure "Mobile" shift-report is under MOBILE category
UPDATE AppPages SET Category = 'MOBILE' WHERE PageRoute = '/mobile/shift-report';
GO
