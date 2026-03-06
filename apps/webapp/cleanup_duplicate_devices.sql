-- ==========================================================
-- GFC SMART DEVICE CLEANUP (SMART ROTATION) - CORRECTED
-- ==========================================================
-- This script safely revokes redundant "duplicate" trust tokens 
-- while KEEPING the MOST RECENT active one for each user.
-- Updated to use the correct column name 'Id'.
-- ==========================================================

USE [ClubMembership];
GO

-- 1. Create a platform identifier to group similar devices together 
WITH PlatformGroupedDevices AS (
    SELECT 
        Id, 
        UserId, 
        DeviceToken,
        UserAgent,
        LastUsedUtc,
        IsRevoked,
        -- Detect the platform type
        CASE 
            WHEN UserAgent LIKE '%Android%' THEN 'Android'
            WHEN UserAgent LIKE '%iPhone%' THEN 'iPhone'
            WHEN UserAgent LIKE '%iPad%' THEN 'iPad'
            ELSE 'Browser'
        END as PlatformType,
        -- Rank devices for EACH USER and EACH PLATFORM by the most recent usage
        ROW_NUMBER() OVER (
            PARTITION BY UserId, 
                         CASE 
                            WHEN UserAgent LIKE '%Android%' THEN 'Android'
                            WHEN UserAgent LIKE '%iPhone%' THEN 'iPhone'
                            WHEN UserAgent LIKE '%iPad%' THEN 'iPad'
                            ELSE 'Browser'
                         END
            ORDER BY LastUsedUtc DESC
        ) as DeviceRank
    FROM TrustedDevices
    WHERE IsRevoked = 0 -- Only look at currently active ones
      AND IsStation = 0 -- DO NOT TOUCH SHARED WORKSTATIONS
)
-- 2. Revoke everything that is NOT the #1 most recent device 
UPDATE TrustedDevices
SET IsRevoked = 1
WHERE Id IN (
    SELECT Id 
    FROM PlatformGroupedDevices 
    WHERE DeviceRank > 1
);

PRINT 'Cleanup complete. Kept the most recent session for each user platform and revoked duplicates.';
GO
