-- =============================================
-- Add Waiver for William Goveny (ID: 111)
-- Years: 2024-2025
-- Reason: Kitchen hood work
-- Created: 2026-01-15
-- =============================================

-- Add waiver period for 2024-2025
INSERT INTO DuesWaiverPeriods (MemberId, StartYear, EndYear, Reason, CreatedDate)
VALUES (
    111,                    -- William Goveny's Member ID
    2024,                   -- Start Year
    2025,                   -- End Year
    'Kitchen hood work',    -- Reason
    GETDATE()               -- Created Date
);

-- Verify the waiver was added
SELECT 
    WaiverId,
    MemberId,
    StartYear,
    EndYear,
    Reason,
    CreatedDate
FROM DuesWaiverPeriods
WHERE MemberId = 111
ORDER BY WaiverId DESC;

PRINT 'Waiver added successfully for William Goveny (ID: 111) for years 2024-2025';
PRINT 'Reason: Kitchen hood work';
