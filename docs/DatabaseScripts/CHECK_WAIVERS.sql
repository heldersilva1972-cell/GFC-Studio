-- Check if DuesWaiverPeriods table exists and has data
SELECT 'DuesWaiverPeriods Table' AS TableName, COUNT(*) AS RecordCount
FROM DuesWaiverPeriods;

-- Show all waiver periods
SELECT 
    dwp.MemberId,
    m.FirstName + ' ' + m.LastName AS MemberName,
    dwp.StartYear,
    dwp.EndYear,
    dwp.Reason,
    dwp.CreatedDate
FROM DuesWaiverPeriods dwp
LEFT JOIN Members m ON dwp.MemberId = m.MemberID
ORDER BY dwp.MemberId, dwp.StartYear;

-- Check if members have waivers for 2025 (previous year)
SELECT 
    m.MemberID,
    m.FirstName + ' ' + m.LastName AS MemberName,
    CASE 
        WHEN EXISTS (
            SELECT 1 FROM DuesWaiverPeriods 
            WHERE MemberId = m.MemberID 
            AND 2025 BETWEEN StartYear AND EndYear
        ) THEN 'YES - Has 2025 Waiver'
        ELSE 'NO - No 2025 Waiver'
    END AS Has2025Waiver,
    CASE 
        WHEN EXISTS (
            SELECT 1 FROM DuesPayments 
            WHERE MemberID = m.MemberID 
            AND Year = 2026
            AND (PaidDate IS NOT NULL OR UPPER(PaymentType) = 'WAIVED')
        ) THEN 'PAID/WAIVED'
        ELSE 'UNPAID'
    END AS Status2026
FROM Members m
WHERE m.Status IN ('REGULAR', 'REGULAR-NP')
  AND m.MemberID IN (73, 23, 18) -- Dan Silva, Darryl Brown, Thomas Alves
ORDER BY m.MemberID;
