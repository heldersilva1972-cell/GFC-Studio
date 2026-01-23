-- Fix the skewed timestamps that were buried at 8 AM
UPDATE ControllerEvents 
SET TimestampUtc = DATEADD(hour, 8, TimestampUtc)
WHERE TimestampUtc < '2026-01-10 14:00:00' 
AND CardNumber IS NOT NULL;