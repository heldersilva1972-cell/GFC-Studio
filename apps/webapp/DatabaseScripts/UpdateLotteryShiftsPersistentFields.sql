/* 
  SQL Script: UpdateLotteryShiftsTable.sql
  Description: Adds persistent financial calculation fields to the LotteryShifts table 
               to ensure consistency across reporting modules.
*/

-- 1. Add new persistent result columns
ALTER TABLE LotteryShifts ADD 
    NetSales DECIMAL(18, 2) NULL,
    ExpectedCash DECIMAL(18, 2) NULL,
    Variance DECIMAL(18, 2) NULL,
    LotteryIncome DECIMAL(18, 2) NULL,
    NetIncome DECIMAL(18, 2) NULL,
    ShiftSalesActivity DECIMAL(18, 2) NULL,
    ShiftPayoutsActivity DECIMAL(18, 2) NULL,
    ShiftCancelsActivity DECIMAL(18, 2) NULL,
    ShiftNetDueActivity DECIMAL(18, 2) NULL;
GO

-- 2. Populate new columns for existing 'Submitted' records
-- For simplicity, we assume existing records were shift-specific or cumulative.
-- If they represent cumulative machine readings (standard for this system):
-- We'll need a CTE to handle the 'Night - Day' logic for activity if we want to be precise,
-- but for now we'll set defaults.

UPDATE LotteryShifts
SET 
    NetSales = TotalSales - TotalPayouts - TotalCancels,
    ExpectedCash = StartingCash + TotalSales - TotalPayouts - TotalCancels + BackupBagAmount,
    Variance = EndingCash - (StartingCash + TotalSales - TotalPayouts - TotalCancels + BackupBagAmount),
    LotteryIncome = (TotalSales - TotalPayouts - TotalCancels) - NetDue,
    NetIncome = ((TotalSales - TotalPayouts - TotalCancels) - NetDue) + (EndingCash - (StartingCash + TotalSales - TotalPayouts - TotalCancels + BackupBagAmount)),
    ShiftSalesActivity = TotalSales, -- Default for non-reconciled history
    ShiftPayoutsActivity = TotalPayouts,
    ShiftCancelsActivity = TotalCancels,
    ShiftNetDueActivity = NetDue
WHERE Status = 'Submitted';
GO

-- 3. Set defaults for Drafts or remaining records
UPDATE LotteryShifts SET NetSales = 0 WHERE NetSales IS NULL;
UPDATE LotteryShifts SET ExpectedCash = 0 WHERE ExpectedCash IS NULL;
UPDATE LotteryShifts SET Variance = 0 WHERE Variance IS NULL;
UPDATE LotteryShifts SET LotteryIncome = 0 WHERE LotteryIncome IS NULL;
UPDATE LotteryShifts SET NetIncome = 0 WHERE NetIncome IS NULL;
UPDATE LotteryShifts SET ShiftSalesActivity = 0 WHERE ShiftSalesActivity IS NULL;
UPDATE LotteryShifts SET ShiftPayoutsActivity = 0 WHERE ShiftPayoutsActivity IS NULL;
UPDATE LotteryShifts SET ShiftCancelsActivity = 0 WHERE ShiftCancelsActivity IS NULL;
UPDATE LotteryShifts SET ShiftNetDueActivity = 0 WHERE ShiftNetDueActivity IS NULL;
GO

-- 4. Apply NOT NULL constraints and defaults
ALTER TABLE LotteryShifts ALTER COLUMN NetSales DECIMAL(18, 2) NOT NULL;
ALTER TABLE LotteryShifts ADD CONSTRAINT DF_LotteryShifts_NetSales DEFAULT 0 FOR NetSales;

ALTER TABLE LotteryShifts ALTER COLUMN ExpectedCash DECIMAL(18, 2) NOT NULL;
ALTER TABLE LotteryShifts ADD CONSTRAINT DF_LotteryShifts_ExpectedCash DEFAULT 0 FOR ExpectedCash;

ALTER TABLE LotteryShifts ALTER COLUMN Variance DECIMAL(18, 2) NOT NULL;
ALTER TABLE LotteryShifts ADD CONSTRAINT DF_LotteryShifts_Variance DEFAULT 0 FOR Variance;

ALTER TABLE LotteryShifts ALTER COLUMN LotteryIncome DECIMAL(18, 2) NOT NULL;
ALTER TABLE LotteryShifts ADD CONSTRAINT DF_LotteryShifts_LotteryIncome DEFAULT 0 FOR LotteryIncome;

ALTER TABLE LotteryShifts ALTER COLUMN NetIncome DECIMAL(18, 2) NOT NULL;
ALTER TABLE LotteryShifts ADD CONSTRAINT DF_LotteryShifts_NetIncome DEFAULT 0 FOR NetIncome;

ALTER TABLE LotteryShifts ALTER COLUMN ShiftSalesActivity DECIMAL(18, 2) NOT NULL;
ALTER TABLE LotteryShifts ADD CONSTRAINT DF_LotteryShifts_ShiftSalesActivity DEFAULT 0 FOR ShiftSalesActivity;

ALTER TABLE LotteryShifts ALTER COLUMN ShiftPayoutsActivity DECIMAL(18, 2) NOT NULL;
ALTER TABLE LotteryShifts ADD CONSTRAINT DF_LotteryShifts_ShiftPayoutsActivity DEFAULT 0 FOR ShiftPayoutsActivity;

ALTER TABLE LotteryShifts ALTER COLUMN ShiftCancelsActivity DECIMAL(18, 2) NOT NULL;
ALTER TABLE LotteryShifts ADD CONSTRAINT DF_LotteryShifts_ShiftCancelsActivity DEFAULT 0 FOR ShiftCancelsActivity;

ALTER TABLE LotteryShifts ALTER COLUMN ShiftNetDueActivity DECIMAL(18, 2) NOT NULL;
ALTER TABLE LotteryShifts ADD CONSTRAINT DF_LotteryShifts_ShiftNetDueActivity DEFAULT 0 FOR ShiftNetDueActivity;
GO
