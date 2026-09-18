-- Step 1: Check what sales are found for Night Shift
SELECT 
    s.Id,
    s.Timestamp,
    s.BartenderName,
    s.PaymentType,
    s.TotalAmount
FROM PosSales s
WHERE CAST(s.Timestamp AS DATE) = '2026-09-17'
  AND s.Timestamp >= '2026-09-17 17:00:00'
  AND (s.IsVoided = 0 OR s.IsVoided IS NULL);
