-- [NEW]
-- GFC Member Data Migration Script
-- Phase 9: Stability, Performance Tuning & User Handover
-- Description: This script migrates legacy member records into the new SQL Server architecture.

-- Ensure this script is idempotent. It should be safe to run multiple times without creating duplicate data.

-- Step 1: Create a temporary table to hold the legacy member data.
-- This structure should match the legacy database's member table.
-- Replace the column definitions with the actual legacy schema.
IF OBJECT_ID('tempdb..#LegacyMembers') IS NOT NULL
    DROP TABLE #LegacyMembers;

CREATE TABLE #LegacyMembers (
    LegacyMemberID INT,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Email NVARCHAR(100),
    -- Add all other legacy columns here
    MembershipStatus NVARCHAR(20)
);

-- Step 2: Populate the temporary table with data from the legacy source.
-- This step will require a mechanism to access the legacy data, such as a linked server or a CSV import.
-- For the purpose of this script, we will use placeholder data.
-- In a real scenario, this would be replaced with an actual data import process.
-- INSERT INTO #LegacyMembers (LegacyMemberID, FirstName, LastName, Email, MembershipStatus)
-- VALUES
-- (1, 'John', 'Doe', 'john.doe@example.com', 'ACTIVE'),
-- (2, 'Jane', 'Smith', 'jane.smith@example.com', 'INACTIVE');


-- Step 3: Insert or update data in the new 'Members' table.
-- This assumes the new table is named 'Members' and is in the dbo schema.
-- The script will perform an 'upsert' operation, inserting new records and updating existing ones.

MERGE INTO dbo.Members AS Target
USING #LegacyMembers AS Source
ON Target.MemberID = Source.LegacyMemberID -- This assumes MemberID can be mapped directly. If not, another unique identifier should be used.

-- When a record exists in both the source and target tables, update the target record.
WHEN MATCHED THEN
    UPDATE SET
        Target.FirstName = Source.FirstName,
        Target.LastName = Source.LastName,
        Target.Email = Source.Email,
        Target.Status = CASE Source.MembershipStatus
                           WHEN 'ACTIVE' THEN 'REGULAR'
                           WHEN 'INACTIVE' THEN 'INACTIVE'
                           ELSE 'GUEST'
                         END;
        -- Map other columns as needed

-- When a record exists in the source table but not in the target table, insert a new record.
WHEN NOT MATCHED BY TARGET THEN
    INSERT (MemberID, FirstName, LastName, Email, Status)
    VALUES (Source.LegacyMemberID, Source.FirstName, Source.LastName, Source.Email,
            CASE Source.MembershipStatus
                WHEN 'ACTIVE' THEN 'REGULAR'
                WHEN 'INACTIVE' THEN 'INACTIVE'
                ELSE 'GUEST'
            END);
            -- Map other columns as needed

-- Step 4: Clean up the temporary table.
DROP TABLE #LegacyMembers;

PRINT 'Member data migration script completed successfully.';
