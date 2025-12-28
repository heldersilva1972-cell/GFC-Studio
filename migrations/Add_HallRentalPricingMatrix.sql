-- Add Detailed Hall Rental Pricing Fields to WebsiteSettings
-- Run this migration to add the new pricing matrix structure

USE ClubMembership;
GO

-- Add new pricing columns
ALTER TABLE WebsiteSettings
ADD FunctionHallNonMemberRate DECIMAL(18,2) NULL DEFAULT 400,
    FunctionHallMemberRate DECIMAL(18,2) NULL DEFAULT 300,
    CoalitionNonMemberRate DECIMAL(18,2) NULL DEFAULT 200,
    CoalitionMemberRate DECIMAL(18,2) NULL DEFAULT 100,
    YouthOrganizationNonMemberRate DECIMAL(18,2) NULL DEFAULT 100,
    YouthOrganizationMemberRate DECIMAL(18,2) NULL DEFAULT 100,
    BartenderServiceFee DECIMAL(18,2) NULL DEFAULT 100;
GO

-- Update existing record with default values
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

PRINT 'Hall Rental Pricing Matrix fields added successfully!';
