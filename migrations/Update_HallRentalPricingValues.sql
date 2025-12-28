-- Update Existing Hall Rental Pricing Fields with Default Values
-- Run this to populate the pricing matrix with baseline rates

USE ClubMembership;
GO

UPDATE WebsiteSettings
SET FunctionHallNonMemberRate = 400,
    FunctionHallMemberRate = 300,
    CoalitionNonMemberRate = 200,
    CoalitionMemberRate = 100,
    YouthOrganizationNonMemberRate = 100,
    YouthOrganizationMemberRate = 100,
    BartenderServiceFee = 100
WHERE Id = 1;
GO

SELECT 
    FunctionHallNonMemberRate,
    FunctionHallMemberRate,
    CoalitionNonMemberRate,
    CoalitionMemberRate,
    YouthOrganizationNonMemberRate,
    YouthOrganizationMemberRate,
    BartenderServiceFee
FROM WebsiteSettings
WHERE Id = 1;
GO

PRINT 'Hall Rental Pricing values updated successfully!';
