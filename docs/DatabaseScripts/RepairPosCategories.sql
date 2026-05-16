-- REPAIR POS CATEGORIES AND VISIBILITY
-- Run this on the PRODUCTION database to ensure the POS terminal can find items.

PRINT 'Starting POS Category & Visibility Repair...';

-- 1. Ensure all used categories exist in PosCategories table
INSERT INTO PosCategories (Name, IsActive, DisplayOrder, CreatedAt)
SELECT DISTINCT 
    Category, 
    1 as IsActive, 
    0 as DisplayOrder, 
    GETUTCDATE() as CreatedAt
FROM LiquorItems
WHERE Category IS NOT NULL 
  AND Category <> ''
  AND Category NOT IN (SELECT Name FROM PosCategories);

-- 2. Activate any deactivated categories that are currently in use
UPDATE PosCategories
SET IsActive = 1
WHERE Name IN (SELECT DISTINCT Category FROM LiquorItems WHERE IsActive = 1);

-- 3. Force ShowInPos for all active, priced items
UPDATE LiquorItems
SET ShowInPos = 1
WHERE IsActive = 1 
  AND RetailPrice > 0;

-- 4. Clean up the 'TOKENS' category if it exists in LiquorItems but we want it as a virtual tab
-- (The API adds it automatically, so we just ensure no items are accidentally assigned to it 
-- if we want them in BEER/LIQUOR etc.)

PRINT 'POS Repair Complete. 
Please refresh the POS terminal (or use the Rescue Menu to clear cache).';
