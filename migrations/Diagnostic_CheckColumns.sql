-- Diagnostic: Check if pricing columns actually exist
USE ClubMembership;

-- 1. Check if columns exist
SELECT 
    c.COLUMN_NAME,
    c.DATA_TYPE,
    c.IS_NULLABLE,
    c.COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS c
WHERE c.TABLE_NAME = 'WebsiteSettings'
ORDER BY c.ORDINAL_POSITION;

-- 2. Show actual table structure
EXEC sp_help 'WebsiteSettings';

-- 3. Try to select the specific columns
SELECT TOP 1
    Id,
    FunctionHallNonMemberRate,
    FunctionHallMemberRate,
    CoalitionNonMemberRate,
    CoalitionMemberRate,
    YouthOrganizationNonMemberRate,
    YouthOrganizationMemberRate,
    BartenderServiceFee
FROM WebsiteSettings;
