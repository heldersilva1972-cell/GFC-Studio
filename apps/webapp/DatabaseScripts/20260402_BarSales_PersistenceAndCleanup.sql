/* 
  SQL Script: 20260402_BarSales_PersistenceAndCleanup.sql
  Description: 
    1. Consolidates duplicate Bar Sales entries by keeping the latest/submitted version.
    2. Ensures persistent history fields (HourlyRate, Taxes) are initialized for reporting.
*/

-- 1. IDENTIFY DUPLICATES (Same Effective Date, Shift, and Hall status)
-- We use a CTE to rank records: 
-- Favor 'Submitted' over 'Draft', then favor latest created.
WITH DuplicateRank AS (
    SELECT 
        Id,
        SaleDate,
        AdjustedSaleDate,
        Shift,
        IsRentalHall,
        Status,
        CreatedAt,
        ROW_NUMBER() OVER (
            PARTITION BY 
                CAST(ISNULL(AdjustedSaleDate, SaleDate) AS DATE), 
                Shift, 
                IsRentalHall 
            ORDER BY 
                CASE WHEN Status = 'Submitted' THEN 0 ELSE 1 END,
                CreatedAt DESC
        ) as DuplicateRankIndex
    FROM BarSaleEntries
    WHERE IsDeleted = 0
)
-- 2. SECURE DELETION of redundant rows 
-- (Keeps only rank #1 for each (Date/Shift/Hall) bucket)
DELETE FROM BarSaleEntries
WHERE Id IN (
    SELECT Id 
    FROM DuplicateRank 
    WHERE DuplicateRankIndex > 1
);
PRINT 'Duplicates consolidated.';

-- 3. INITIALIZE PERSISTENT CALCULATION FIELDS (Gold Standard Audit Trail)
-- This ensures records from before the fix can still participate in labor cost analytics.
UPDATE BarSaleEntries
SET 
    HourlyRate_AtTimeOfShift = COALESCE(HourlyRate_AtTimeOfShift, 0),
    TotalEmployeeTaxes_AtTimeOfShift = COALESCE(TotalEmployeeTaxes_AtTimeOfShift, 0),
    TotalEmployerTaxes_AtTimeOfShift = COALESCE(TotalEmployerTaxes_AtTimeOfShift, 0)
WHERE 
    HourlyRate_AtTimeOfShift IS NULL OR 
    TotalEmployeeTaxes_AtTimeOfShift IS NULL OR 
    TotalEmployerTaxes_AtTimeOfShift IS NULL;

PRINT 'Persistence fields initialized.';

-- 4. VALIDATE LOG (Optional diagnostic)
-- SELECT COUNT(*), CAST(ISNULL(AdjustedSaleDate, SaleDate) AS DATE), Shift, IsRentalHall
-- FROM BarSaleEntries
-- GROUP BY CAST(ISNULL(AdjustedSaleDate, SaleDate) AS DATE), Shift, IsRentalHall
-- HAVING COUNT(*) > 1;

GO
