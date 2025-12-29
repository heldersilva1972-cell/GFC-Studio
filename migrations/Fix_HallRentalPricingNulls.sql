-- Comprehensive Fix for Hall Rental Pricing Fields
-- This script ensures all pricing fields have proper values

USE ClubMembership;
GO

-- First, check if the columns exist
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
           WHERE TABLE_NAME = 'WebsiteSettings' 
           AND COLUMN_NAME = 'FunctionHallNonMemberRate')
BEGIN
    PRINT 'Pricing columns exist. Updating values...';
    
    -- Update all pricing fields, handling NULLs
    UPDATE WebsiteSettings
    SET 
        FunctionHallNonMemberRate = ISNULL(FunctionHallNonMemberRate, 400),
        FunctionHallMemberRate = ISNULL(FunctionHallMemberRate, 300),
        CoalitionNonMemberRate = ISNULL(CoalitionNonMemberRate, 200),
        CoalitionMemberRate = ISNULL(CoalitionMemberRate, 100),
        YouthOrganizationNonMemberRate = ISNULL(YouthOrganizationNonMemberRate, 100),
        YouthOrganizationMemberRate = ISNULL(YouthOrganizationMemberRate, 100),
        BartenderServiceFee = ISNULL(BartenderServiceFee, 100),
        BaseFunctionHours = ISNULL(BaseFunctionHours, 5),
        AdditionalHourRate = ISNULL(AdditionalHourRate, 50)
    WHERE Id = 1;
    
    PRINT 'Pricing values updated successfully!';
    
    -- Show the results
    SELECT 
        Id,
        FunctionHallNonMemberRate,
        FunctionHallMemberRate,
        CoalitionNonMemberRate,
        CoalitionMemberRate,
        YouthOrganizationNonMemberRate,
        YouthOrganizationMemberRate,
        BartenderServiceFee,
        BaseFunctionHours,
        AdditionalHourRate
    FROM WebsiteSettings
    WHERE Id = 1;
END
ELSE
BEGIN
    PRINT 'ERROR: Pricing columns do not exist. Run the migration first.';
END
GO
