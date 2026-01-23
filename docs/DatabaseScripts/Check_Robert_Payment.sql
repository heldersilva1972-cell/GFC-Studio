-- Diagnostic query to check Robert's 2026 payment
SELECT 
    MemberId,
    Year,
    Amount,
    PaidDate,
    CASE WHEN PaidDate IS NULL THEN 'NULL' ELSE 'NOT NULL' END AS PaidDateStatus,
    PaymentType,
    CASE WHEN PaymentType IS NULL THEN 'NULL' ELSE 'NOT NULL' END AS PaymentTypeStatus,
    Notes
FROM DuesPayments
WHERE MemberId = 22 AND Year = 2026;

-- Check if the payment satisfies the eligibility check
SELECT 
    CASE 
        WHEN EXISTS (
            SELECT 1
            FROM DuesPayments
            WHERE MemberId = 22
              AND Year = 2026
              AND (
                  PaidDate IS NOT NULL
                  OR (PaymentType IS NOT NULL AND UPPER(PaymentType) = 'WAIVED')
              )
        )
        THEN 'SHOULD BE PAID'
        ELSE 'WILL SHOW PAST DUE'
    END AS EligibilityStatus;
