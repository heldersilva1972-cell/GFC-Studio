-- Test the exact query that KeyCardService.GetKeyCardEligibility uses
-- This should return 1 if Robert is considered "paid" for 2026

DECLARE @MemberId INT = 22;
DECLARE @Year INT = 2026;

-- Test 1: Check if payment exists
SELECT 
    'Payment Check' AS Test,
    CASE 
        WHEN EXISTS (
            SELECT 1
            FROM DuesPayments
            WHERE MemberID = @MemberId
              AND Year = @Year
              AND (
                  PaidDate IS NOT NULL
                  OR (PaymentType IS NOT NULL AND UPPER(PaymentType) = 'WAIVED')
              )
        )
        THEN 'PASS - Payment Found'
        ELSE 'FAIL - No Payment'
    END AS Result;

-- Test 2: Check waivers
SELECT 
    'Waiver Check' AS Test,
    CASE 
        WHEN EXISTS (
            SELECT 1
            FROM DuesWaiverPeriods
            WHERE MemberId = @MemberId
              AND @Year BETWEEN StartYear AND EndYear
        )
        THEN 'PASS - Waiver Found'
        ELSE 'FAIL - No Waiver'
    END AS Result;

-- Test 3: Check old Waivers table
SELECT 
    'Old Waiver Check' AS Test,
    CASE 
        WHEN EXISTS (
            SELECT 1
            FROM Waivers
            WHERE MemberId = @MemberId
              AND Year = @Year
        )
        THEN 'PASS - Old Waiver Found'
        ELSE 'FAIL - No Old Waiver'
    END AS Result;

-- Test 4: Final eligibility (this is what the repository returns)
SELECT 
    'Final Eligibility' AS Test,
    CASE 
        WHEN EXISTS (
            SELECT 1
            FROM DuesPayments
            WHERE MemberID = @MemberId
              AND Year = @Year
              AND (
                  PaidDate IS NOT NULL
                  OR (PaymentType IS NOT NULL AND UPPER(PaymentType) = 'WAIVED')
              )
        )
        OR EXISTS (
            SELECT 1
            FROM DuesWaiverPeriods
            WHERE MemberId = @MemberId
              AND @Year BETWEEN StartYear AND EndYear
        )
        OR EXISTS (
            SELECT 1
            FROM Waivers
            WHERE MemberId = @MemberId
              AND Year = @Year
        )
        THEN 1  -- TRUE - Should show as PAID
        ELSE 0  -- FALSE - Will show as PAST DUE
    END AS EligibilityResult;
