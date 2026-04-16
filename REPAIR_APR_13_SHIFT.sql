-- =================================================================================
-- REPAIR SCRIPT: Fix April 13th Night Shift (Darren Marques)
-- Task: Redirect $140 scheduled for Backup Bag Refill into the Envelope Deposit.
-- Reason: Staff placed the full amount ($430) into the envelope instead of splitting it.
-- =================================================================================

BEGIN TRANSACTION;

-- 1. Verify the record exists before updating
SELECT ShiftId, ShiftDate, ShiftType, EmployeeName, EnvelopeAmount, BagRefillAmount, Variance
FROM LotteryShifts
WHERE ShiftDate = '2026-04-13' 
  AND ShiftType = 'Night' 
  AND EmployeeName = 'Darren Marques';

-- 2. Perform the update
-- We move the 140 from BagRefillAmount to EnvelopeAmount
-- Total Drop remains $430 ($290 existing envelope + $140 from bag refill)
UPDATE LotteryShifts
SET 
    BagRefillAmount = 0,
    EnvelopeAmount = 430.00,
    ModifiedBy = 'SQL_Repair',
    ModifiedDate = GETDATE(),
    Notes = ISNULL(Notes, '') + ' [FIX: $140 redirected from Bag Refill to Envelope per Admin]'
WHERE ShiftDate = '2026-04-13' 
  AND ShiftType = 'Night' 
  AND EmployeeName = 'Darren Marques'
  AND BagRefillAmount = 140; -- Safety check to ensure we only update if the specific error exists

-- 3. Verify the change
SELECT ShiftId, ShiftDate, ShiftType, EmployeeName, EnvelopeAmount, BagRefillAmount, Variance
FROM LotteryShifts
WHERE ShiftDate = '2026-04-13' 
  AND ShiftType = 'Night' 
  AND EmployeeName = 'Darren Marques';

-- COMMIT or ROLLBACK
-- COMMIT; -- Run this only if the SELECT results look correct
ROLLBACK; -- Default to rollback for safety until verified
