-- Check payment status for specific members
SELECT 
    m.MemberID,
    m.FirstName + ' ' + m.LastName AS MemberName,
    m.Status,
    
    -- 2025 Status (Previous Year)
    CASE 
        WHEN EXISTS (
            SELECT 1 FROM DuesPayments 
            WHERE MemberID = m.MemberID 
            AND Year = 2025
            AND (PaidDate IS NOT NULL OR UPPER(PaymentType) = 'WAIVED')
        ) THEN 'PAID/WAIVED'
        WHEN EXISTS (
            SELECT 1 FROM DuesWaiverPeriods 
            WHERE MemberId = m.MemberID 
            AND 2025 BETWEEN StartYear AND EndYear
        ) THEN 'WAIVER PERIOD'
        ELSE 'UNPAID'
    END AS Status2025,
    
    -- 2026 Status (Current Year)
    CASE 
        WHEN EXISTS (
            SELECT 1 FROM DuesPayments 
            WHERE MemberID = m.MemberID 
            AND Year = 2026
            AND (PaidDate IS NOT NULL OR UPPER(PaymentType) = 'WAIVED')
        ) THEN 'PAID/WAIVED'
        WHEN EXISTS (
            SELECT 1 FROM DuesWaiverPeriods 
            WHERE MemberId = m.MemberID 
            AND 2026 BETWEEN StartYear AND EndYear
        ) THEN 'WAIVER PERIOD'
        ELSE 'UNPAID'
    END AS Status2026,
    
    -- What SHOULD be displayed
    CASE 
        -- If 2026 is paid/waived, show Paid
        WHEN EXISTS (
            SELECT 1 FROM DuesPayments 
            WHERE MemberID = m.MemberID 
            AND Year = 2026
            AND (PaidDate IS NOT NULL OR UPPER(PaymentType) = 'WAIVED')
        ) THEN 'Should Show: PAID'
        
        -- If 2025 is paid/waived and 2026 is not, and grace period active, show In Grace Period
        WHEN EXISTS (
            SELECT 1 FROM DuesPayments 
            WHERE MemberID = m.MemberID 
            AND Year = 2025
            AND (PaidDate IS NOT NULL OR UPPER(PaymentType) = 'WAIVED')
        ) AND NOT EXISTS (
            SELECT 1 FROM DuesPayments 
            WHERE MemberID = m.MemberID 
            AND Year = 2026
            AND (PaidDate IS NOT NULL OR UPPER(PaymentType) = 'WAIVED')
        ) THEN 'Should Show: IN GRACE PERIOD'
        
        ELSE 'Should Show: PAST DUE'
    END AS ExpectedDisplay

FROM Members m
WHERE m.MemberID IN (73, 23, 18, 3) -- Dan Silva, Darryl Brown, Thomas Alves, Dan Fialho
ORDER BY m.MemberID;

-- Show actual DuesPayments records for these members
SELECT 
    dp.MemberID,
    m.FirstName + ' ' + m.LastName AS MemberName,
    dp.Year,
    dp.Amount,
    dp.PaidDate,
    dp.PaymentType,
    dp.Notes
FROM DuesPayments dp
LEFT JOIN Members m ON dp.MemberID = m.MemberID
WHERE dp.MemberID IN (73, 23, 18, 3)
ORDER BY dp.MemberID, dp.Year DESC;

-- Check grace period settings
SELECT 
    Year,
    GraceEndDate,
    CASE 
        WHEN GraceEndDate IS NULL THEN 'NO GRACE PERIOD SET'
        WHEN GETDATE() <= GraceEndDate THEN 'GRACE PERIOD ACTIVE'
        ELSE 'GRACE PERIOD EXPIRED'
    END AS GraceStatus,
    DATEDIFF(DAY, GETDATE(), GraceEndDate) AS DaysRemaining
FROM DuesYearSettings
WHERE Year = 2026;
