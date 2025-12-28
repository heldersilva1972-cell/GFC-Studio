-- Add Hall Rental Pricing Matrix Columns
-- This script adds the new pricing columns to the WebsiteSettings table

USE ClubMembership;
GO

-- Check if columns already exist before adding them
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'WebsiteSettings' AND COLUMN_NAME = 'FunctionHallNonMemberRate')
BEGIN
    ALTER TABLE WebsiteSettings
    ADD FunctionHallNonMemberRate DECIMAL(18,2) NULL;
    PRINT 'Added FunctionHallNonMemberRate';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'WebsiteSettings' AND COLUMN_NAME = 'FunctionHallMemberRate')
BEGIN
    ALTER TABLE WebsiteSettings
    ADD FunctionHallMemberRate DECIMAL(18,2) NULL;
    PRINT 'Added FunctionHallMemberRate';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'WebsiteSettings' AND COLUMN_NAME = 'CoalitionNonMemberRate')
BEGIN
    ALTER TABLE WebsiteSettings
    ADD CoalitionNonMemberRate DECIMAL(18,2) NULL;
    PRINT 'Added CoalitionNonMemberRate';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'WebsiteSettings' AND COLUMN_NAME = 'CoalitionMemberRate')
BEGIN
    ALTER TABLE WebsiteSettings
    ADD CoalitionMemberRate DECIMAL(18,2) NULL;
    PRINT 'Added CoalitionMemberRate';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'WebsiteSettings' AND COLUMN_NAME = 'YouthOrganizationNonMemberRate')
BEGIN
    ALTER TABLE WebsiteSettings
    ADD YouthOrganizationNonMemberRate DECIMAL(18,2) NULL;
    PRINT 'Added YouthOrganizationNonMemberRate';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'WebsiteSettings' AND COLUMN_NAME = 'YouthOrganizationMemberRate')
BEGIN
    ALTER TABLE WebsiteSettings
    ADD YouthOrganizationMemberRate DECIMAL(18,2) NULL;
    PRINT 'Added YouthOrganizationMemberRate';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'WebsiteSettings' AND COLUMN_NAME = 'BartenderServiceFee')
BEGIN
    ALTER TABLE WebsiteSettings
    ADD BartenderServiceFee DECIMAL(18,2) NULL;
    PRINT 'Added BartenderServiceFee';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'WebsiteSettings' AND COLUMN_NAME = 'BaseFunctionHours')
BEGIN
    ALTER TABLE WebsiteSettings
    ADD BaseFunctionHours INT NULL;
    PRINT 'Added BaseFunctionHours';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'WebsiteSettings' AND COLUMN_NAME = 'AdditionalHourRate')
BEGIN
    ALTER TABLE WebsiteSettings
    ADD AdditionalHourRate DECIMAL(18,2) NULL;
    PRINT 'Added AdditionalHourRate';
END

GO

-- Now set default values
UPDATE WebsiteSettings
SET 
    FunctionHallNonMemberRate = 400,
    FunctionHallMemberRate = 300,
    CoalitionNonMemberRate = 200,
    CoalitionMemberRate = 100,
    YouthOrganizationNonMemberRate = 100,
    YouthOrganizationMemberRate = 100,
    BartenderServiceFee = 100,
    BaseFunctionHours = 5,
    AdditionalHourRate = 50
WHERE Id = 1;

GO

-- Verify the columns were added
SELECT 
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

GO

PRINT 'Migration completed successfully!';
