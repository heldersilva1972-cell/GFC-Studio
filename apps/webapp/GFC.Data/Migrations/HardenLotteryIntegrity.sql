-- GFC Lottery Data Integrity Hardening
-- This script adds a UNIQUE constraint to prevent duplicate shifts and cleans up existing orphans.

PRINT 'Starting Lottery Data Integrity Hardening...';

-- 1. CLEANUP: Identify and remove duplicate shifts
-- Keeps the latest record (by ModifiedDate or ShiftId) for every unique (Date, Type, Machine)
WITH DuplicateShifts AS (
    SELECT 
        ShiftId,
        ROW_NUMBER() OVER (
            PARTITION BY CAST(ShiftDate AS DATE), ShiftType, ISNULL(MachineId, 'MAIN')
            ORDER BY ModifiedDate DESC, ShiftId DESC
        ) as Occurrence
    FROM LotteryShifts
)
DELETE FROM LotteryShifts 
WHERE ShiftId IN (SELECT ShiftId FROM DuplicateShifts WHERE Occurrence > 1);
PRINT 'Cleanup complete. Duplicate shifts removed.';

-- 2. ENFORCE: Add Unique Constraint
IF EXISTS (SELECT * FROM sys.objects WHERE name = 'UQ_LotteryShifts_DateTypeMachine' AND type = 'UQ')
BEGIN
    ALTER TABLE LotteryShifts DROP CONSTRAINT UQ_LotteryShifts_DateTypeMachine;
END

ALTER TABLE LotteryShifts 
ADD CONSTRAINT UQ_LotteryShifts_DateTypeMachine UNIQUE (ShiftDate, ShiftType, MachineId);

PRINT 'Unique constraint added successfully.';

-- 3. REPAIR: Fix 'Day' shift math (Fresh Start Logic)
-- For Day shifts, activity is just (Current - 0) because the machine reset at night.
UPDATE LotteryShifts
SET EnvelopeAmount = 0,
    BagRefillAmount = 0,
    ShiftSalesActivity = TotalSales,
    ShiftPayoutsActivity = TotalPayouts,
    ShiftCancelsActivity = TotalCancels,
    NetSales = (TotalSales - TotalPayouts - TotalCancels),
    -- Variance = EndingCash - (StartingCash + NetSales + BackupBag)
    ExpectedCash = (StartingCash + (TotalSales - TotalPayouts - TotalCancels) + BackupBagAmount),
    Variance = EndingCash - (StartingCash + (TotalSales - TotalPayouts - TotalCancels) + BackupBagAmount)
WHERE ShiftType = 'Day';

PRINT 'Historical Day shift math repaired (Baseline reset to 0).';

-- 4. REPAIR: Fix 'Night' shift variance (Ghost Variance Fix)
-- Variance should not include the money put into envelopes/refills.
UPDATE LotteryShifts
SET Variance = EndingCash - (StartingCash + ShiftSalesActivity - ShiftPayoutsActivity - ShiftCancelsActivity + BackupBagAmount)
WHERE ShiftType = 'Night';

PRINT 'Historical Night shift variances repaired (Ghost Drops ignored).';
